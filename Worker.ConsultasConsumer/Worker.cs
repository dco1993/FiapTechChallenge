namespace Worker.ConsultasConsumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using Domain.Entity;
using Domain.Inputs;
using Domain.Repository;
using Tools;
using System.Text;
using System.Threading.Channels;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger; 
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _messageConnection;
    private IModel? _messageChannel;

    public Worker(ILogger<Worker> logger,
                  IConfiguration config,
                  IServiceProvider serviceProvider)
    {
        _logger = logger;
        _config = config;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            const string configKeyName = "RabbitMQ:QueueName";
            string queueName = _config[configKeyName] ?? "consulta-contato";

            _messageConnection = _serviceProvider.GetService<IConnection>();

            _messageChannel = _messageConnection!.CreateModel();
            _messageChannel.QueueDeclare(queueName, exclusive: false);

            var consumer = new EventingBasicConsumer(_messageChannel);

            consumer.Received += (model, ea) => {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("Received: {0}", message);

                // Processa a consulta e retorna o resultado 
                var result = "Resultado da Consulta: " + message; 
                var response = Encoding.UTF8.GetBytes(result);

                _messageChannel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: response);
            };

            await Task.Delay(1000, stoppingToken);
        }
    }
}
