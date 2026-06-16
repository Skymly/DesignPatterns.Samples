using Autofac;
using PluginAssemblies.Sample.Contracts;

namespace PluginAssemblies.Sample.Providers.Alpha;

public sealed class AlphaProviderModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        CardMotionRegistry.RegisterAutofac(builder);
    }
}
