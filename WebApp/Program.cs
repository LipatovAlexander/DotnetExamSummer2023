using WebApp;
using WebApp.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddQueryType<ItemQuery>()
    .AddMutationType<ItemMutation>();

builder.Services.AddSingleton<IProducer, Producer>();

var app = builder.Build();

app.MapBananaCakePop();
app.MapGraphQL();

app.Run();