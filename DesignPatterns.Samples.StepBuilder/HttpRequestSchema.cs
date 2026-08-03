using DesignPatterns.Creational;

namespace DesignPatterns.Samples.StepBuilder;

/// <summary>
/// Schema holder for the generated <c>HttpRequestSchemaBuilder</c> (Step Builder domain).
/// Required steps gate <c>Build()</c> at compile time; optional/mutex constraints are runtime diagnostics.
/// </summary>
[GenerateBuilder]
public static class HttpRequestSchema
{
    [BuilderStep]
    public static void WithUrl(string url)
    {
    }

    [BuilderStep]
    public static void WithMethod(string method)
    {
    }

    [BuilderStep(Required = false)]
    public static void WithHeader(string header)
    {
    }

    [BuilderStep(Required = false)]
    public static void WithBody(string body)
    {
    }

    // MutexGroup = "Auth": at most one of these may be applied (InvalidOperationException on conflict).
    [BuilderStep(Required = false, MutexGroup = "Auth")]
    public static void WithBearerToken(string token)
    {
    }

    [BuilderStep(Required = false, MutexGroup = "Auth")]
    public static void WithBasicAuth(string credentials)
    {
    }

    [BuilderAssemble]
    public static HttpRequest Assemble(
        string url,
        string method,
        string? header,
        string? body,
        string? bearerToken,
        string? basicAuth) =>
        new(url, method, header, body, bearerToken, basicAuth);
}
