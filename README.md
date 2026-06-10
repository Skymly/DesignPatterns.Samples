# DesignPatterns.Samples

Runnable console applications for **[Skymly/DesignPatterns](https://github.com/Skymly/DesignPatterns)** — Strategy, Chain of Responsibility, Composite, Factory Registry, Decorator, Event Aggregator, Singleton, and MSDI integration.

## Prerequisites

- [.NET SDK 8](https://dotnet.microsoft.com/download) (see [`global.json`](global.json))

## Clone layout

Samples default to a **local sibling** of the generator repo (no NuGet publish required yet):

```
Skymly/
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
| **DesignPatterns.Samples.Strategy** | `[RegisterStrategy]` → Keys + static `Instance` registry; sync pay + async `ExecuteAsync` |
| **DesignPatterns.Samples.Chain** | `[HandlerOrder]` → generated handler pipeline |
| **DesignPatterns.Samples.Composite** | `[CompositePart]` → `BuildForest()` / `TraverseForest` (+ `BuildRoot`, manual builder) |
| **DesignPatterns.Samples.Factory** | `[RegisterFactory]` factory registry |
| **DesignPatterns.Samples.RegisterFactory** | Manual `FactoryRegistryBuilder` registration |
| **DesignPatterns.Samples.Decorator** | `[Decorator]` → stack + `DecoratorOrder` + conditional `Add` |
| **DesignPatterns.Samples.EventAggregator** | `IEventAggregator` publish/subscribe |
| **DesignPatterns.Samples.GenerateSingleton** | `[GenerateSingleton]` lazy singleton |
| **DesignPatterns.Samples.DependencyInjection** | `RegisterDi` for Strategy / Factory / Handler |

## Future NuGet consumption

When `DesignPatterns` is published, set `-p:UseLocalDesignPatterns=false` and pin `DesignPatternsPackageVersion` in `Directory.Build.props`.

## Related

| Link | Description |
|------|-------------|
| [DesignPatterns](https://github.com/Skymly/DesignPatterns) | Runtime, source generators, tests |
| [DesignPatterns.Docs](https://github.com/Skymly/DesignPatterns.Docs) | User documentation (VitePress) |

## License

MIT — see [LICENSE](LICENSE).
