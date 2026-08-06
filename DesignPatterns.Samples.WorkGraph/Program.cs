using DesignPatterns.Behavioral;
using DesignPatterns.Samples.WorkGraph;

Console.WriteLine("=== Request-prep Work Graph shape ===");
Console.WriteLine("  Auth ∥ LoadConfig → BuildPrincipal → Authorize");
Console.WriteLine($"  Keys: {RequestPrepWorkStepKeys.Auth}, {RequestPrepWorkStepKeys.LoadConfig}, {RequestPrepWorkStepKeys.BuildPrincipal}, {RequestPrepWorkStepKeys.Authorize}");

Console.WriteLine();
Console.WriteLine("=== Manual WorkGraphBuilder ===");

var manual = new WorkGraphBuilder<PrepContext>()
    .Add(RequestPrepWorkStepKeys.Auth, new AuthStep())
    .Add(RequestPrepWorkStepKeys.LoadConfig, new LoadConfigStep())
    .Add(
        RequestPrepWorkStepKeys.BuildPrincipal,
        new BuildPrincipalStep(),
        RequestPrepWorkStepKeys.Auth,
        RequestPrepWorkStepKeys.LoadConfig)
    .Add(
        RequestPrepWorkStepKeys.Authorize,
        new AuthorizeStep(),
        RequestPrepWorkStepKeys.BuildPrincipal)
    .Build();

var manualContext = new PrepContext();
await manual.RunAsync(manualContext);
AssertRequestPrepSucceeded(manualContext, "manual");

Console.WriteLine();
Console.WriteLine("=== Generated RequestPrepWorkGraph.Create(resolver) ===");

var generated = RequestPrepWorkGraph.Create(id => id switch
{
    RequestPrepWorkStepKeys.Auth => new AuthStep(),
    RequestPrepWorkStepKeys.LoadConfig => new LoadConfigStep(),
    RequestPrepWorkStepKeys.BuildPrincipal => new BuildPrincipalStep(),
    RequestPrepWorkStepKeys.Authorize => new AuthorizeStep(),
    _ => throw new ArgumentOutOfRangeException(nameof(id), id, "Unknown work step id."),
});

var generatedContext = new PrepContext();
await generated.RunAsync(generatedContext);
AssertRequestPrepSucceeded(generatedContext, "generated");

Console.WriteLine();
Console.WriteLine("=== Empty graph rejected at Build ===");

try
{
    _ = new WorkGraphBuilder<PrepContext>().Build();
    throw new InvalidOperationException("Empty Build() should have thrown.");
}
catch (InvalidWorkGraphException ex)
{
    Console.WriteLine($"  Expected: {ex.Message}");
}

static void AssertRequestPrepSucceeded(PrepContext context, string path)
{
    if (context.Token is null || context.ConfigRole is null || context.Principal is null || !context.Authorized)
    {
        throw new InvalidOperationException($"Request-prep ({path}) did not fully populate PrepContext.");
    }

    var log = string.Join('\n', context.SnapshotLog());
    if (!log.Contains("Auth start", StringComparison.Ordinal)
        || !log.Contains("LoadConfig start", StringComparison.Ordinal)
        || !log.Contains("BuildPrincipal start", StringComparison.Ordinal)
        || !log.Contains("Authorize start", StringComparison.Ordinal))
    {
        throw new InvalidOperationException($"Request-prep ({path}) log missing expected step markers.");
    }

    var buildIndex = log.IndexOf("BuildPrincipal start", StringComparison.Ordinal);
    var authDone = log.IndexOf("Auth done", StringComparison.Ordinal);
    var configDone = log.IndexOf("LoadConfig done", StringComparison.Ordinal);
    if (authDone < 0 || configDone < 0 || buildIndex < 0 || authDone > buildIndex || configDone > buildIndex)
    {
        throw new InvalidOperationException(
            $"Request-prep ({path}) wave order violated: Auth/LoadConfig must finish before BuildPrincipal.");
    }

    Console.WriteLine($"  Context ({path}): principal={context.Principal}, authorized={context.Authorized}");
}
