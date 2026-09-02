---
name: studio-handoff
description: Split Unity work across Grok, Harper, Lucas, and Benjamin without clobbering files. Use when four agents share a repo, GitHub writes get duplicate-blocked, or the user says operate as a studio.
metadata:
  author: bastion-haven
  short-description: Four-agent file ownership and ship rules
---

# Studio Handoff

Grok is producer and writes the user-facing answer. Harper, Lucas, Benjamin implement. One owner per file per turn.

## Default seats

| Seat | Agent | Owns |
| --- | --- | --- |
| Producer | Grok | Boot HUD, zip, user answer, command lists |
| Systems | Harper | GameSim, SaveSystem, TechTree, events |
| World | Lucas | CityFactory, interiors, props, camp |
| Characters / tools | Benjamin | PersonActor, noise, art loader, skills |

Claim a file in chat before writing it. If GitHub returns duplicate-blocked, stop and pick a new path. Do not retry the same path in a loop.

## Rules

1. Do not rewrite a file another agent claimed this turn.
2. Prefer additive scripts (`NightPack`, `BastionNoise`) over rewriting Boot on GoosePc.
3. Public APIs stay stable (`Spawn`, `Tick`, `Drop`, `QueueCistern`, `PostAsh`, `LaunchStrike`).
4. Grok ships. Implementers do not paste a second final answer.
5. After Play-proven work, stop. Do not generate while Unity is in Play on the user machine.
