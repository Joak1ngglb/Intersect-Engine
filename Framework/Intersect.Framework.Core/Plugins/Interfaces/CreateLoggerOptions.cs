
using Microsoft.Extensions.Logging;

namespace Intersect.Plugins.Interfaces;

/// <summary>
/// Configuration options for creating <see cref="Microsoft.Extensions.Logging.Logger{T}"/>s.
/// </summary>
public record struct CreateLoggerOptions
{
    /// <summary>
    /// The minimum <see cref="LogLevel"/> for console output, set to <see cref="LogLevel.None"/> to disable.
    /// </summary>
    public LogLevel Console { get; init; }

    /// <summary>
    /// The minimum <see cref="LogLevel"/> for file output, set to <see cref="LogLevel.None"/> to disable.
    /// </summary>
    public LogLevel File { get; init; }

    /// <summary>
    /// The name of the created <see cref="Microsoft.Extensions.Logging.Logger{T}"/>.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// The context type of the logger.
    /// </summary>
    public Type ContextType { get; init; }
}