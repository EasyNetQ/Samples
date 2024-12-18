using EasyNetQ;
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

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("RabbitMQ connection string is not configured.");
}

builder.Services.AddEasyNetQ(connectionString).UseSystemTextJson();

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
