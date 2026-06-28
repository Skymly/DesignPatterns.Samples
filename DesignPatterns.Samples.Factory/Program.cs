using DesignPatterns.Creational;
using Factory.Sample;

var registry = new FactoryRegistryBuilder<string, IProduct>()
    .Register("standard", () => new StandardProduct())
    .Register("premium", () => new PremiumProduct())
    .Build();

var standard = registry.Create("standard");
var premium = registry.Create("premium");

Console.WriteLine($"Created: {standard.Name}");
Console.WriteLine($"Created: {premium.Name}");

var standardAgain = registry.Create("standard");
Console.WriteLine($"Same instance? {ReferenceEquals(standard, standardAgain)}");

if (!registry.TryCreate("unknown", out _))
{
    Console.WriteLine("Unknown key not found (expected).");
}

try
{
    registry.Create("missing");
}
catch (FactoryNotFoundException ex)
{
    Console.WriteLine($"Create missing key: {ex.Message}");
}

// Async factory registry: IAsyncFactoryRegistry with CreateAsync.
Console.WriteLine();
Console.WriteLine("=== Async factory registry (IAsyncFactoryRegistry) ===");

var asyncRegistry = new AsyncFactoryRegistryBuilder<string, IProduct>()
    .Register("async-standard", () => new StandardProduct())
    .Register("async-premium", ct => new ValueTask<IProduct>(new PremiumProduct()))
    .Build();

var asyncProduct = await asyncRegistry.CreateAsync("async-standard");
Console.WriteLine($"Async created: {asyncProduct.Name}");

var asyncPremium = await asyncRegistry.CreateAsync("async-premium");
Console.WriteLine($"Async created: {asyncPremium.Name}");

// Pooled factory registry: IPooledFactoryRegistry with RentAsync / Return.
Console.WriteLine();
Console.WriteLine("=== Pooled factory registry (IPooledFactoryRegistry) ===");

var pooledBuild = new AsyncFactoryRegistryBuilder<string, PooledBuffer>()
    .Register("buffer", () => new PooledBuffer())
    .WithPooling(poolSize: 4)
    .Build();

// Build() returns IAsyncFactoryRegistry; cast to IPooledFactoryRegistry when pooling is enabled.
var pooledRegistry = (IPooledFactoryRegistry<string, PooledBuffer>)pooledBuild;

var buffer1 = await pooledRegistry.RentAsync("buffer");
var buffer2 = await pooledRegistry.RentAsync("buffer");
Console.WriteLine($"Rented two buffers: same instance? {ReferenceEquals(buffer1, buffer2)} (expected: false)");

pooledRegistry.Return("buffer", buffer1);
pooledRegistry.Return("buffer", buffer2);

var buffer3 = await pooledRegistry.RentAsync("buffer");
var buffer4 = await pooledRegistry.RentAsync("buffer");
Console.WriteLine($"After return + rent: buffer3 is buffer1? {ReferenceEquals(buffer3, buffer1)} or buffer2? {ReferenceEquals(buffer3, buffer2)} (expected: one of them)");
Console.WriteLine($"buffer4 is the other? {ReferenceEquals(buffer4, buffer1) || ReferenceEquals(buffer4, buffer2)} (expected: true)");
