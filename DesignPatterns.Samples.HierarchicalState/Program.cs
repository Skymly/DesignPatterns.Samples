using DesignPatterns.Behavioral;
using DesignPatterns.Extensions.DependencyInjection;
using DesignPatterns.Samples.HierarchicalState;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("=== Hierarchical State Machine ===");
Console.WriteLine();
Console.WriteLine("Hierarchy:");
Console.WriteLine("  Active (parent)");
Console.WriteLine("  ├── Submitted (child)");
Console.WriteLine("  └── Paid (child)");
Console.WriteLine("  Draft (root)");
Console.WriteLine("  Cancelled (root)");
Console.WriteLine();
Console.WriteLine("Submitted and Paid inherit Cancel → Cancelled from Active.");
Console.WriteLine();

// --- Flattened transitions ---
Console.WriteLine("--- Flattened transitions (compile-time) ---");

var table = OrderStatusTransitionTable.Instance;

// Direct edges
RunTransition(table, OrderStatus.Draft, OrderTrigger.Submit);       // Draft → Submitted
RunTransition(table, OrderStatus.Submitted, OrderTrigger.Pay);      // Submitted → Paid

// Inherited edges (flattened from Active)
RunTransition(table, OrderStatus.Submitted, OrderTrigger.Cancel);   // Submitted → Cancelled (inherited)
RunTransition(table, OrderStatus.Paid, OrderTrigger.Cancel);        // Paid → Cancelled (inherited)

Console.WriteLine();

// --- IStateHierarchy queries ---
Console.WriteLine("--- IStateHierarchy queries ---");

if (table is IStateHierarchy<OrderStatus> hierarchy)
{
    Console.WriteLine($"GetParent(Submitted) = {hierarchy.GetParent(OrderStatus.Submitted)}");
    Console.WriteLine($"GetParent(Paid) = {hierarchy.GetParent(OrderStatus.Paid)}");
    Console.WriteLine($"GetParent(Draft) = {hierarchy.GetParent(OrderStatus.Draft) ?? (object)"(root)"}");
    Console.WriteLine($"IsInState(Submitted, Active) = {hierarchy.IsInState(OrderStatus.Submitted, OrderStatus.Active)}");
    Console.WriteLine($"IsInState(Paid, Active) = {hierarchy.IsInState(OrderStatus.Paid, OrderStatus.Active)}");
    Console.WriteLine($"IsInState(Cancelled, Active) = {hierarchy.IsInState(OrderStatus.Cancelled, OrderStatus.Active)}");
    Console.WriteLine($"IsInState(Active, Active) = {hierarchy.IsInState(OrderStatus.Active, OrderStatus.Active)}");
    Console.WriteLine($"GetAncestors(Submitted) = [{string.Join(", ", hierarchy.GetAncestors(OrderStatus.Submitted))}]");
}

Console.WriteLine();

// --- GetAllowedTriggers (includes inherited) ---
Console.WriteLine("--- GetAllowedTriggers (includes inherited) ---");

var submittedTriggers = table.GetAllowedTriggers(OrderStatus.Submitted);
Console.WriteLine($"Allowed from Submitted: {string.Join(", ", submittedTriggers)}");
// Expected: Pay (direct), Cancel (inherited from Active)

var paidTriggers = table.GetAllowedTriggers(OrderStatus.Paid);
Console.WriteLine($"Allowed from Paid: {string.Join(", ", paidTriggers)}");
// Expected: Cancel (inherited from Active)

Console.WriteLine();

// --- Entry/exit action chains (TryTransitionAsync) ---
Console.WriteLine("--- Entry/exit action chains (TryTransitionAsync) ---");
Console.WriteLine("Scenario: Submitted → Cancelled (inherited edge, composite exit chain)");
Console.WriteLine("Expected: OnExitSubmitted fires, then OnExitActive fires (composite delegate)");
Console.WriteLine();

var actionMachine = new StateMachine<OrderStatus, OrderTrigger>(table);

// Move to Submitted first
Console.WriteLine("Step 1: Draft → Submitted");
await actionMachine.TryTransitionAsync(OrderTrigger.Submit, CancellationToken.None);
Console.WriteLine($"  Current state: {actionMachine.CurrentState}");
Console.WriteLine();

// Now fire Cancel — this triggers the composite exit chain
Console.WriteLine("Step 2: Submitted → Cancelled (inherited from Active)");
Console.WriteLine("  The composite delegate CompositeExit_Submitted_Cancel fires:");
await actionMachine.TryTransitionAsync(OrderTrigger.Cancel, CancellationToken.None);
Console.WriteLine($"  Current state: {actionMachine.CurrentState}");

Console.WriteLine();

// --- DI integration ---
Console.WriteLine("--- DI integration (RegisterDi + AddStateHierarchy) ---");

var services = new ServiceCollection();
OrderStatusTransitionTable.RegisterDi(services);
services.AddStateMachine<OrderStatus, OrderTrigger>();

var provider = services.BuildServiceProvider();
var diTable = provider.GetRequiredService<ITransitionTable<OrderStatus, OrderTrigger>>();
var diHierarchy = provider.GetRequiredService<IStateHierarchy<OrderStatus>>();
var diMachine = provider.GetRequiredService<IStateMachine<OrderStatus, OrderTrigger>>();

Console.WriteLine($"Resolved ITransitionTable from DI: {diTable.GetType().Name}");
Console.WriteLine($"Resolved IStateHierarchy from DI: {diHierarchy.GetType().Name}");
Console.WriteLine($"Resolved IStateMachine from DI: {diMachine.GetType().Name}");
Console.WriteLine($"DI hierarchy: IsInState(Submitted, Active) = {diHierarchy.IsInState(OrderStatus.Submitted, OrderStatus.Active)}");
Console.WriteLine($"DI machine initial state: {diMachine.CurrentState}");

Console.WriteLine();

static void RunTransition(
    ITransitionTable<OrderStatus, OrderTrigger> table,
    OrderStatus current,
    OrderTrigger trigger)
{
    if (table.TryTransition(current, trigger, out var next))
    {
        var isInherited = current is OrderStatus.Submitted or OrderStatus.Paid && trigger is OrderTrigger.Cancel;
        var tag = isInherited ? " (inherited)" : "";
        Console.WriteLine($"  {current} + {trigger} -> {next}{tag}");
    }
    else
    {
        Console.WriteLine($"  {current} + {trigger} is invalid.");
    }
}
