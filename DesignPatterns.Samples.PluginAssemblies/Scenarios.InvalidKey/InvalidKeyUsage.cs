using PluginAssemblies.Sample.Contracts;

namespace PluginAssemblies.Sample.Scenarios.InvalidKey;

/// <summary>
/// Illustrates DP025: unknown literal registry keys are flagged at compile time (IDE / NuGet analyzer).
/// See sibling DesignPatterns repo <c>UnknownRegistryKeyAnalyzerTests</c>; CI runs that test for S3.
/// </summary>
public static class InvalidKeyUsage
{
    public static void UseUnknownLiteralKey()
    {
        _ = CardMotionRegistry.Instance.Get("not-a-registered-key");
    }
}
