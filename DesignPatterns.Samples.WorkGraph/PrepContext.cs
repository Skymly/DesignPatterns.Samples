namespace DesignPatterns.Samples.WorkGraph;

/// <summary>
/// Shared context mutated by request-prep work steps. Edges carry readiness only;
/// outputs land here (no typed payload channels).
/// </summary>
public sealed class PrepContext
{
    private readonly object _gate = new();
    private readonly List<string> _log = [];

    public string? Token { get; set; }

    public string? ConfigRole { get; set; }

    public string? Principal { get; set; }

    public bool Authorized { get; set; }

    public void Log(string message)
    {
        var line = $"{DateTimeOffset.UtcNow:HH:mm:ss.fff} {message}";
        lock (_gate)
        {
            _log.Add(line);
        }

        Console.WriteLine($"  {line}");
    }

    public IReadOnlyList<string> SnapshotLog()
    {
        lock (_gate)
        {
            return _log.ToArray();
        }
    }
}
