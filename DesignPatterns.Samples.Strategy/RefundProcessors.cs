using DesignPatterns.Behavioral;

namespace Strategy.Sample;

public interface IRefundProcessor : IAsyncStrategy<decimal, string>
{
}

[RegisterStrategy<IRefundProcessor>("standard")]
public sealed class StandardRefundProcessor : IRefundProcessor
{
    public ValueTask<string> ExecuteAsync(decimal amount, CancellationToken cancellationToken = default) =>
        new ValueTask<string>($"Standard refund: {amount:C}");
}

[RegisterStrategy<IRefundProcessor>("express")]
public sealed class ExpressRefundProcessor : IRefundProcessor
{
    public ValueTask<string> ExecuteAsync(decimal amount, CancellationToken cancellationToken = default) =>
        new ValueTask<string>($"Express refund: {amount:C}");
}
