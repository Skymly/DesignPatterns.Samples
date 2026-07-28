# DesignPatterns.Samples — AI agent notes

## Scope

This repository contains **sample console apps only**. Do not add generators or library code here — change [DesignPatterns](https://github.com/Skymly/DesignPatterns) instead. User guides belong in [DesignPatterns.Docs](https://github.com/Skymly/DesignPatterns.Docs).

## Agent skills

Config: [`docs/agents/`](docs/agents/) (`issue-tracker.md`, `triage-labels.md`, `domain.md`).

| Change lands in… | File execution issue on… | Local path |
|------------------|--------------------------|------------|
| Sample projects / sample CI | **`Skymly/DesignPatterns.Samples`** (this repo) | `C:\Code\Skymly\DesignPatterns\DesignPatterns.Samples` |
| Library / maintainer docs | `Skymly/DesignPatterns` | `C:\Code\Skymly\DesignPatterns\DesignPatterns` |
| User VitePress site | `Skymly/DesignPatterns.Docs` | `C:\Code\Skymly\DesignPatterns\DesignPatterns.Docs` |

Cross-feature: parent/map may live on DesignPatterns; link with `Relates to` / `Blocked by` URLs. Do not dual-file full acceptance criteria.

## Local development

- **Default sibling layout**: `../DesignPatterns/` (`C:\Code\Skymly\DesignPatterns\DesignPatterns`) with `UseLocalDesignPatterns=true` when `DesignPatterns.slnx` exists (`Directory.Build.props`).
- **Run one sample**: `dotnet run --project DesignPatterns.Samples.Strategy -c Release`
- **CI**: `./build.ps1 --target Ci --configuration Release` from repo root.

## Language

- User chat: 简体中文 (unless requested otherwise).
- Commit messages and GitHub Issue/PR text: **English**.

## Git

- Do not commit or push unless the user asks.
- Do not bump DesignPatterns package versions without user approval.
