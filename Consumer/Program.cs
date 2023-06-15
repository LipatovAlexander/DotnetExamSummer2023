using Confluent.Kafka;
using Consumer;
using Consumer.Configurations;
using Consumer.Messages;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=54321;Username=postgres;Password=postgrespw;Database=items");
});

builder.Services.AddSingleton(sp =>
{
    var config = new AdminClientConfig { BootstrapServers = "localhost:9092" };
    return new AdminClientBuilder(config).Build();
});

builder.Services.AddSingleton(sp =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = "localhost:9092",
        GroupId = "group",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    return new ConsumerBuilder<Ignore, IMessage>(config)
        .SetValueDeserializer(new MessageSerializer())
        .Build();
});

builder.Services.AddHostedService<MessageConsumer>();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());

var app = builder.Build();

app.MapHealthChecks("/healthz");

app.Run();