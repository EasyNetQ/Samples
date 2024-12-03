using EasyNetQ;
using EasyNetQTest.ServiceDefaults;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();

var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
var connectionString = $"host={rabbitMqHost};username={rabbitMqUser};password={rabbitMqPass}";

builder.Services.AddEasyNetQ(connectionString).UseSystemTextJson();
builder.Services.AddLogging(loggingBuilder => loggingBuilder
    .ClearProviders()
    .AddProvider(new CustomConsoleLoggerProvider()));

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting subscriber application.");

var bus = app.Services.GetRequiredService<IBus>();

await bus.PubSub.SubscribeAsync<TextMessage>("test", HandleTextMessage);
logger.LogInformation("Listening for messages. Hit <return> to quit.");
Console.ReadLine();

logger.LogInformation("Subscriber application stopped.");

void HandleTextMessage(TextMessage textMessage)
{
    logger.LogInformation("Got message: {0}", textMessage.Text);
}