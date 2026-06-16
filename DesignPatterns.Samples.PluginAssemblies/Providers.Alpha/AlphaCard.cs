using DesignPatterns.Behavioral;
using PluginAssemblies.Sample.Contracts;

namespace PluginAssemblies.Sample.Providers.Alpha;

[RegisterStrategy<ICardMotion>("alpha")]
public sealed class AlphaCard : ICardMotion
{
    public string ProviderName => "alpha";
}
