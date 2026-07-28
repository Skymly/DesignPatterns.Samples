using DesignPatterns.Behavioral;

namespace DesignPatterns.Samples.CommandRouter;

public sealed class PingCommand : ICommand;

public sealed class GetTotalCommand : ICommand<decimal>
{
    public GetTotalCommand(decimal unitPrice, int quantity)
    {
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public decimal UnitPrice { get; }

    public int Quantity { get; }
}
