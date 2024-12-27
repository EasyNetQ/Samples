using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.Persistent;
using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Subscriber;

/// <summary>
/// Initializes a new instance of the <see cref="EasyNetQHostedService"/> class.
/// </summary>
/// <param name="bus">The bus instance used for subscribing to messages.</param>
/// <param name="logger">The logger instance used for logging information.</param>
public class EasyNetQHostedService(IBus bus, IServiceProvider provider, ILogger<EasyNetQHostedService> logger) : BackgroundService
{
    private SubscriptionResult _subscriber;

    /// <summary>
    /// Handles the text message.
    /// </summary>
    /// <param name="textMessage">The text message to handle.</param>
    async Task HandleTextMessage(TextMessage textMessage)
    {
        logger.LogInformation("Got message: {0}", textMessage.Text);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting subscriber application.");

        var subscriber = new AutoSubscriber(bus, provider, "test");

        await subscriber.SubscribeAsync([typeof(TestConsumer)], stoppingToken);
        //_subscriber = bus.PubSub.Subscribe<TextMessage>("test", HandleTextMessage, cancellationToken: stoppingToken);

        await bus.Advanced.EnsureConnectedAsync(PersistentConnectionType.Consumer, stoppingToken);
    }


}

public class TestConsumer(ILogger<TestConsumer> logger) : IConsume<TextMessage>
{
    public void Consume(TextMessage message, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Got message: {0}", message.Text);
    }
}