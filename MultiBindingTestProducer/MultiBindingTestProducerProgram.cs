using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;

namespace MultiBindingTestProducer
{
    class MultiBindingProducerProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Producer...");

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddEasyNetQ("host=rabbitmq").UseSystemTextJson();

            using var provider = serviceCollection.BuildServiceProvider();
            var bus = provider.GetRequiredService<IBus>();
            var advancedBus = bus.Advanced;

            var exchange = advancedBus.ExchangeDeclare(Messages.Constants.MultiBindingExchange, ExchangeType.Topic);

            // publish 1000 messages to each binding
            for (var i1 = 0; i1 < 10; i1++)
            {
                for (var i2 = 0; i2 < 10; i2++)
                {
                    for (var i3 = 0; i3 < 10; i3++)
                    {
                        var routingKey = $"{i1}.{i2}.{i3}";
                        Publish(routingKey);
                    }
                }
            }

            void Publish(string routingKey) =>
            bus.PubSub.Publish(new Messages.TextMessage
            {
                Text = routingKey
            }, configuration => configuration.WithTopic(routingKey));

            Console.WriteLine("Producer finished. Press Enter to quit");
            Console.ReadLine();
        }
    }
}
