var builder = DistributedApplication.CreateBuilder(args);


var rabbitmq = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin();

builder.AddProject<Projects.Api_ConsultasProducer>("api-consultasproducer")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

builder.AddProject<Projects.Api_CadastroProducer>("api-cadastroproducer")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

builder.AddProject<Projects.Worker_CadastroConsumer>("worker-cadastroconsumer")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

builder.AddProject<Projects.Worker_ConsultasConsumer>("worker-consultasconsumer")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);





builder.Build().Run();
