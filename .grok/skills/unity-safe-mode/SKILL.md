---
name: unity-safe-mode
description: Recover a Unity 6 project from Safe Mode. Use when Play never starts, Console is red, a type or method is missing, LoadImage fails, or a script was spliced between methods.
metadata:
  author: bastion-haven
  short-description: Safe Mode compile recovery from real Bastion hits
---

# Unity Safe Mode

Safe Mode means scripts failed to compile. There is no Game view city. Do not Play. Do not add packages to escape it. Patch the missing API, Exit Safe Mode, confirm 0 red, then Play paused.

## Why we actually entered Safe Mode

| Console | Cause | Fix |
| --- | --- | --- |
| `JsonUtility` missing | `com.unity.modules.jsonserialize` not in manifest | Rewrite save to PlayerPrefs. Do not add the module mid-Safe-Mode. |
| `StrikeDirector.Active` / `OnResolved` missing | Zip dropped Boot that called APIs the Strike file did not have | Add the property + event on the callee. |
| `CityFactory.Strip` missing | Road builder called a helper that was never shipped | Add `Strip()`, do not rewrite every road. |
| `BastionNoise.ZeroOne` / `Tint` CS0117 | Textures script called helpers Noise never defined | Add the methods (overloads) on Noise before shipping Textures. |
| `Texture2D.LoadImage` missing | `com.unity.modules.imageconversion` not in manifest | Load via reflection + noise fallback. Add the module only when Unity is closed and compiling clean. |
| `BastionRoster.Fill` between methods in Boot | Bot or agent pasted a call into the middle of a class | Move the call inside an existing method (`SpawnCrowd`). Never insert statements between methods. |
| CS0618 `FindObjectOfType` | Obsolete API | `FindFirstObjectByType`. Warning, not Safe Mode. |

DirMon / "Couldn't create ... PackageManager\\BuiltinPackages" yellows under `Program Files\Unity\Hub` are Hub file-lock permissions. Ignore them. They are not compile errors.

## Protocol

1. Read Console. Group: missing type, missing method, syntax (brace / statement between methods), missing module.
2. Patch the **callee** with the smallest matching API. Do not rewrite the caller unless the signature is wrong.
3. Compile the set as one API. If Boot calls `X.Y`, the zip that contains Boot must also contain `Y`.
4. Exit Safe Mode. Confirm 0 red. Then maximize Game view and Play paused.
5. Never Play from Safe Mode. Never leave Unity in Safe Mode overnight.

## Compile contract (ship rule)

Before a zip leaves the studio:

- Every public call site has a definition in the same zip.
- No new packages required to compile.
- New helpers (`ZeroOne`, `Tint`, `Roof`, `Rust`, `Strip`) live on the library file, not as comments in README.
- Additive overlay scripts (`StreetDress`, `CityLife`, `DoorCut`) use `RuntimeInitializeOnLoad` so Boot does not get spliced.
- Do not overwrite a fat GoosePc `GameSim` / `BastionBoot` with a thinner GitHub copy. You will delete farm, recruit, and Steward's Fill fix.

## Overlay instead of Boot surgery

Prefer a new `MonoBehaviour` with:

```csharp
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
static void Attach()
{
    if (Object.FindFirstObjectByType<MyOverlay>()) return;
    new GameObject("MyOverlay").AddComponent<MyOverlay>();
}
```

If two overlays would spawn the same clinic, one of them must no-op when the object already exists.
