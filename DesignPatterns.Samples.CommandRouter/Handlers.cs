using DesignPatterns.Behavioral;

namespace DesignPatterns.Samples.CommandRouter;

[RegisterCommandHandler<PingCommand>]
public sealed class PingCommandHandler : ICommandHandler<PingCommand>
{
    public ValueTask HandleAsync(PingCommand command, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("  [Ping] handled");
        return default;
    }
}

[RegisterCommandHandler<GetTotalCommand>]
public sealed class GetTotalCommandHandler : ICommandHandler<GetTotalCommand, decimal>
{
    public ValueTask<decimal> HandleAsync(GetTotalCommand command, CancellationToken cancellationToken = default)
    {
        var total = command.UnitPrice * command.Quantity;
        Console.WriteLine($"  [GetTotal] {command.UnitPrice:C} x {command.Quantity} = {total:C}");
        return new ValueTask<decimal>(total);
    }
}
