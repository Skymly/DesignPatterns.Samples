# DesignPatterns.Samples

Sample console applications only for [Skymly/DesignPatterns](https://github.com/Skymly/DesignPatterns). Source generators and library changes belong in the main repository.

## Build & Verify

```powershell
# Run an individual sample:
dotnet run --project DesignPatterns.Samples.Strategy -c Release

# CI-equivalent Nuke target:
./build.ps1 --target Ci --configuration Release
```

- When a sibling `../DesignPatterns/DesignPatterns.slnx` checkout exists, `Directory.Build.props` defaults `UseLocalDesignPatterns=true` to test against local sources instead of NuGet.

## Docs & Agent Config

- Issue routing & triage labels: [`docs/agents/`](docs/agents/)

## Conventions

- Commit messages and GitHub Issue/PR text: **English**.
- Do not bump `DesignPatterns` package versions without maintainer approval.
- Do not commit or push unless explicitly requested.
