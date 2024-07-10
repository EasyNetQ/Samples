using EasyNetQ;
using Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Subscriber;

class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        
        var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
        var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
        var connectionString = $"host={rabbitMqHost};username={rabbitMqUser};password={rabbitMqPass}";

        serviceCollection.AddEasyNetQ(connectionString).UseSystemTextJson();

        using var provider = serviceCollection.BuildServiceProvider();
        var bus = provider.GetRequiredService<IBus>();

        bus.PubSub.Subscribe<TextMessage>("test", HandleTextMessage);
        Console.WriteLine("Listening for messages. Hit <return> to quit.");
        Console.ReadLine();
    }

    static void HandleTextMessage(TextMessage textMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Got message: {0}", textMessage.Text);
        Console.ResetColor();
    }
}