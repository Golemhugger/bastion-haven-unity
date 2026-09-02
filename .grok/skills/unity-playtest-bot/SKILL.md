---
name: unity-playtest-bot
description: Write command lists for a Grok Bot that can open Unity, click HUD, screenshot, and quit. Use when the user says paste to Steward, playtest, or give the bot commands.
metadata:
  author: bastion-haven
  short-description: Editor bot playtest lists
---

# Unity Playtest Bot

One paste block. Numbered. Observable checks only. Quit Unity at the end.

## Always include

- Project path
- Do not add URP / Cinemachine / NavMesh / .exe
- Maximize Game view before Play
- Start paused on Day 1. If Day is stale, click New Game
- Named clicks that change HUD numbers
- Screenshot Game view, not Scene view
- File → Exit Unity
- Fixed report template

## Never

- manage / make it better with no pass-fail
- 4x overnight
- leave Play running
- treat an AFK Day-100 screenshot as a click-through

## Report template

```
Play result:
- paused Day 1:
- clicks that worked:
- clicks that failed:
- Day / Water / Food / Order / Scrap / Wardens:
- pink / Console red:
- screenshot notes:
- Unity closed:
```
