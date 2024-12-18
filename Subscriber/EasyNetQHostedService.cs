using EasyNetQ;
using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Subscriber;

/// <summary>
/// Initializes a new instance of the <see cref="EasyNetQHostedService"/> class.
/// </summary>
/// <param name="bus">The bus instance used for subscribing to messages.</param>
/// <param name="logger">The logger instance used for logging information.</param>
public class EasyNetQHostedService(IBus bus, ILogger<EasyNetQHostedService> logger) : IHostedLifecycleService
{
    private SubscriptionResult _subscriber;

    /// <summary>
    /// Handles the text message.
    /// </summary>
    /// <param name="textMessage">The text message to handle.</param>
    void HandleTextMessage(TextMessage textMessage)
    {
        logger.LogInformation("Got message: {0}", textMessage.Text);
    }

    /// <summary>
    /// Starts the subscriber application.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous start operation.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting subscriber application.");
        _subscriber = await bus.PubSub.SubscribeAsync<TextMessage>("test", HandleTextMessage, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Stops the subscriber application.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous stop operation.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscriber application stop.");
        _subscriber.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the application started event.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous started operation.</returns>
    public Task StartedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscriber application started");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the application starting event.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous starting operation.</returns>
    public Task StartingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscriber application starting");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the application stopped event.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous stopped operation.</returns>
    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscriber application stopped");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the application stopping event.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous stopping operation.</returns>
    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Subscriber application stopping");
        return Task.CompletedTask;
    }
}