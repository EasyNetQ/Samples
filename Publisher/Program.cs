﻿using EasyNetQ;
using EasyNetQTest.ServiceDefaults;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// Add Aspire's service defaults
builder.AddServiceDefaults();

// Configure RabbitMQ connection
var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
var connectionString = $"host={rabbitMqHost};username={rabbitMqUser};password={rabbitMqPass}";

// Add EasyNetQ with System.Text.Json serialization
builder.Services.AddEasyNetQ(connectionString).UseSystemTextJson();

// Configure custom logging
builder.Services.AddLogging(loggingBuilder => loggingBuilder
    .ClearProviders()
    .AddConsole());

// Build the application
var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting publisher application...");

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
