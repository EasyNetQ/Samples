using Microsoft.Extensions.Logging;
using System;

public class CustomConsoleLogger : ILogger
{
    private readonly string _name;

    public CustomConsoleLogger(string name)
    {
        _name = name;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
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