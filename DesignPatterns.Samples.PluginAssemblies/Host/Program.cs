using System.Configuration;
using Autofac;
using DesignPatterns.Extensions.AppSettings;
using PluginAssemblies.Sample.Contracts;
using PluginAssemblies.Sample.Providers.Alpha;
using PluginAssemblies.Sample.Providers.Gamma;

var scenario = args.FirstOrDefault() ?? "s1";
var cardAppSettingsKey = scenario.Equals("s2", StringComparison.OrdinalIgnoreCase) ? "CardMissing" : "Card";

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
        cardAppSettingsKey,
        out var card,
        defaultKey: CardMotionKeys.Alpha))
{
    var configuredValue = ConfigurationManager.AppSettings[cardAppSettingsKey];
    var strategyKey = string.IsNullOrWhiteSpace(configuredValue) ? CardMotionKeys.Alpha : configuredValue;
    Console.Error.WriteLine(
        $"Card provider '{strategyKey}' is not registered. Available keys: {cardKeys}. " +
        "Reference the matching provider assembly (e.g. Providers.Beta for 'beta').");
    return 1;
}

if (!RegistryConfiguration.TryResolveConfigured(
        fcRegistry,
        "FC",
        out var fc,
        defaultKey: FCControlKeys.Gamma)
    || !RegistryConfiguration.TryResolveConfigured(
        fcErrorRegistry,
        "FC",
        out var fcError,
        defaultKey: FCControlKeys.Gamma))
{
    var fcKey = ConfigurationManager.AppSettings["FC"] ?? FCControlKeys.Gamma;
    Console.Error.WriteLine($"FC provider '{fcKey}' is not registered for both IFCControl and IFCError.");
    return 1;
}

Console.WriteLine($"Card={card!.ProviderName}, FC={fc!.ProviderName}, FCError={fcError!.ProviderName}");
return 0;
