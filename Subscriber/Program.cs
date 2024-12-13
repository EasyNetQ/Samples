using EasyNetQ;
using EasyNetQTest.ServiceDefaults;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Subscriber;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;
builder.AddServiceDefaults();

string connectionString = null;

builder.AddRabbitMQClient(
    "messaging",
    settings =>
    {
        settings.DisableHealthChecks = true;
        connectionString = settings.ConnectionString;
    }
    );

// var connectionString = configuration.GetValue<string>("Aspire:RabbitMQ:Client:ConnectionString");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("RabbitMQ connection string is not configured.");
}
builder.Services.AddEasyNetQ(connectionString).UseSystemTextJson();
builder.Services.AddLogging(loggingBuilder => loggingBuilder
    .ClearProviders()
    .AddProvider(new CustomConsoleLoggerProvider()));

builder.Services.AddHostedService<EasyNetQHostedService>();

var app = builder.Build();

await app.RunAsync();