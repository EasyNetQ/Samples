using EasyNetQ;
using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Subscriber;

public class EasyNetQHostedService(IBus bus, ILogger<EasyNetQHostedService> logger) : IHostedLifecycleService
{
    private SubscriptionResult _subscriber;

    void HandleTextMessage(TextMessage textMessage)
    {
        logger.LogInformation("Got message: {0}", textMessage.Text);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting subscriber application.");
        _subscriber = await bus.PubSub.SubscribeAsync<TextMessage>("test", HandleTextMessage, cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscriber application stop.");
        _subscriber.Dispose();
        return Task.CompletedTask;
    }

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscriber application started");
        return Task.CompletedTask;
    }

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscriber application starting");
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscriber application stopped");
        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscriber application stopping");
        return Task.CompletedTask;
    }
}