using EventBus.Base;
using EventBus.Base.Config;
using EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;

namespace EventBus.RabbitMQ;
public class RabbitMQManager : BaseEventBus
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IModel _consumerChannel;
    RabbitMQConnection rabbitMQConnection;
    public RabbitMQManager(EventBusConfig eventBusConfig, IServiceProvider serviceProvider) : base(eventBusConfig, serviceProvider)
    {
        _connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(
                             JsonConvert.SerializeObject(EventBusConfig.Connection,
                             new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

        rabbitMQConnection = new RabbitMQConnection(_connectionFactory);
        _consumerChannel = CreateConsumerChannel();
        _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
    }
    public override void Publish(IntegrationEvent @event)
    {
        if (!rabbitMQConnection.IsConnected()) rabbitMQConnection.TryConnect();
        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { });
      
        var eventName = ProcessEventName(@event.GetType().Name);
       
        _consumerChannel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName, type: "direct");
       
        policy.Execute(() =>
        {
            var properties = _consumerChannel.CreateBasicProperties();
            properties.DeliveryMode = 2;
            _consumerChannel.BasicPublish(exchange: EventBusConfig.DefaultTopicName,
                                          routingKey: eventName,
                                          mandatory: true,
                                          basicProperties: properties,
                                          body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)));
        });
    }
    public override void Subscribe<TEvent, TEventHandler>()
    {
        var eventName = ProcessEventName(typeof(TEvent).Name);
        if (!_subsManager.IsSubscriptionsForEvent(eventName))
        {
            if (!rabbitMQConnection.IsConnected()) rabbitMQConnection.TryConnect();
            _consumerChannel.QueueDeclare(queue: GetSubName(eventName),
                                          durable: true,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null);
            _consumerChannel.QueueBind(queue: GetSubName(eventName),
                                       exchange: EventBusConfig.DefaultTopicName,
                                       routingKey: eventName);
        }
        _subsManager.AddSubscription<TEvent, TEventHandler>();
        StartBasicConsume(eventName);
    }
    public override void UnSubscribe<TEvent, TEventHandler>() => _subsManager.RemoveSubscription<TEvent, TEventHandler>();
    private void StartBasicConsume(string eventName)
    {
        if (_consumerChannel != null)
        {
            var consumer = new EventingBasicConsumer(_consumerChannel);
            consumer.Received += Consumer_Received;
            _consumerChannel.BasicConsume(queue: GetSubName(eventName),
                                          autoAck: false,
                                          consumer: consumer);
        }
    }
    public IModel CreateConsumerChannel()
    {
        if (!rabbitMQConnection.IsConnected()) rabbitMQConnection.TryConnect();
        var channel = rabbitMQConnection.CreateModel();
        channel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName, type: "direct");
        return channel;
    }
    private async void Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = ProcessEventName(eventArgs.RoutingKey);
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
        try
        {
            await ProcessEvent(eventName, message);
        }
        catch (Exception e)
        {
            //logging
        }
        _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
    }
    private void SubsManager_OnEventRemoved(object sender, string eventName)
    {
        eventName = ProcessEventName(eventName);
        if (!rabbitMQConnection.IsConnected())
            rabbitMQConnection.TryConnect();
        _consumerChannel.QueueUnbind(queue: eventName, exchange: EventBusConfig.DefaultTopicName, routingKey: eventName);
        if (_subsManager.IsEmpty) _consumerChannel.Close();
    }
}