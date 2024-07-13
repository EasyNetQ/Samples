using EasyNetQ;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging(builder => builder
    .ClearProviders()
    .AddProvider(new CustomConsoleLoggerProvider()));

var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
var connectionString = $"host={rabbitMqHost};username={rabbitMqUser};password={rabbitMqPass}";

serviceCollection.AddEasyNetQ(connectionString).UseSystemTextJson();
using var provider = serviceCollection.BuildServiceProvider();

var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Starting subscriber application.");

IBus bus = provider.GetRequiredService<IBus>();

bus.PubSub.Subscribe<TextMessage>("test", HandleTextMessage);
logger.LogInformation("Listening for messages. Hit <return> to quit.");
Console.ReadLine();

logger.LogInformation("Subscriber application stopped.");

void HandleTextMessage(TextMessage textMessage)
{
    logger.LogInformation("Got message: {0}", textMessage.Text);
}
