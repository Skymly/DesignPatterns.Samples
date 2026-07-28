# DesignPatterns.Samples

Runnable console applications for **[Skymly/DesignPatterns](https://github.com/Skymly/DesignPatterns)** — Strategy, Chain of Responsibility, Command Router, Composite, Factory Registry, Decorator, Event Aggregator, State transition table, Singleton, and MSDI integration.

## Prerequisites

- [.NET SDK 8](https://dotnet.microsoft.com/download) (see [`global.json`](global.json))

## Clone layout

Samples default to a **local sibling** of the generator repo for development:

```
<workspace-root>/
  Skymly/
    DesignPatterns/
      DesignPatterns/
      DesignPatterns.Samples/    ← this repo
      DesignPatterns.Docs/
```

```powershell
git clone https://github.com/Skymly/DesignPatterns.git
git clone https://github.com/Skymly/DesignPatterns.Samples.git
cd DesignPatterns.Samples
```

When `../DesignPatterns/DesignPatterns.slnx` exists, `UseLocalDesignPatterns` is **true** by default (`Directory.Build.props`).

## Run one sample

```powershell
dotnet run --project DesignPatterns.Samples.Strategy -c Release
dotnet run --project DesignPatterns.Samples.DependencyInjection -c Release
```

## Run all (CI)

```powershell
./build.ps1 --target Ci --configuration Release
```

Equivalent: `dotnet run --project build/_build.csproj -- --root . --target Ci --configuration Release`

## Projects

| Sample | Demonstrates |
|--------|--------------|
| **DesignPatterns.Samples.Strategy** | `[RegisterStrategy]` → Keys + static `Instance` registry; sync pay + async `ExecuteAsync`; guard predicate (`TryGetWithGuard`); execution tracing (`ExecuteTracedAsync`) |
| **DesignPatterns.Samples.Chain** | `[HandlerOrder]` → generated handler pipeline; traced invocation (`InvokeTracedAsync`); exception observability (`IHandlerExceptionObserver`) |
| **DesignPatterns.Samples.CommandRouter** | Manual `CommandRouterBuilder` + `[RegisterCommandHandler]` → `SendAsync` / `TrySendAsync`; DI via `RegisterDi` + `AddCommandRouter` |
| **DesignPatterns.Samples.Composite** | `[CompositePart]` → `BuildForest()` / `TraverseForest` (+ `BuildRoot`, manual builder) |
| **DesignPatterns.Samples.Factory** | `[RegisterFactory]` factory registry; async factory (`IAsyncFactoryRegistry`); pooled factory (`IPooledFactoryRegistry` with `RentAsync`/`Return`) |
| **DesignPatterns.Samples.RegisterFactory** | Manual `FactoryRegistryBuilder` registration |
| **DesignPatterns.Samples.Decorator** | `[Decorator]` → stack + `DecoratorOrder` + conditional `Add` |
| **DesignPatterns.Samples.EventAggregator** | `IEventAggregator` publish/subscribe; error isolation (`ContinueOnError`); publish tracing (`PublishTracedAsync`) |
| **DesignPatterns.Samples.GenerateSingleton** | `[GenerateSingleton]` lazy singleton |
| **DesignPatterns.Samples.DependencyInjection** | `RegisterDi` for Strategy / Factory / Handler |
| **DesignPatterns.Samples.State** | Manual `TransitionTableBuilder` + `[StateMachine]` / `[Transition]` order lifecycle; guard predicates; entry/exit actions; `IStateMachine` wrapper; `TransitionTrace` |
| **DesignPatterns.Samples.PluginAssemblies** | Multi-assembly `[RegisterStrategy]` + `RegisterAutofac` + `RegistryConfiguration` (`IConfiguration`; see nested [README](DesignPatterns.Samples.PluginAssemblies/README.md)) |

## Published NuGet consumption

The published `0.2.3-preview2` package is pinned in `Directory.Build.props`. To run
the package-backed samples, set `UseLocalDesignPatterns=false`:

```powershell
dotnet run --project build/_build.csproj -- --root . --target Ci --configuration Release --use-local-design-patterns false
```

`PluginAssemblies` also works fully from NuGet (`Skymly.DesignPatterns` + Autofac + Configuration extensions) when `UseLocalDesignPatterns=false`.

## Related

| Link | Description |
|------|-------------|
| [DesignPatterns](https://github.com/Skymly/DesignPatterns) | Runtime, source generators, tests |
| [DesignPatterns.Docs](https://github.com/Skymly/DesignPatterns.Docs) | User documentation (VitePress) |

## License

MIT — see [LICENSE](LICENSE).
