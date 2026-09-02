# BASTION studio

Four agents. One ship.

- Grok — producer, Boot/HUD, user answer, Steward lists
- Harper — sim, save, tech, events
- Lucas — city, interiors, camp, props
- Benjamin — people, noise, art, skills

Skills live in `.grok/skills/`:

- `unity-playable-project`
- `studio-handoff`
- `unity-safe-mode`
- `unity-playtest-bot`
- `unity-runtime-art`
- `game-loop-design`

Install on GoosePc as `C:\\Users\\jandk\\Bastion\\.grok\\skills\\<name>\\SKILL.md`
and globally as `%USERPROFILE%\\.grok\\skills\\<name>\\SKILL.md`.

Official complement, not a replacement:

```
npx skills add Unity-Technologies/skills
grok plugin install Unity-Technologies/unity-agent-plugin --trust
```
