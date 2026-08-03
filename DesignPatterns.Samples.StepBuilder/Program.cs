using DesignPatterns.Samples.StepBuilder;

Console.WriteLine("=== Step Builder: happy path (required + optional + one auth) ===");

// HttpRequestSchemaBuilder is generated from [GenerateBuilder] on HttpRequestSchema.
// Build() exists only when every required step (Url, Method) has been applied — omitting either
// makes Build() uncallable (no matching extension). Optional Header/Body/auth may be skipped;
// unset optionals arrive as null in Assemble.
var request = HttpRequestSchemaBuilder.Create()
    .WithUrl("https://api.example.com/orders")
    .WithMethod("POST")
    .WithHeader("Content-Type: application/json")
    .WithBody("""{"sku":"widget","qty":2}""")
    .WithBearerToken("tok_sample")
    .Build();

Console.WriteLine($"  {request.Method} {request.Url}");
Console.WriteLine($"  Header: {request.Header}");
Console.WriteLine($"  Body:   {request.Body}");
Console.WriteLine($"  Auth:   Bearer {request.BearerToken}");

Console.WriteLine();
Console.WriteLine("=== Required-step completeness (compile-time) ===");
Console.WriteLine("  // Does not compile — Build() is not available until Url and Method are set:");
Console.WriteLine("  // HttpRequestSchemaBuilder.Create().WithUrl(\"…\").Build();");

Console.WriteLine();
Console.WriteLine("=== Mutex Auth group (BearerToken vs BasicAuth) ===");

try
{
    // Same MutexGroup on both steps: applying the second throws at runtime.
    _ = HttpRequestSchemaBuilder.Create()
        .WithUrl("https://api.example.com/profile")
        .WithMethod("GET")
        .WithBearerToken("tok_a")
        .WithBasicAuth("user:pass")
        .Build();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"  Expected: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("=== Minimal request (required only) ===");

var minimal = HttpRequestSchemaBuilder.Create()
    .WithMethod("GET")
    .WithUrl("https://api.example.com/health")
    .Build();

Console.WriteLine($"  {minimal.Method} {minimal.Url} (header={minimal.Header is null}, body={minimal.Body is null}, auth unset)");
