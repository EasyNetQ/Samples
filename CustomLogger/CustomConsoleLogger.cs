using Microsoft.Extensions.Logging;

namespace CustomLogger;


/// <summary>
/// A custom console logger implementation.
/// </summary>
public class CustomConsoleLogger : ILogger
{
    private readonly string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomConsoleLogger"/> class.
    /// </summary>
    /// <param name="name">The name of the logger.</param>
    public CustomConsoleLogger(string name)
    {
        _name = name;
    }

    /// <summary>
    /// Begins a logical operation scope.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <param name="state">The identifier for the scope.</param>
    /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
    IDisposable? ILogger.BeginScope<TState>(TState state) => null;

    /// <summary>
    /// Checks if the given log level is enabled.
    /// </summary>
    /// <param name="logLevel">The log level to check.</param>
    /// <returns><c>true</c> if the log level is enabled; otherwise, <c>false</c>.</returns>
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <summary>
    /// Logs a message.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <param name="logLevel">The log level.</param>
    /// <param name="eventId">The event ID.</param>
    /// <param name="state">The state.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="formatter">The formatter function.</param>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var originalColor = Console.ForegroundColor;
        var logMessage = formatter(state, exception);

        if (logLevel == LogLevel.Information && (logMessage.StartsWith("Got message:") || logMessage.StartsWith("Message published!")))
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else
        {
            Console.ForegroundColor = GetLogLevelConsoleColor(logLevel);
        }

        Console.WriteLine($"{logLevel}: {logMessage}");

        Console.ForegroundColor = originalColor;
    }

    private ConsoleColor GetLogLevelConsoleColor(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Information => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Critical => ConsoleColor.DarkRed,
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Trace => ConsoleColor.DarkGray,
            _ => Console.ForegroundColor
        };
    }
}