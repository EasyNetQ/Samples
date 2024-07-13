using EasyNetQ;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();

var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
var connectionString = $"host={rabbitMqHost};username={rabbitMqUser};password={rabbitMqPass}";

serviceCollection.AddLogging(builder => builder.AddConsole());
serviceCollection.AddEasyNetQ(connectionString).UseSystemTextJson();
using var provider = serviceCollection.BuildServiceProvider();

var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Starting publisher application.");

IBus bus = provider.GetRequiredService<IBus>();

var input = string.Empty;
logger.LogInformation("Enter a message. 'Quit' to quit.");
while ((input = Console.ReadLine()) != "Quit")
{

    await bus.PubSub.PublishAsync(new TextMessage { Text = input });
    logger.LogInformation("Message published!");
}

logger.LogInformation("Publisher application stopped.");
