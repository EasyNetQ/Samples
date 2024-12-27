using EasyNetQ;
using EasyNetQ.Persistent;
using EasyNetQSample.ServiceDefaults;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Subscriber;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;
builder.AddServiceDefaults();

string connectionString = string.Empty;

builder.Services.AddSingleton(new HostOptions {
    ServicesStartConcurrently = true,
    ServicesStopConcurrently = false,
});
builder.AddRabbitMQClient(
    "messaging",
    settings =>
    {
        settings.DisableHealthChecks = true;
        connectionString = settings.ConnectionString ?? string.Empty;
    }
    );
builder.Services.AddSingleton( new ConnectionConfiguration {
    ClientName = "subscriber"
});
builder.Services.AddEasyNetQ().UseSystemTextJson();
builder.Services.AddLogging(loggingBuilder => loggingBuilder
    .ClearProviders()
    .AddConsole()
    .AddOpenTelemetry(logging => { logging.IncludeFormattedMessage = true; }));

builder.Services.AddHostedService<EasyNetQHostedService>();

var app = builder.Build();

await app.RunAsync();


