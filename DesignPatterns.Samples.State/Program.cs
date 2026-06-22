using DesignPatterns.Behavioral;
using DesignPatterns.Extensions.DependencyInjection;
using DesignPatterns.Samples.State;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("=== Manual TransitionTableBuilder (with guard) ===");

var manualTable = new TransitionTableBuilder<OrderStatus, OrderTrigger>()
    .WithInitial(OrderStatus.Draft)
    .Add(OrderStatus.Draft, OrderTrigger.Submit, OrderStatus.Submitted)
    .Add(OrderStatus.Draft, OrderTrigger.Cancel, OrderStatus.Cancelled,
         guard: (state, trigger) => true) // guard always passes in sample
    .Add(OrderStatus.Submitted, OrderTrigger.Pay, OrderStatus.Paid)
    .Add(OrderStatus.Submitted, OrderTrigger.Cancel, OrderStatus.Cancelled,
         guard: (state, trigger) => true)
    .Build();

RunTransition(manualTable, OrderStatus.Draft, OrderTrigger.Submit);
RunTransition(manualTable, OrderStatus.Submitted, OrderTrigger.Pay);
RunTransition(manualTable, OrderStatus.Draft, OrderTrigger.Cancel);
AssertInvalid(manualTable, OrderStatus.Paid, OrderTrigger.Pay);

Console.WriteLine();
Console.WriteLine("=== Generated [StateMachine] table (with guard) ===");
Console.WriteLine($"Initial state: {OrderMachine.InitialState}");

RunGenerated(OrderStatus.Draft, OrderTrigger.Submit);
RunGenerated(OrderStatus.Submitted, OrderTrigger.Pay);
RunGenerated(OrderStatus.Draft, OrderTrigger.Cancel);
RunGenerated(OrderStatus.Submitted, OrderTrigger.Cancel);
AssertInvalidGenerated(OrderStatus.Paid, OrderTrigger.Pay);

Console.WriteLine();
Console.WriteLine("=== DI integration (RegisterDi + AddTransitionTable) ===");

// Generated RegisterDi: registers ITransitionTable<OrderStatus, OrderTrigger> as singleton
var services = new ServiceCollection();
OrderStatusTransitionTable.RegisterDi(services);

// Also demonstrate manual AddTransitionTable extension (TryAdd idempotent — won't overwrite)
services.AddTransitionTable(OrderStatusTransitionTable.Instance);

var provider = services.BuildServiceProvider();
var diTable = provider.GetRequiredService<ITransitionTable<OrderStatus, OrderTrigger>>();

Console.WriteLine($"Resolved from DI: {diTable.GetType().Name}");
Console.WriteLine($"DI table initial state: {diTable.InitialState}");

RunTransition(diTable, OrderStatus.Draft, OrderTrigger.Submit);
RunTransition(diTable, OrderStatus.Submitted, OrderTrigger.Pay);
AssertInvalid(diTable, OrderStatus.Cancelled, OrderTrigger.Submit);

Console.WriteLine();
Console.WriteLine("=== GetAllowedTriggers ===");

var allowed = diTable.GetAllowedTriggers(OrderStatus.Draft);
Console.WriteLine($"Allowed triggers from Draft: {string.Join(", ", allowed)}");

var submittedAllowed = diTable.GetAllowedTriggers(OrderStatus.Submitted);
Console.WriteLine($"Allowed triggers from Submitted: {string.Join(", ", submittedAllowed)}");

var paidAllowed = diTable.GetAllowedTriggers(OrderStatus.Paid);
Console.WriteLine($"Allowed triggers from Paid (terminal): {(paidAllowed.Count == 0 ? "(none — terminal state)" : string.Join(", ", paidAllowed))}");

static void RunTransition(
    ITransitionTable<OrderStatus, OrderTrigger> table,
    OrderStatus current,
    OrderTrigger trigger)
{
    if (table.TryTransition(current, trigger, out var next))
    {
        Console.WriteLine($"  {current} + {trigger} -> {next}");
    }
}

static void AssertInvalid(
    ITransitionTable<OrderStatus, OrderTrigger> table,
    OrderStatus current,
    OrderTrigger trigger)
{
    if (!table.TryTransition(current, trigger, out _))
    {
        Console.WriteLine($"  {current} + {trigger} is invalid (expected).");
    }
}

static void RunGenerated(OrderStatus current, OrderTrigger trigger)
{
    if (OrderMachine.TryTransition(current, trigger, out var next))
    {
        Console.WriteLine($"  {current} + {trigger} -> {next}");
    }
}

static void AssertInvalidGenerated(OrderStatus current, OrderTrigger trigger)
{
    if (!OrderMachine.TryTransition(current, trigger, out _))
    {
        Console.WriteLine($"  {current} + {trigger} is invalid (expected).");
    }
}
