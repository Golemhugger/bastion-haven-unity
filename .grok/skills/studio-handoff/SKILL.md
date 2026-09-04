---
name: studio-handoff
description: Split Unity work across four agents without clobbering files or Safe Mode. Use when Grok/Harper/Lucas/Benjamin share a repo or the user says operate as a studio.
metadata:
  author: bastion-haven
  short-description: Four-agent ownership and ship rules
---

# Studio Handoff

Grok is producer and writes the user-facing answer. Others implement. One owner per file per turn.

## Default seats

| Seat | Agent | Owns |
| --- | --- | --- |
| Producer | Grok | Boot HUD, zip, user answer, Steward lists |
| Systems | Harper | GameSim, Save, Tech, events |
| World | Lucas | CityFactory HQ/camp, interiors |
| Overlay / skills | Benjamin | Additive RuntimeInitialize scripts, noise, art loader, skills |

Claim a file in chat before writing it. Duplicate-blocked GitHub write → pick a new path. Do not retry the same path in a loop.

## Rules

1. Do not rewrite a file another agent claimed this turn.
2. Prefer additive overlays over rewriting Boot on GoosePc.
3. Public APIs stay stable (`Spawn`, `Tick`, `Drop`, `QueueCistern`, `PostAsh`, `LaunchStrike`).
4. Grok ships one zip. Implementers do not paste a second final answer.
5. After Play-proven work, stop generating while Unity is in Play.
6. Do not re-prove signed-off beats (cistern, Ash, sparks, lobby, density, clutter).
7. GoosePc files that Steward patched locally (Boot Fill placement) must be copied back into source before the next full overwrite.
