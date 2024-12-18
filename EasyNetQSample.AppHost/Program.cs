var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder
    .AddRabbitMQ("messaging")
    .WithManagementPlugin();

builder.AddProject<Projects.Publisher>("publisher")
    .WithReference(rabbitmq);
builder.AddProject<Projects.Subscriber>("subscriber")
    .WithReference(rabbitmq);

await builder.Build().RunAsync();
