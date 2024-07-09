using EasyNetQ;
using Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Subscriber;

class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddEasyNetQ("host=rabbitmq").UseSystemTextJson();

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