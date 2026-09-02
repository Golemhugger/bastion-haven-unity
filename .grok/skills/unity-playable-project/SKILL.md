---
name: unity-playable-project
description: Build and playtest a Hub-openable Unity 6 game that boots from C# at Play with no scene setup. Use when creating or managing a Unity project, fixing Safe Mode, writing RuntimeInitializeOnLoad bootstraps, OnGUI command HUDs, procedural city/people meshes, colony-sim loops, or writing command lists for a Grok Bot that can open the Editor. Complements official Unity CLI and package skills. Does not produce an .exe or add Asset Store packs by default.
metadata:
  author: bastion-haven
  short-description: Unity 6 playable drop-in plus Editor playtest protocol
argument-hint: "[project-path]"
---

# Unity Playable Project

Ship a folder Unity Hub can Open, then prove Play in the Game view before adding packages, art pipelines, or builds.

## When to use

- New Unity 6 prototype, colony sim, city builder, or runtime-generated 3D scene
- User says Play, Safe Mode, compile errors, white/pink Game view, or manage my Unity project
- Writing command lists for a Bot that can click the Editor
- Drop-in scripts under `Assets/` that must compile on Built-in RP and URP

## When not to use

- Official Editor install, Hub templates, or UPM add/remove — use `unity-cli` / `unity-package-management`
- UI Toolkit / uGUI canvases as the first HUD — OnGUI is allowed until Play is proven
- NavMesh bake, Cinemachine, HDRP, Addressables, or a Windows build until the loop Play-tests

## Hard rules

1. A Unity project is a folder with `Assets/`, `Packages/manifest.json`, and `ProjectSettings/`. There is no `.unityproj`.
2. Never create, commit, or delete `Library/`, `Temp/`, `Logs/`, `obj/`.
3. Never invent an `.exe`, installer, or Asset Store package.
4. Never add URP, Cinemachine, or NavMesh to fix Play. Add URP only if materials are magenta after Shader.Find fallbacks fail.
5. Match existing public C# APIs. Do not rename methods a boot script already calls.
6. No gacha. Progress is earned by player decisions.
7. Stop and report after each Play-test. Do not keep generating while the Editor sits in Play.
8. Close the Editor when the playtest is done.

## Project layout

```
ProjectName/
  Assets/<Game>/
    Boot.cs              RuntimeInitializeOnLoad — required for Play
    GameSim.cs           day tick, resources, missions
    CityFactory.cs       procedural meshes + BastionGfx
    PersonActor.cs       Spawn / Tick / Drop / SetTarget
    SaveSystem.cs        PlayerPrefs or JsonUtility if the jsonserialize module exists
  Packages/manifest.json
  ProjectSettings/ProjectVersion.txt
  .gitignore             Library, Temp, Logs, obj, Build, *.csproj, *.sln
```

Scripts live under `Assets/<Game>/`, never the repo root, never inside `Library/`.
Empty `SampleScene` is fine if Boot attaches itself.

Target Unity 6 LTS (`6000.0.x`). Accept Hub patch upgrades (`49f1` → `53f1`).

## Boot

```csharp
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
static void Boot()
{
    if (Object.FindFirstObjectByType<GameBoot>()) return;
    var go = new GameObject("GAME");
    go.AddComponent<GameBoot>();
}
```

Use `FindFirstObjectByType` (not obsolete `FindObjectOfType`).
On first Play: set sim speed to Pause so Day does not run away.
Do not auto-load a save on Play. New Game replaces in-memory sim and must not write PlayerPrefs.

## Shaders

```csharp
Shader.Find("Universal Render Pipeline/Lit")
    ?? Shader.Find("Universal Render Pipeline/Simple Lit")
    ?? Shader.Find("Standard")
    ?? Shader.Find("Unlit/Color")
    ?? Shader.Find("Sprites/Default");
```

Set `_BaseColor` and `_Color`. Enable `_EMISSION` only when emit > 0.
Pink/magenta = shader miss. Flatten look is acceptable. Dead Play is not.

## Safe Mode / compile

If Unity drops into Safe Mode:

1. Read the Console. Group errors by missing type or missing method.
2. Add the missing method on the callee. Do not rewrite the caller unless the signature is wrong.
3. Common holes from split authors: `Strip()`, `Active`/`OnResolved`, `JsonUtility` without `com.unity.modules.jsonserialize`.
4. Prefer PlayerPrefs string saves over adding packages in Safe Mode.
5. Exit Safe Mode (Ignore or reopen). Confirm 0 red. Then Play.

One-line API patches beat new systems.

## Playtest protocol

1. Exit Play if a leftover session is running.
2. Maximize the Game view so OnGUI is not under the Inspector.
3. Enter Play. Pause immediately. HUD must show Day 1 (or New Game to force it).
4. Prove the loop with clicks that change numbers (scrap, order, water).
5. Screenshot Game view, not Scene view.
6. Ignore DirMon yellows about `UnityDirMonSyncFile` under Hub Editor Data.
7. Exit Play. File → Exit Unity. Do not leave Play running.

Identical HUD numbers after a claimed click-through means the leftover session was never reset.

## Look pass (primitives only)

Until FBX exists:

- Buildings = cubes/cylinders + roof slab + door recess + window slits + one emissive accent
- People = box torso/head/limbs + role silhouette (coat/visor vs scarf vs plates) + blob shadow
- Hull colliders on enterable buildings are triggers. Keep a floor collider. Cut a south-face door.
- VFX visible from the city camera: spark scale ≥ 1.4, y ≥ 2.5, lifetime ≥ 0.45s, Point light range 8
- Do not switch to skinned meshes mid-loop.

## Camera

Clamp city orbit: pitch `0.25..1.05`, distance `12..80`.
On a strike or off-screen event, snap look-at and pull distance so the subject fills the Game view.
Black Game view after a drag = unclamped pitch. Fix the clamp, do not add Cinemachine yet.

## HUD

OnGUI is valid for the first playable.
Put New Game / Save / Load at the top of the command panel.
Size the panel tall enough that doctrine/research buttons do not clip (`~240x320`, not `220x140`).
Hide actions that already fired (queued cistern, posted beat, strike in flight).
Show resource deltas (`Water 14 -10`).

## Save

- F5 / Save writes one slot.
- F9 / Load reads that slot.
- New Game does not write the slot.
- Play start does not load the slot.
Prove with: F5 on Day X → New Game Day 1 → F9 returns Day X.

## Command lists for an Editor Bot

Write one paste block. Numbered. After each block: Play, report, wait.
Every item must be observable in the Game view or Console.
End with File → Exit Unity.
Never say manage or make it better without a pass/fail check.

## Play report template

```
Play result:
- project path:
- Unity version:
- paused Day 1: yes/no
- clicks that worked:
- clicks that failed:
- Day / Water / Food / Order / Scrap / Wardens:
- pink materials: yes/no
- Console red:
- screenshot notes:
- Unity closed: yes
```

## After Play is proven

Only then: painted facades, Cinemachine street rig, NavMesh bake, Audio Mixer, URP volume, Windows build.
Still no gacha.
