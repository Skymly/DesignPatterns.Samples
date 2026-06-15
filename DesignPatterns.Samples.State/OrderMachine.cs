using DesignPatterns.Behavioral;

namespace State.Sample;

public enum OrderStatus
{
    Draft,
    Submitted,
    Paid,
}

public enum OrderTrigger
{
    Submit,
    Pay,
}

[StateMachine(typeof(OrderStatus), typeof(OrderTrigger), Initial = OrderStatus.Draft)]
[Transition(OrderStatus.Draft, OrderTrigger.Submit, OrderStatus.Submitted)]
[Transition(OrderStatus.Submitted, OrderTrigger.Pay, OrderStatus.Paid)]
public static partial class OrderMachine;
