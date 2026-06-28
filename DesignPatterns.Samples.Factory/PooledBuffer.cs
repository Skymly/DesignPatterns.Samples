using DesignPatterns.Creational;

namespace Factory.Sample;

/// <summary>
/// A simple resettable product used to demonstrate pooled factory registry.
/// Implements IResettable so the pool calls Reset() before reuse.
/// </summary>
public sealed class PooledBuffer : IProduct, IResettable
{
    private byte[] _data = new byte[1024];

    public string Name => $"Buffer#{GetHashCode()}";

    public Span<byte> Data => _data.AsSpan();

    public void Reset()
    {
        _data.AsSpan().Clear();
    }
}
