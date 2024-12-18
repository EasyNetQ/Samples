using Microsoft.Extensions.Logging;

namespace CustomLogger;


/// <summary>
/// Provides a custom console logger.
/// </summary>
public class CustomConsoleLoggerProvider : ILoggerProvider
{
    /// <summary>
    /// Creates a new instance of the custom console logger.
    /// </summary>
    /// <param name="categoryName">The category name for the logger.</param>
    /// <returns>An instance of <see cref="ILogger"/>.</returns>
    public ILogger CreateLogger(string categoryName)
    {
        return new CustomConsoleLogger(categoryName);
    }

    /// <summary>
    /// Disposes the resources used by the custom console logger provider.
    /// </summary>
    public void Dispose() { }
}