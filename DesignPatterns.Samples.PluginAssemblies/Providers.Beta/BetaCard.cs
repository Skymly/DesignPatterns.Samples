using DesignPatterns.Behavioral;
using PluginAssemblies.Sample.Contracts;

namespace PluginAssemblies.Sample.Providers.Beta;

[RegisterStrategy<ICardMotion>("beta")]
public sealed class BetaCard : ICardMotion
{
    public string ProviderName => "beta";
}
