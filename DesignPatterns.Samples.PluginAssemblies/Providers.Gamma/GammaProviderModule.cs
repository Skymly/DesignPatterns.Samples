using Autofac;
using PluginAssemblies.Sample.Contracts;

namespace PluginAssemblies.Sample.Providers.Gamma;

public sealed class GammaProviderModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        FCControlRegistry.RegisterAutofac(builder);
        FCErrorRegistry.RegisterAutofac(builder);
    }
}
