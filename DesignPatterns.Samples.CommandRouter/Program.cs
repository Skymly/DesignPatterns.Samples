using DesignPatterns.Behavioral;
using DesignPatterns.Extensions.DependencyInjection;
using DesignPatterns.Samples.CommandRouter;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("=== Manual CommandRouterBuilder ===");

var manualRouter = new CommandRouterBuilder()
    .Register(new PingCommandHandler())
    .Register(new GetTotalCommandHandler())
    .Build();

await manualRouter.SendAsync(new PingCommand());
var manualTotal = await manualRouter.SendAsync<GetTotalCommand, decimal>(new GetTotalCommand(19.99m, 3));
Console.WriteLine($"  SendAsync result: {manualTotal:C}");

if (!await manualRouter.TrySendAsync(new UnregisteredCommand()))
{
    Console.WriteLine("  TrySendAsync(UnregisteredCommand) returned false (expected).");
}

Console.WriteLine();
Console.WriteLine("=== Generated [RegisterCommandHandler] (CreateRouter / RegisterAll) ===");

// Single-command convenience: CreateRouter wires that command's handler only.
var pingRouter = PingCommandHandlerRegistry.CreateRouter();
await pingRouter.SendAsync(new PingCommand());

// Combine registries when more than one command type is registered.
var generatedBuilder = new CommandRouterBuilder();
PingCommandHandlerRegistry.RegisterAll(generatedBuilder);
GetTotalCommandHandlerRegistry.RegisterAll(generatedBuilder);
var generatedRouter = generatedBuilder.Build();

await generatedRouter.SendAsync(new PingCommand());
var generatedTotal = await generatedRouter.SendAsync<GetTotalCommand, decimal>(new GetTotalCommand(9.50m, 2));
Console.WriteLine($"  SendAsync result: {generatedTotal:C}");

var attempt = await generatedRouter.TrySendAsync<GetTotalCommand, decimal>(new GetTotalCommand(5m, 4));
Console.WriteLine($"  TrySendAsync success={attempt.Success}, result={attempt.Result:C}");

Console.WriteLine();
Console.WriteLine("=== DI integration (RegisterDi + AddCommandRouter) ===");

var services = new ServiceCollection();
PingCommandHandlerRegistry.RegisterDi(services);
GetTotalCommandHandlerRegistry.RegisterDi(services);
services.AddCommandRouter((builder, sp) =>
{
    PingCommandHandlerRegistry.RegisterAll(builder, sp);
    GetTotalCommandHandlerRegistry.RegisterAll(builder, sp);
});

var provider = services.BuildServiceProvider();
var diRouter = provider.GetRequiredService<ICommandRouter>();

await diRouter.SendAsync(new PingCommand());
var diTotal = await diRouter.SendAsync<GetTotalCommand, decimal>(new GetTotalCommand(12.00m, 5));
Console.WriteLine($"  SendAsync result: {diTotal:C}");

// Demonstrates throwing Send when no handler was registered for the command type.
try
{
    await diRouter.SendAsync(new UnregisteredCommand());
}
catch (CommandHandlerNotFoundException ex)
{
    Console.WriteLine($"  SendAsync(UnregisteredCommand) threw {ex.GetType().Name} (expected).");
}

// Command type deliberately omitted from all registries.
file sealed class UnregisteredCommand : ICommand;
