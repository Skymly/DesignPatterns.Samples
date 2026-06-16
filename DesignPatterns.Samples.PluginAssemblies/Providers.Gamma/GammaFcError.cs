using DesignPatterns.Behavioral;
using PluginAssemblies.Sample.Contracts;

namespace PluginAssemblies.Sample.Providers.Gamma;

[RegisterStrategy<IFCError>("gamma")]
public sealed class GammaFcError : IFCError
{
    public string ProviderName => "gamma";
}
