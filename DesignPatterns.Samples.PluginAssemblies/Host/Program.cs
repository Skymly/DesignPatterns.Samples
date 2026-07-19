using System.Configuration;
using Autofac;
using DesignPatterns.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using PluginAssemblies.Sample.Contracts;
using PluginAssemblies.Sample.Providers.Alpha;
using PluginAssemblies.Sample.Providers.Gamma;

var scenario = args.FirstOrDefault() ?? "s1";
var cardConfigurationKey = scenario.Equals("s2", StringComparison.OrdinalIgnoreCase) ? "CardMissing" : "Card";

IConfiguration configuration = new AppSettingsConfiguration();

var builder = new ContainerBuilder();
builder.RegisterModule<AlphaProviderModule>();
builder.RegisterModule<GammaProviderModule>();

using var container = builder.Build();

var cardRegistry = CardMotionRegistry.Create(container);
var fcRegistry = FCControlRegistry.Create(container);
var fcErrorRegistry = FCErrorRegistry.Create(container);

var cardKeys = string.Join(", ", cardRegistry.Keys.OrderBy(key => key, StringComparer.Ordinal));
Console.WriteLine($"CardMotion keys (Alpha assembly): {cardKeys}");

if (scenario.Equals("s1", StringComparison.OrdinalIgnoreCase))
{
    if (!cardRegistry.Keys.Contains(CardMotionKeys.Alpha))
    {
        Console.Error.WriteLine("Expected 'alpha' in CardMotionRegistry.Keys.");
        return 1;
    }

    if (cardRegistry.Keys.Contains("beta"))
    {
        Console.Error.WriteLine("Beta key must not appear when Providers.Beta is not referenced.");
        return 1;
    }
}

if (!RegistryConfiguration.TryResolveConfigured(
        cardRegistry,
        configuration,
        cardConfigurationKey,
        out var card,
        defaultKey: CardMotionKeys.Alpha))
{
    var configuredValue = configuration[cardConfigurationKey];
    var strategyKey = string.IsNullOrWhiteSpace(configuredValue) ? CardMotionKeys.Alpha : configuredValue;
    Console.Error.WriteLine(
        $"Card provider '{strategyKey}' is not registered. Available keys: {cardKeys}. " +
        "Reference the matching provider assembly (e.g. Providers.Beta for 'beta').");
    return 1;
}

if (!RegistryConfiguration.TryResolveConfigured(
        fcRegistry,
        configuration,
        "FC",
        out var fc,
        defaultKey: FCControlKeys.Gamma)
    || !RegistryConfiguration.TryResolveConfigured(
        fcErrorRegistry,
        configuration,
        "FC",
        out var fcError,
        defaultKey: FCControlKeys.Gamma))
{
    var fcKey = configuration["FC"] ?? FCControlKeys.Gamma;
    Console.Error.WriteLine($"FC provider '{fcKey}' is not registered for both IFCControl and IFCError.");
    return 1;
}

Console.WriteLine($"Card={card!.ProviderName}, FC={fc!.ProviderName}, FCError={fcError!.ProviderName}");
return 0;

file sealed class AppSettingsConfiguration : IConfiguration
{
    public string? this[string key]
    {
        get => ConfigurationManager.AppSettings[key];
        set => throw new NotSupportedException();
    }

    public IEnumerable<IConfigurationSection> GetChildren() =>
        throw new NotSupportedException();

    public IChangeToken GetReloadToken() =>
        throw new NotSupportedException();

    public IConfigurationSection GetSection(string key) =>
        throw new NotSupportedException();
}
