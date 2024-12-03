var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Publisher>("publisher");
builder.AddProject<Projects.Subscriber>("subscriber");

builder.Build().Run();
