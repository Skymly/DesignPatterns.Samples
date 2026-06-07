# DesignPatterns.Samples — AI agent notes

## Scope

Runnable console samples only. Generator, runtime, and analyzer changes belong in [DesignPatterns](https://github.com/Skymly/DesignPatterns).

## Layout

| Path | Purpose |
|------|---------|
| `DesignPatterns.Samples.*/` | One sample per design pattern |
| `Directory.Build.props` | `UseLocalDesignPatterns` (default `true` → sibling `../DesignPatterns`) |
| `Directory.Build.targets` | ProjectReference vs future NuGet by `DesignPatternsSampleKind` |
| `build/Program.cs` | Nuke `Ci` — build and run every sample |

## Commands

```powershell
dotnet build DesignPatterns.Samples.slnx -c Release
./build.ps1 --target Ci --configuration Release
```

CI checks out [Skymly/DesignPatterns](https://github.com/Skymly/DesignPatterns) as a sibling folder so `UseLocalDesignPatterns=true` works without a published NuGet package.

## Language

- User-facing README: English (match Observables.Samples).
- Commit messages and GitHub Issue/PR text: **English**.

## Git

- Do not commit or push unless the user asks.
