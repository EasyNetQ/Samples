var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", ()=>"guest", secret: true);
var password = builder.AddParameter("password", ()=>"guest", secret: true);

var rabbitmq = builder
    .AddRabbitMQ("messaging", username,  password)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume(isReadOnly: false)
    .WithManagementPlugin();

builder.AddProject<Projects.Publisher>("publisher")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);
builder.AddProject<Projects.Subscriber>("subscriber")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

await builder.Build().RunAsync();
