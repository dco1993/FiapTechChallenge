using Worker.ConsultasConsumer;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker.ConsultasConsumer.Worker>();

builder.AddRabbitMQClient(connectionName: "messaging");

var host = builder.Build();
host.Run();
