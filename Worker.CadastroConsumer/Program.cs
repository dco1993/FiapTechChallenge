using Domain.Repository;
using Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json").Build();

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker.CadastroConsumer.Worker>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
}, ServiceLifetime.Singleton);

builder.Services.AddSingleton<IContatoRepository, ContatoRepository>();

builder.AddRabbitMQClient(connectionName: "messaging");

var host = builder.Build();
host.Run();
