using DesignPatterns.Behavioral;
using PluginAssemblies.Sample.Contracts;

namespace PluginAssemblies.Sample.Providers.Gamma;

[RegisterStrategy<IFCControl>("gamma")]
public sealed class GammaFc : IFCControl
{
    public string ProviderName => "gamma";
}
