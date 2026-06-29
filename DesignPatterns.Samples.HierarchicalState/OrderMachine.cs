using DesignPatterns.Behavioral;

namespace DesignPatterns.Samples.HierarchicalState;

/// <summary>
/// Order status enum with a natural hierarchy:
///   Active (parent)
///   ├── Submitted (child)
///   └── Paid (child)
///   Draft (root)
///   Cancelled (root)
///
/// Submitted and Paid both inherit the Cancel transition from Active.
/// </summary>
public enum OrderStatus
{
    Draft,
    Active,
    Submitted,
    Paid,
    Cancelled,
}

public enum OrderTrigger
{
    Submit,
    Pay,
    Cancel,
}

/// <summary>
/// Hierarchical state machine: [StateParent] declares parent-child relationships,
/// [StateMachine(Hierarchical = true)] enables compile-time flattening.
///
/// The Cancel transition is declared on Active (the parent). After flattening,
/// both Submitted and Paid inherit it — no need to repeat the edge per child.
/// </summary>
[StateMachine(typeof(OrderStatus), typeof(OrderTrigger), Initial = OrderStatus.Draft, Hierarchical = true)]
[StateParent(OrderStatus.Submitted, OrderStatus.Active)]
[StateParent(OrderStatus.Paid, OrderStatus.Active)]

// Direct edges
[Transition(OrderStatus.Draft, OrderTrigger.Submit, OrderStatus.Submitted,
    OnExit = nameof(OnExitDraft))]
[Transition(OrderStatus.Submitted, OrderTrigger.Pay, OrderStatus.Paid,
    OnExit = nameof(OnExitSubmitted))]

// Parent-level edge: Submitted and Paid both inherit Cancel → Cancelled.
// The exit action chain fires OnExitSubmitted then OnExitActive (RFC §8).
[Transition(OrderStatus.Active, OrderTrigger.Cancel, OrderStatus.Cancelled,
    OnExit = nameof(OnExitActive))]
public static partial class OrderMachine
{
    /// <summary>
    /// Exit action for Draft: logs when leaving the initial state.
    /// </summary>
    public static void OnExitDraft(OrderStatus from, OrderStatus to, OrderTrigger trigger)
        => Console.WriteLine($"  [OnExit] leaving {from} (trigger: {trigger})");

    /// <summary>
    /// Exit action for Submitted: logs when leaving the submitted state.
    /// </summary>
    public static void OnExitSubmitted(OrderStatus from, OrderStatus to, OrderTrigger trigger)
        => Console.WriteLine($"  [OnExit] leaving {from} (trigger: {trigger})");

    /// <summary>
    /// Exit action for Active (parent): logs when leaving the active subtree.
    /// When a child state (Submitted/Paid) transitions via an inherited edge,
    /// the composite delegate fires this after the child's exit action.
    /// </summary>
    public static void OnExitActive(OrderStatus from, OrderStatus to, OrderTrigger trigger)
        => Console.WriteLine($"  [OnExit] leaving Active subtree (was in {from})");
}
