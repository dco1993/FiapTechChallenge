namespace Worker.CadastroConsumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using Domain.Entity;
using Domain.Inputs;
using Domain.Repository;
using Tools;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _messageConnection;
    private IModel? _messageChannel;
    private IContatoRepository _contatoRepository;

    public Worker(ILogger<Worker> logger, 
                  IConfiguration config, 
                  IServiceProvider serviceProvider, 
                  IContatoRepository contatoRepository)
    {
        _logger = logger;
        _config = config;
        _serviceProvider = serviceProvider;
        _contatoRepository = contatoRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            const string configKeyName = "RabbitMQ:QueueName";
            string queueName = _config[configKeyName] ?? "cadastro";

            _messageConnection = _serviceProvider.GetService<IConnection>();

            _messageChannel = _messageConnection!.CreateModel();
            _messageChannel.QueueDeclare(queueName, exclusive: false);

            var consumer = new EventingBasicConsumer(_messageChannel);
            consumer.Received += ProcessMessageAsync;

            _messageChannel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);

            await Task.Delay(10000, stoppingToken);
        }
    }

    private void ProcessMessageAsync(object? sender, BasicDeliverEventArgs args)
    {
        _logger.LogInformation($"Processando mensagem as: {DateTime.UtcNow} | Mensagem de Id: {args.BasicProperties.MessageId}");

        var mensagem = args.Body;

        var contato = JsonSerializer.Deserialize<ContatoInput>(mensagem.Span);

        try
        {
            _logger.LogInformation($"Processando novo contato: {contato!.NomeCompleto}");

            Contato novoContato = new() {
                NomeCompleto = contato.NomeCompleto,
                TelefoneDdd = contato.TelefoneDdd,
                TelefoneNum = contato.TelefoneNum,
                Email = contato.Email
            };

            string validacao = Validacoes.ValidaContato(novoContato);

            if (string.IsNullOrEmpty(validacao))
            {
                _contatoRepository.Cadastrar(novoContato);
            }
            else
                throw new Exception(validacao);

            _logger.LogInformation($"Contato {contato!.NomeCompleto} cadastrado com sucesso.");

        }
        catch (Exception ex) 
        {
            _logger.LogError($"Erro ao cadastrar contato : {contato!.NomeCompleto} | Erro: {ex.Message}");
        }
    }
}
