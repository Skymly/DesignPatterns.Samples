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
[Transition(OrderStatus.Draft, OrderTrigger.Submit, OrderStatus.Submitted)]
[Transition(OrderStatus.Draft, OrderTrigger.Cancel, OrderStatus.Cancelled, Guard = nameof(CanCancelDraft))]
[Transition(OrderStatus.Submitted, OrderTrigger.Pay, OrderStatus.Paid)]
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
}
