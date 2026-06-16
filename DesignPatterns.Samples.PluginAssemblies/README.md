# Plugin assemblies sample

Runnable demonstration of **compile-time plugin registration** across multiple assemblies using `[RegisterStrategy]`, generated registries, and Autofac `RegisterAutofac`.

Maps to [PluginAssemblies.md](https://github.com/Skymly/DesignPatterns/blob/main/docs/PluginAssemblies.md) in the main DesignPatterns repo.

## Layout

```
Contracts/                 Shared interfaces (no provider code)
Providers.Alpha/           Card motion — key "alpha"
Providers.AlphaConflict/   Card motion — duplicate key "alpha" (DP033 demo only)
Providers.Beta/            Card motion — key "beta" (optional reference)
Providers.Gamma/           FC control + error — companion key "gamma"
Host/                      Autofac host (references Alpha + Gamma, not Beta)
Scenarios.InvalidKey/      DP025 compile-time failure demo
Scenarios.DuplicateKey/    DP033 compile-time failure demo
```

Each provider assembly emits its own `{Contract}Registry` in the **contract namespace** (e.g. `PluginAssemblies.Sample.Contracts.CardMotionRegistry` inside `Providers.Alpha.dll`).

## Scenarios

| ID | How to run | Expected result |
|----|------------|-----------------|
| **S1** | `dotnet run --project Host` | Starts; prints `Card=alpha`; `CardMotionRegistry.Keys` contains `alpha`, not `beta` |
| **S2** | `dotnet run --project Host -- s2` | Exit code 1; `App.config` key `CardMissing=beta` resolves via `RegistryConfiguration` but `beta` is not registered (Providers.Beta not referenced) |
| **S3** | `dotnet test` in sibling DesignPatterns repo (`UnknownRegistryKeyAnalyzerTests`) or IDE on `Scenarios.InvalidKey` | Diagnostic **DP025** for unknown literal key |
| **S4** | `dotnet test` in sibling DesignPatterns repo (`CrossAssemblyRegistryKeyAnalyzerTests`) or IDE on `Scenarios.DuplicateKey` | Diagnostic **DP033** when Alpha and AlphaConflict both register `alpha` |

`Host/App.config` selects `Card=alpha` and `FC=gamma` for S1. The host uses `DesignPatterns.Extensions.AppSettings.RegistryConfiguration` to map those keys to strategy registries (`CardMissing=beta` drives S2).

`Scenarios.InvalidKey/InvalidKeyUsage.cs` shows the invalid literal pattern; DP025 is enforced by the DesignPatterns analyzer (IDE or NuGet package). Local sibling `ProjectReference` builds may not surface Info-level diagnostics on the command line — CI runs the analyzer unit test instead.

`Scenarios.DuplicateKey` references **Providers.Alpha** and **Providers.AlphaConflict** (both register `ICardMotion` key `alpha`). DP033 is an **Error**; IDE or command-line builds with analyzers should fail. CI falls back to `CrossAssemblyRegistryKeyAnalyzerTests` when the demo project build does not surface DP033.

## Prerequisites

Sibling [DesignPatterns](https://github.com/Skymly/DesignPatterns) clone with `DesignPatterns.Extensions.Autofac` and `DesignPatterns.Extensions.AppSettings` (merged on `main`).

```powershell
cd DesignPatterns.Samples
dotnet run --project DesignPatterns.Samples.PluginAssemblies/Host -c Release
```

## CI

`./build.ps1 --target Ci` builds the host, runs S1, asserts S2 failure output, and runs sibling DesignPatterns analyzer tests for S3 (DP025) and S4 (DP033).
