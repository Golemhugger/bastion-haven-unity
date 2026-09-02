# BASTION — Haven (Unity 6)

Post-collapse city command sim. You run **The Ward**: peacekeepers in the streets, strike teams with extreme prejudice. Rebuild Haven. No gacha.

This is a **script-drop project**. It builds the 5×5 city, people, cameras, and first two minutes at runtime. You do not need Asset Store packs.

## Open in Unity (do this)

1. Install **Unity Hub** and **Unity 6** (6.3 LTS / 6000.3 if you have it; any Unity 6 works).
2. **New project** → template **Universal 3D** (URP). Name it `BastionHaven`.
3. Copy this repo’s `Assets/Bastion` folder into that project’s `Assets/` folder.
4. Open the default scene (or a new empty scene).
5. Press **Play**. A bootstrap component attaches itself automatically.

If Play is a blank sky: look at the Console. If URP Lit is missing the game falls back to Unlit/Color.

Optional packages (nice, not required):
- AI Navigation (`com.unity.ai.navigation`) — we do not bake a NavMesh; people steer themselves.
- Cinemachine — not used. Camera is custom.

## Controls

| Input | City view | Streets (possess) |
|---|---|---|
| WASD | Pan | Walk |
| Mouse drag | Orbit | Look |
| Scroll | Zoom | Zoom |
| E / Streets | Possess nearest Warden | — |
| Esc | — | Return to city |
| LMB | Select person / lot | — |
| 1 2 4 Space | Speed / pause | same |

## First two minutes

Haven breathes. The cisterns will not.

1. Water is short. Queue a **Cistern** on Wire Street.
2. Crews walk to the pad. Water recovers.
3. **Ash Row** tips. Post two Wardens.
4. **Camp West** grows teeth. Strike, or they raid the stores.

Prejudice raises odds and drops morale.

## What is in here

| Script | Job |
|---|---|
| `BastionGame` | Auto-boot, loop, camera, input |
| `GameSim` | Day tick, resources, missions |
| `CityFactory` | Lots, roads, HQ, hydro, habs, interiors, clutter |
| `PersonActor` | Low-poly Warden / civilian / raider + jobs |
| `HudOverlay` | Command HUD + event cards |

No loot boxes. Progress is food, water, order, scrap, and doctrine later.

## Honest scope

This is a playable Unity **foundation**, not a finished store build. People are assembled primitives. Lighting is URP-safe dusk. Next in-Editor pass: baked NavMesh, painted facades, Timeline hatch, Audio Mixer beds.
