using DesignPatterns.Behavioral;
using State.Sample;

Console.WriteLine("=== Manual TransitionTableBuilder ===");

var manualTable = new TransitionTableBuilder<OrderStatus, OrderTrigger>()
    .WithInitial(OrderStatus.Draft)
    .Add(OrderStatus.Draft, OrderTrigger.Submit, OrderStatus.Submitted)
    .Add(OrderStatus.Submitted, OrderTrigger.Pay, OrderStatus.Paid)
    .Build();

RunTransition(manualTable, OrderStatus.Draft, OrderTrigger.Submit);
RunTransition(manualTable, OrderStatus.Submitted, OrderTrigger.Pay);
AssertInvalid(manualTable, OrderStatus.Paid, OrderTrigger.Pay);

Console.WriteLine();
Console.WriteLine("=== Generated [StateMachine] table ===");
Console.WriteLine($"Initial state: {OrderMachine.InitialState}");

RunGenerated(OrderStatus.Draft, OrderTrigger.Submit);
RunGenerated(OrderStatus.Submitted, OrderTrigger.Pay);
AssertInvalidGenerated(OrderStatus.Paid, OrderTrigger.Pay);

static void RunTransition(
    ITransitionTable<OrderStatus, OrderTrigger> table,
    OrderStatus current,
    OrderTrigger trigger)
{
    if (table.TryTransition(current, trigger, out var next))
    {
        Console.WriteLine($"{current} + {trigger} -> {next}");
    }
}

static void AssertInvalid(
    ITransitionTable<OrderStatus, OrderTrigger> table,
    OrderStatus current,
    OrderTrigger trigger)
{
    if (!table.TryTransition(current, trigger, out _))
    {
        Console.WriteLine($"{current} + {trigger} is invalid (expected).");
    }
}

static void RunGenerated(OrderStatus current, OrderTrigger trigger)
{
    if (OrderMachine.TryTransition(current, trigger, out var next))
    {
        Console.WriteLine($"{current} + {trigger} -> {next}");
    }
}

static void AssertInvalidGenerated(OrderStatus current, OrderTrigger trigger)
{
    if (!OrderMachine.TryTransition(current, trigger, out _))
    {
        Console.WriteLine($"{current} + {trigger} is invalid (expected).");
    }
}
