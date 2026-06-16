using Autofac;
using PluginAssemblies.Sample.Contracts;

namespace PluginAssemblies.Sample.Providers.Beta;

public sealed class BetaProviderModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        CardMotionRegistry.RegisterAutofac(builder);
    }
}
