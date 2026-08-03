namespace DesignPatterns.Samples.StepBuilder;

/// <summary>
/// Product DTO assembled by <see cref="HttpRequestSchema"/>. Free of builder metadata.
/// </summary>
public sealed record HttpRequest(
    string Url,
    string Method,
    string? Header,
    string? Body,
    string? BearerToken,
    string? BasicAuth);
