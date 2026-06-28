using Chain.Sample;
using DesignPatterns.Behavioral;

// With DesignPatterns.Extensions.DependencyInjection + targets:
//   var services = new ServiceCollection();
//   RequestContextHandlerPipeline.RegisterDi(services);
//   var pipeline = services.BuildServiceProvider()
//       .GetRequiredService<HandlerPipeline<RequestContext>>();

var pipeline = RequestContextHandlerPipeline.Instance;

Console.WriteLine("=== Authenticated request ===");
var authorized = new RequestContext("/api/orders", isAuthenticated: true);
await pipeline.InvokeAsync(authorized);
Console.WriteLine($"Response: {authorized.Response}");
Console.WriteLine();

Console.WriteLine("=== Unauthenticated request (short-circuit) ===");
var unauthorized = new RequestContext("/api/orders", isAuthenticated: false);
await pipeline.InvokeAsync(unauthorized);
Console.WriteLine($"Response: {unauthorized.Response}");

// Traced invocation: InvokeTracedAsync returns a trace with per-step status.
Console.WriteLine();
Console.WriteLine("=== Traced invocation (InvokeTracedAsync) ===");
var traceContext = new RequestContext("/api/orders", isAuthenticated: true);
var trace = await pipeline.InvokeTracedAsync(traceContext);
foreach (var step in trace.Steps)
{
    Console.WriteLine($"  [{step.Index}] {step.Name} -> {step.Status}");
}
Console.WriteLine($"Response: {traceContext.Response}");

// Exception observability: Failed status + IHandlerExceptionObserver.
Console.WriteLine();
Console.WriteLine("=== Exception observability ===");
var failingPipeline = new HandlerPipelineBuilder<RequestContext>()
    .Use(new ThrowingHandler())
    .Build();

try
{
    var failTrace = await failingPipeline.InvokeTracedAsync(
        new RequestContext("/api/fail", isAuthenticated: true),
        new ConsoleExceptionObserver());
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Re-thrown after trace: {ex.Message}");
}

public sealed class ThrowingHandler : IHandler<RequestContext>
{
    public ValueTask InvokeAsync(
        RequestContext context,
        HandlerDelegate<RequestContext> next,
        CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Simulated handler failure");
}

public sealed class ConsoleExceptionObserver : IHandlerExceptionObserver<RequestContext>
{
    public void OnHandlerException(RequestContext context, int handlerIndex, string handlerName, Exception exception)
        => Console.WriteLine($"  [Observer] Handler {handlerName} (index {handlerIndex}) failed: {exception.Message}");
}
