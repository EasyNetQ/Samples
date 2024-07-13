using Microsoft.Extensions.Logging;

public class CustomConsoleLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new CustomConsoleLogger(categoryName);
    }

    public void Dispose() { }
}