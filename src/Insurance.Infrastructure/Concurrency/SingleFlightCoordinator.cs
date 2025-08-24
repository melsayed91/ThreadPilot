using System.Collections.Concurrent;

namespace Insurance.Infrastructure.Concurrency;

/// <summary>
/// Ensures only one Task per key runs at a time; concurrent callers await the same Task.
/// On completion, the entry is removed to avoid caching results (this is NOT a cache).
/// </summary>
public sealed class SingleFlightCoordinator
{
    private readonly ConcurrentDictionary<string, Lazy<Task<object?>>> _inflight = new();

    public async Task<T> RunAsync<T>(string key, Func<CancellationToken, Task<T>> factory, CancellationToken ct)
    {
        var lazy = _inflight.GetOrAdd(key, static (_, state) =>
                new Lazy<Task<object?>>(async () => await state.factory(state.ct).ConfigureAwait(false),
                    LazyThreadSafetyMode.ExecutionAndPublication),
            (factory: (Func<CancellationToken, Task<object?>>)(async c => await factory(c).ConfigureAwait(false)), ct));

        try
        {
            var result = await (lazy.Value).ConfigureAwait(false);
            return (T)result!;
        }
        finally
        {
            _inflight.TryRemove(new KeyValuePair<string, Lazy<Task<object?>>>(key, lazy));
        }
    }
}
