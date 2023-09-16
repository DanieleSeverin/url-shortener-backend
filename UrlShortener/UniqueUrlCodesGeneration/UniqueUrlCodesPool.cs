using System.Collections.Concurrent;
using System.Reflection.Emit;

namespace UrlShortener.UniqueUrlCodesGeneration;

public sealed class UniqueUrlCodesPool
{
    private ConcurrentQueue<string> _codes = new ConcurrentQueue<string>();
    private ConcurrentDictionary<string, byte> _existingCodes = new ConcurrentDictionary<string, byte>();  // byte used as a dummy value to minimize memory usage

    public string? Dequeue()
    {
        if (_codes.TryDequeue(out string? code))
        {
            _existingCodes.TryRemove(code, out _);
            return code;
        }

        return null;
    }

    public void Enqueue(string code)
    {
        if (_existingCodes.TryAdd(code, 0))
        {
            _codes.Enqueue(code);
        }
    }

    public int Count => _codes.Count;
}
