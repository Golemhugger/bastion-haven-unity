---
name: game-loop-design
description: Design a no-gacha survival city loop with dual-role military police. Use when tuning food, water, order, strikes, doctrine, or first-session pressure.
metadata:
  author: bastion-haven
  short-description: Survival plus Wardens, no gacha
---

# Game Loop Design

First session must force a decision in under two minutes. No loot boxes.

## First loop

1. Water is dying. Queue cistern (costs scrap, two days).
2. Order is dying. Post Ash Row (spend Wardens, Order jumps on the click).
3. Threat sits at Camp West. Launch strike (spend Wardens, flashes, resolve in days).

If a click does not change a HUD number the same frame or the next day tick, the verb is fake.

## Numbers

Start paused. Show deltas (`Water 14 -10`). Hide verbs that already fired.
New Game does not write the save. F5 writes. F9 reads. Play does not auto-load.

Order decay while unposted. Instant bump on Post so the player feels the beat.
Strike buttons hide while `OnStrike > 0`.

Population growth is gated on food and water surplus. Do not grow on Day 1.

## Doctrine

Pax Haven vs Iron Haven is a fork, not a skin. It changes strike odds, morale, and the late toast. Do not add a third doctrine until both endings Play-test.
