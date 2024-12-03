using EasyNetQ;
using EasyNetQTest.ServiceDefaults;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;
builder.AddServiceDefaults();

builder.AddRabbitMQClient(
    "messaging",
    static settings => settings.DisableHealthChecks = true);

var connectionString = configuration.GetValue<string>("Aspire:RabbitMQ:Client:ConnectionString");
builder.Services.AddEasyNetQ(connectionString).UseSystemTextJson();
builder.Services.AddLogging(loggingBuilder => loggingBuilder
    .ClearProviders()
    .AddProvider(new CustomConsoleLoggerProvider()));

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting subscriber application.");

var bus = app.Services.GetRequiredService<IBus>();

await bus.PubSub.SubscribeAsync<TextMessage>("test", HandleTextMessage);
logger.LogInformation("Listening for messages. Hit <return> to quit.");
logger.LogInformation("Subscriber application stopped.");

void HandleTextMessage(TextMessage textMessage)
{
    logger.LogInformation("Got message: {0}", textMessage.Text);
}

await app.RunAsync();