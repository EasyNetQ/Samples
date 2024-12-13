using EasyNetQ;
using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class EasyNetQHostedService(IBus bus, ILogger<EasyNetQHostedService> logger) : IHostedLifecycleService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting publisher application.");

        var input = string.Empty;

        while (!cancellationToken.IsCancellationRequested && (input = Console.ReadLine()) != "Quit")
        {
            await bus.PubSub.PublishAsync(new TextMessage { Text = input }, cancellationToken: cancellationToken);
            logger.LogInformation("Message published!");
            Console.WriteLine("Enter a message. 'Quit' to quit.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("publisher application stop.");
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