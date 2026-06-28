using DesignPatterns.Behavioral;

namespace DesignPatterns.Samples.State;

public enum OrderStatus
{
    Draft,
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

[StateMachine(typeof(OrderStatus), typeof(OrderTrigger), Initial = OrderStatus.Draft)]
[Transition(OrderStatus.Draft, OrderTrigger.Submit, OrderStatus.Submitted,
    Guard = nameof(AlwaysTrue), OnEnter = nameof(OnEnterSubmitted), OnExit = nameof(OnExitDraft))]
[Transition(OrderStatus.Draft, OrderTrigger.Cancel, OrderStatus.Cancelled, Guard = nameof(CanCancelDraft))]
[Transition(OrderStatus.Submitted, OrderTrigger.Pay, OrderStatus.Paid,
    Guard = nameof(AlwaysTrue), OnEnter = nameof(OnEnterPaid), OnExit = nameof(OnExitSubmitted))]
[Transition(OrderStatus.Submitted, OrderTrigger.Cancel, OrderStatus.Cancelled, Guard = nameof(CanCancelSubmitted))]
public static partial class OrderMachine
{
    /// <summary>
    /// Guard: draft orders can only be cancelled when no items have been submitted yet.
    /// Demonstrates a guard that always passes in the sample (no domain state).
    /// </summary>
    public static bool CanCancelDraft(OrderStatus state, OrderTrigger trigger) => true;

    /// <summary>
    /// Guard: submitted orders can only be cancelled before payment is processed.
    /// Demonstrates a guard that always passes in the sample (no domain state).
    /// </summary>
    public static bool CanCancelSubmitted(OrderStatus state, OrderTrigger trigger) => true;

    /// <summary>
    /// Always-true guard used when a transition needs a guard placeholder to satisfy
    /// the generator's positional parameter requirement when actions are present.
    /// </summary>
    public static bool AlwaysTrue(OrderStatus state, OrderTrigger trigger) => true;

    /// <summary>
    /// Entry action for Submitted state: logs the transition.
    /// Demonstrates entry/exit actions (DP037-DP039).
    /// </summary>
    public static void OnEnterSubmitted(OrderStatus from, OrderStatus to, OrderTrigger trigger)
        => Console.WriteLine($"  [OnEnter] {from} -> {to} (trigger: {trigger})");

    /// <summary>
    /// Exit action for Draft state: logs the transition.
    /// </summary>
    public static void OnExitDraft(OrderStatus from, OrderStatus to, OrderTrigger trigger)
        => Console.WriteLine($"  [OnExit] leaving {from} (trigger: {trigger})");

    /// <summary>
    /// Entry action for Paid state: logs the transition.
    /// </summary>
    public static void OnEnterPaid(OrderStatus from, OrderStatus to, OrderTrigger trigger)
        => Console.WriteLine($"  [OnEnter] {from} -> {to} (trigger: {trigger})");

    /// <summary>
    /// Exit action for Submitted state: logs the transition.
    /// </summary>
    public static void OnExitSubmitted(OrderStatus from, OrderStatus to, OrderTrigger trigger)
        => Console.WriteLine($"  [OnExit] leaving {from} (trigger: {trigger})");
}
