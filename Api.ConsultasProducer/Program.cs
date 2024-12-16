using Domain.Inputs;
using Scalar.AspNetCore;
using Tools;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

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
    options.WithTitle("Consultas Producer");
    options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    options.Servers = [];
});

app.UseHttpsRedirection();


app.MapGet("/teste", (IConnection messageConnection, IConfiguration configuration, string text) =>
{
    const string configKeyName = "RabbitMQ:QueueName";
    string? queueName = configuration[configKeyName];

    using var channel = messageConnection.CreateModel();
    channel.QueueDeclare(queueName, exclusive: false);

    var corrId = Guid.NewGuid().ToString();
    var body = Encoding.UTF8.GetBytes($"Consulta: {corrId} - {text}");

    var properties = channel.CreateBasicProperties();
    properties.CorrelationId = corrId;

    channel.BasicPublish(
        exchange: "",
        routingKey: queueName,
        basicProperties: properties,
        body: JsonSerializer.SerializeToUtf8Bytes(body));

    var consumer = new EventingBasicConsumer(channel);

    var result = channel.BasicConsume(queue: queueName, consumer: consumer, autoAck: true);

    //var response = Encoding.UTF8.GetString(result);

    return result;
})
.WithName("Teste");

app.Run();