using EasyNetQ;
using EasyNetQTest.ServiceDefaults;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

var configuration = builder.Configuration;

// Add Aspire's service defaults
builder.AddServiceDefaults();

builder.AddRabbitMQClient(
    "messaging",
    static settings => settings.DisableHealthChecks = true);


// Add EasyNetQ with System.Text.Json serialization
builder.Services.AddEasyNetQ(configuration.GetValue<string>("Aspire:RabbitMQ:Client:ConnectionString")).UseSystemTextJson();

// Configure custom logging
builder.Services.AddLogging(loggingBuilder => loggingBuilder
    .ClearProviders()
    .AddConsole());

// Build the application
var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting publisher application...");

await app.RunAsync();

// Get the EasyNetQ bus
var bus = app.Services.GetRequiredService<IBus>();

var input = string.Empty;
logger.LogInformation("Enter a message. 'Quit' to quit.");
while ((input = Console.ReadLine()) != "Quit")
{
    await bus.PubSub.PublishAsync(new TextMessage { Text = input });
    logger.LogInformation("Message published!");
}

logger.LogInformation("Publisher application stopped.");

