using EasyNetQ;
using Messages;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
var connectionString = $"host={rabbitMqHost};username={rabbitMqUser};password={rabbitMqPass}";

serviceCollection.AddEasyNetQ(connectionString).UseSystemTextJson();
using var provider = serviceCollection.BuildServiceProvider();
var bus = provider.GetRequiredService<IBus>();

var input = String.Empty;
Console.WriteLine("Enter a message. 'Quit' to quit.");
while ((input = Console.ReadLine()) != "Quit")
{
    bus.PubSub.Publish(new TextMessage { Text = input });
    Console.WriteLine("Message published!");
}
