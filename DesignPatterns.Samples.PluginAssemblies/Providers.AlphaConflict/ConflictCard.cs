using DesignPatterns.Behavioral;
using PluginAssemblies.Sample.Contracts;

namespace PluginAssemblies.Sample.Providers.AlphaConflict;

[RegisterStrategy<ICardMotion>("alpha")]
public sealed class ConflictCard : ICardMotion
{
    public string ProviderName => "alpha-conflict";
}
