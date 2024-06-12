using EasyNetQ;
using Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Publisher;

class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddEasyNetQ("host=localhost").UseSystemTextJson();

        using var provider = serviceCollection.BuildServiceProvider();
        var bus = provider.GetRequiredService<IBus>();

        var input = String.Empty;
        Console.WriteLine("Enter a message. 'Quit' to quit.");
        while ((input = Console.ReadLine()) != "Quit")
        {
            bus.PubSub.Publish(new TextMessage { Text = input });
            Console.WriteLine("Message published!");
        }
    }
}