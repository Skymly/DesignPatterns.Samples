using DesignPatterns.Behavioral;

namespace Strategy.Sample;

[RegisterStrategy<IPaymentStrategy>("alipay", Guard = nameof(IsEnabled))]
public sealed class AlipayPayment : IPaymentStrategy
{
    public string Pay(decimal amount) => $"Alipay: {amount:C}";

    /// <summary>
    /// Guard: Alipay is enabled only for amounts up to 5000.
    /// Demonstrates a compile-time guard predicate (DP047-DP049).
    /// </summary>
    public static bool IsEnabled(string key) => true;
}
