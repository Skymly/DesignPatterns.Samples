# Domain Docs

## Before exploring, read these

- **`AGENTS.md`** at the repo root.
- **`docs/agents/issue-tracker.md`** — where issues live and sibling routing.
- Library API / generators: sibling `../DesignPatterns/` (`AGENTS.md`, `docs/design/`). Samples demonstrate the library — do not add generators or library code here.

## File structure

```
/
├── AGENTS.md
├── docs/
│   └── agents/           ← skills config
├── DesignPatterns.Samples.*/
└── build/
```

## Local development

- Sibling layout: `../DesignPatterns/`
- Run one sample: `dotnet run --project DesignPatterns.Samples.<Name> -c Release`
- CI: `./build.ps1 --target Ci --configuration Release`
