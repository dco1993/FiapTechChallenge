using Domain.Inputs;
using Scalar.AspNetCore;
using Tools;
using RabbitMQ.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.AddRabbitMQClient(connectionName: "messaging");

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapScalarApiReference(options =>
{
    options.WithLayout(ScalarLayout.Classic);
    options.WithTheme(ScalarTheme.BluePlanet);
    options.WithTitle("Cadastro Producer");
    options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    options.Servers = [];
});

app.UseHttpsRedirection();


app.MapPost("/cadastrarContato", (IConnection messageConnection, IConfiguration configuration, ContatoInput novoContato) =>
{
    const string configKeyName = "RabbitMQ:QueueName";
    string? queueName = configuration[configKeyName];

    using var channel = messageConnection.CreateModel();
    channel.QueueDeclare(queueName, exclusive: false);
    channel.BasicPublish(
        exchange: "",
        routingKey: queueName,
        basicProperties: null,
        body: JsonSerializer.SerializeToUtf8Bytes(novoContato));
})
 .WithName("CadastrarContato")
 .WithTags("CadastrarContato")
 .WithOpenApi();

app.Run();