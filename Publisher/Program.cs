using EasyNetQ;
using EasyNetQ.Persistent;
using EasyNetQSample.ServiceDefaults;
using Messages;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = string.Empty;

builder.AddRabbitMQClient(
    "messaging",
    settings =>
    {
        settings.DisableHealthChecks = true;
        connectionString = settings.ConnectionString ?? string.Empty;
    }
);


builder.Services.AddSingleton( new ConnectionConfiguration {
        ClientName = "publisher"
});
builder.Services.AddEasyNetQ().UseSystemTextJson();

builder.Services.AddHostedService<PublisherHostedService>();

builder.Services.AddLogging(loggingBuilder => loggingBuilder
    .ClearProviders()
    .AddConsole()
    .AddOpenTelemetry(logging => { logging.IncludeFormattedMessage = true; }));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.MapPost("/publish", async (IBus bus, string message, ILogger<Program> logger) =>
{
    if (string.IsNullOrWhiteSpace(message))
    {
        return Results.BadRequest("Message cannot be empty.");
    }

    await bus.PubSub.PublishAsync(new TextMessage { Text = message });
    logger.LogInformation("Message published: {Message}", message);
    return Results.Ok("Message published successfully.");
})
.WithName("PublishMessage");

app.Run();

public class PublisherHostedService(IBus bus) : IHostedLifecycleService
{
    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task StartedAsync(CancellationToken cancellationToken)
    {
        await bus.Advanced.EnsureConnectedAsync(PersistentConnectionType.Producer, cancellationToken);
    }

    public Task StartingAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public Task StoppedAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public Task StoppingAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
