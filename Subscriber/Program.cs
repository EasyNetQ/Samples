using EasyNetQ;
using EasyNetQSample.ServiceDefaults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Subscriber;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;
builder.AddServiceDefaults();

string connectionString = string.Empty;

builder.AddRabbitMQClient(
    "messaging",
    settings =>
    {
        settings.DisableHealthChecks = true;
        connectionString = settings.ConnectionString ?? string.Empty;
    }
    );

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("RabbitMQ connection string is not configured.");
}
builder.Services.AddEasyNetQ(connectionString).UseSystemTextJson();
builder.Services.AddLogging(loggingBuilder => loggingBuilder
    .ClearProviders()
    .AddConsole()
    .AddOpenTelemetry(logging => { logging.IncludeFormattedMessage = true; }));

builder.Services.AddHostedService<EasyNetQHostedService>();

var app = builder.Build();

await app.RunAsync();