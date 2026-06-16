namespace PluginAssemblies.Sample.Scenarios.DuplicateKey;

/// <summary>
/// Illustrates DP033: the same strategy key for one contract in multiple referenced provider assemblies.
/// See sibling DesignPatterns repo <c>CrossAssemblyRegistryKeyAnalyzerTests</c>; CI runs that test for S4.
/// </summary>
public static class DuplicateKeyUsage
{
    public static void HostReferencesConflictingProviders()
    {
    }
}
