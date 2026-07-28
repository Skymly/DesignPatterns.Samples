# Issue tracker: GitHub

Issues and PRDs for this repo live as GitHub issues on **`Skymly/DesignPatterns.Samples`**. Use the `gh` CLI for all operations.

## Conventions

- **Create**: `gh issue create --repo Skymly/DesignPatterns.Samples --title "..." --body "..."`
- **Read**: `gh issue view <n> --repo Skymly/DesignPatterns.Samples --comments`
- **List**: `gh issue list --repo Skymly/DesignPatterns.Samples --state open --json number,title,body,labels,comments`
- **Comment / label / close**: `gh issue comment` / `gh issue edit --add-label` / `gh issue close` with `--repo Skymly/DesignPatterns.Samples`

When cwd is this clone, `gh` may omit `--repo`; keep it when coordinating with siblings.

## Pull requests as a triage surface

**PRs as a request surface: no.**

## When a skill says "publish to the issue tracker"

Create a GitHub issue **in this repo** for sample-app work. Library / user-docs / maintainer-doc work must go to the owning sibling (below).

## Sibling issue routing

| Local path | GitHub repo | Owns |
|------------|-------------|------|
| `C:\Code\Skymly\DesignPatterns\DesignPatterns` | `Skymly/DesignPatterns` | Library + maintainer `docs/` |
| `C:\Code\Skymly\DesignPatterns\DesignPatterns.Docs` | `Skymly/DesignPatterns.Docs` | User VitePress site |
| `C:\Code\Skymly\DesignPatterns\DesignPatterns.Samples` | `Skymly/DesignPatterns.Samples` | **This repo** (console samples + sample CI) |

**Rules**

- Execution issues for sample projects / sample CI land **here**.
- Do **not** open library or user-docs execution issues in this tracker.
- Default local layout: sibling `../DesignPatterns/` with `UseLocalDesignPatterns=true` when `DesignPatterns.slnx` exists.
- Cross-feature work: parent/map on DesignPatterns; this issue links `Relates to` / `Blocked by` the library issue URL. No dual-filed full AC.

## Wayfinding operations

Same `wayfinder:*` label vocabulary as the core repo. Cross-repo map children use URL checklists + `Relates to`, not GitHub sub-issues.
