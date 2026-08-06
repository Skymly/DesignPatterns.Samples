# DesignPatterns.Samples — AI agent notes

## Scope

This repository contains **sample console apps only**. Do not add generators or library code here — change [DesignPatterns](https://github.com/Skymly/DesignPatterns) instead.

## Local development

- **Default sibling layout**: `../DesignPatterns/` with `UseLocalDesignPatterns=true` when `DesignPatterns.slnx` exists (`Directory.Build.props`).
- **Run one sample**: `dotnet run --project DesignPatterns.Samples.Strategy -c Release`
- **CI**: `./build.ps1 --target Ci --configuration Release` from repo root.

## Language

- User chat: 简体中文 (unless requested otherwise).
- Commit messages and GitHub Issue/PR text: **English**.

## Git

- Do not commit or push unless the user asks.
- Do not bump DesignPatterns package versions without user approval.

## Agent skills

### Issue tracker

Issues live in this repo's GitHub Issues (via `gh`). See `docs/agents/issue-tracker.md`.

### Triage labels

Default roles: `needs-triage`, `needs-info`, `ready-for-agent`, `ready-for-human`, `wontfix`. See `docs/agents/triage-labels.md`.

### Domain docs

Single-context layout (`CONTEXT.md` + `docs/adr/` at repo root). See `docs/agents/domain.md`.
