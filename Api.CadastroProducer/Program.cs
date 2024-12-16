using Domain.Entity;
using Domain.Inputs;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Tools;
using Prometheus;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddRabbitMQClient(connectionName: "messaging");

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
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