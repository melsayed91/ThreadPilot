using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    private readonly Action<ILogger, string, object, Exception?> _handlingRequest;
    private readonly Action<ILogger, string, long, Exception?> _handledRequest;
    private readonly Action<ILogger, string, long, Exception?> _errorHandlingRequest;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;

        _handlingRequest = LoggerMessage.Define<string, object>(
            LogLevel.Information,
            new EventId(1, "HandlingRequest"),
            "Handling {RequestName} with payload {@Request}");

        _handledRequest = LoggerMessage.Define<string, long>(
            LogLevel.Information,
            new EventId(2, "HandledRequest"),
            "Handled {RequestName} in {Elapsed}ms");

        _errorHandlingRequest = LoggerMessage.Define<string, long>(
            LogLevel.Error,
            new EventId(3, "ErrorHandlingRequest"),
            "Error handling {RequestName} after {Elapsed}ms");
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(next);

        var name = typeof(TRequest).Name;
        var sw = Stopwatch.StartNew();

        try
        {
            _handlingRequest(_logger, name, request, null);

            var response = await next().ConfigureAwait(false);

            sw.Stop();
            _handledRequest(_logger, name, sw.ElapsedMilliseconds, null);
            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            _errorHandlingRequest(_logger, name, sw.ElapsedMilliseconds, ex);
            throw;
        }
    }
}
