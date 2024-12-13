using EasyNetQ;
using EasyNetQTest.ServiceDefaults;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
    .AddConsole());

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting publisher application...");


var bus = app.Services.GetRequiredService<IBus>();

var input = string.Empty;
logger.LogInformation("Enter a message. 'Quit' to quit.");
while ((input = Console.ReadLine()) != "Quit")
{
    await bus.PubSub.PublishAsync(new TextMessage { Text = input });
    logger.LogInformation("Message published!");
}

logger.LogInformation("Publisher application stopped.");

await app.RunAsync();