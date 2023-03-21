using EventBus.Base.Abstraction;
using EventBus.Base.Config;
using EventBus.Base.Enum;
using EventBus.Manager;
using Microsoft.Extensions.Configuration;
using Product.Infrastructure.Outbox;
using Product.Infrastructure.Outbox.Stores;
using Product.Infrastructure.Outbox.Stores.MongoDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig busConfig = new()
    {
        HostName = builder.Configuration.GetSection("RabbitConfig:Url").Value,
        UserName = builder.Configuration.GetSection("RabbitConfig:Name").Value,
        Password = builder.Configuration.GetSection("RabbitConfig:Password").Value,
    };
    return EventBusManager.Create(new EventBusConfig
    {
        Connection = busConfig,
        EventNameSuffix = builder.Configuration.GetSection("RabbitConfig:EventNameSuffix").Value,
        SubscriberServiceName = builder.Configuration.GetSection("RabbitConfig:SubscriberServiceName").Value,
        EventBusType = (EventBusType)Convert.ToInt32(builder.Configuration.GetSection("RabbitConfig:EventBusType").Value)
    }, sp);
});
var options = new MongoDbOutboxOptions();
builder.Configuration.GetSection(nameof(OutboxOptions)).Bind(options);
builder.Services.Configure<MongoDbOutboxOptions>(builder.Configuration.GetSection(nameof(OutboxOptions)));
builder.Services.AddScoped<IOutboxStore, MongoDbOutboxStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
