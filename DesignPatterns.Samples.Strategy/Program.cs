using DesignPatterns.Behavioral;
using Strategy.Sample;

// Without DI package: use generated static Instance (eager new()).
// With DesignPatterns.Extensions.DependencyInjection + targets:
//   var services = new ServiceCollection();
//   PaymentStrategyRegistry.RegisterDi(services);
//   var registry = services.BuildServiceProvider()
//       .GetRequiredService<IStrategyRegistry<string, IPaymentStrategy>>();

var registry = PaymentStrategyRegistry.Instance;

var alipay = registry.Get(PaymentStrategyKeys.Alipay);
var wechat = registry.Get(PaymentStrategyKeys.Wechat);

Console.WriteLine(alipay.Pay(100m));
Console.WriteLine(wechat.Pay(200m));

if (!registry.TryGet("unknown", out _))
{
    Console.WriteLine("Unknown key not found (expected).");
}

// Guard predicate: TryGetWithGuard evaluates the guard before returning the strategy.
Console.WriteLine();
Console.WriteLine("=== Guard predicate (TryGetWithGuard) ===");
if (registry.TryGetWithGuard(PaymentStrategyKeys.Alipay, out var guardedAlipay))
{
    Console.WriteLine(guardedAlipay!.Pay(150m));
}
else
{
    Console.WriteLine("Alipay guard rejected (unexpected in sample).");
}

// Execution tracing: ExecuteTracedAsync returns a trace with status, output, and timing.
Console.WriteLine();
Console.WriteLine("=== Execution tracing (ExecuteTracedAsync) ===");
var refundRegistry = RefundProcessorRegistry.Instance;

var standardTrace = await refundRegistry.ExecuteTracedAsync<IRefundProcessor, string, decimal>(
    RefundProcessorKeys.Standard,
    50m);
Console.WriteLine($"Standard refund: {standardTrace.Output}, status={standardTrace.Status}, elapsed={standardTrace.ElapsedMilliseconds}ms");

var expressTrace = await refundRegistry.ExecuteTracedAsync<IRefundProcessor, string, decimal>(
    RefundProcessorKeys.Express,
    75m);
Console.WriteLine($"Express refund: {expressTrace.Output}, status={expressTrace.Status}, elapsed={expressTrace.ElapsedMilliseconds}ms");

// KeyNotFound trace: requesting a non-existent key produces a KeyNotFound status.
var missingTrace = await refundRegistry.ExecuteTracedAsync<IRefundProcessor, string, decimal>(
    "nonexistent",
    10m);
Console.WriteLine($"Missing key: status={missingTrace.Status} (expected KeyNotFound)");

// Non-traced execution (original path, still available).
var standardRefund = await refundRegistry.ExecuteAsync<IRefundProcessor, string, decimal>(
    RefundProcessorKeys.Standard,
    50m);
var expressRefund = await refundRegistry.ExecuteAsync<IRefundProcessor, string, decimal>(
    RefundProcessorKeys.Express,
    75m);

Console.WriteLine(standardRefund);
Console.WriteLine(expressRefund);
