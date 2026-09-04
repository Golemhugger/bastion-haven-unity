---
name: unity-playtest-bot
description: Write command lists for a Grok Bot that opens Unity, clicks HUD, screenshots, and quits. Use when the user says Steward, playtest, or paste commands.
metadata:
  author: bastion-haven
  short-description: Editor bot lists that actually click
---

# Unity Playtest Bot

One paste block. Numbered. Observable checks. Quit Unity at the end.

## Why clicks miss

- Game view not maximized — HUD buttons sit under Inspector / Hierarchy.
- Bot stares at a screenshot instead of clicking the Game view.
- First click hits Load F9 because New Game is clipped.
- Space / Shift+Space is bound as speed and advances the day.
- Unity left in leftover Play (Day 89) — numbers look like the click did nothing.
- AFK 4x overnight is not a playtest.

## Always

1. Unity closed before a drop.
2. Drop only the files named in the list.
3. Open. Confirm 0 red. Exit Safe Mode if needed.
4. Maximize Game view. Hide Console overlay.
5. Play paused Day 1. New Game if Day is stale.
6. Named clicks. Named screenshots of Game view.
7. Do not re-test beats already signed off this week.
8. File → Exit Unity.

## Never

- "manage" / "make it better" with no pass-fail
- 4x overnight
- leave Play running on a laptop
- treat an AFK Day-100 shot as a click-through
- drop nightpack + tomorrow + LATEST in one pass

## Report template

```
Play result:
- red / pink:
- paused Day 1:
- clicks that worked:
- clicks that failed:
- named visual (shack / beds / sparks / lobby):
- Unity closed:
```
