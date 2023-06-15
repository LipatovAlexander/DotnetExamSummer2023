using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using WebApp;
using WebApp.Configurations;
using WebApp.GraphQL;
using WebApp.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql("Host=localhost;Port=54321;Username=postgres;Password=postgrespw;Database=items");
});

builder.Services.AddGraphQLServer()
    .AddQueryType<ItemQuery>()
    .AddMutationType<ItemMutation>();
    
builder.Services.AddSingleton(sp =>
{
    var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

    return new ProducerBuilder<Null, IMessage>(config)
        .SetValueSerializer(new MessageSerializer())
        .Build();
});

builder.Services.AddSingleton<IMessageProducer, MessageProducer>();

var app = builder.Build();

app.MapBananaCakePop();
app.MapGraphQL();

app.MapHealthChecks("/healthz");

app.Run();