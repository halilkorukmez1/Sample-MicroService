using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace EventBus.RabbitMQ;
public class RabbitMQConnection : IDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection _connection;
    public RabbitMQConnection(IConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;
    public bool TryConnect()
    {
        try
        {
            lock (new object())
            {
                Policy.Handle<BrokerUnreachableException>()
                   .Or<SocketException>()
                   .WaitAndRetry(0, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { })
                   .Execute(() => _connection = _connectionFactory.CreateConnection());

                if (IsConnected())
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;
                    return true;
                }
                return false;
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.InnerException.InnerException.InnerException.ToString());
        }

    }
    public IModel CreateModel() => _connection.CreateModel();
    public void Dispose() => _connection.Dispose();
    public bool IsConnected() => _connection != null && _connection.IsOpen;
    void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e) => TryConnect();
    void OnCallbackException(object sender, CallbackExceptionEventArgs e) => TryConnect();
    void OnConnectionShutdown(object sender, ShutdownEventArgs reason) => TryConnect();
}