---
name: unity-safe-mode
description: Recover a Unity 6 project from Safe Mode and compile errors. Use when Play never starts, Console is red, JsonUtility is missing, or a method the boot script calls does not exist.
metadata:
  author: bastion-haven
  short-description: Safe Mode compile recovery
---

# Unity Safe Mode

Compile the set as one API. Do not add packages to escape Safe Mode.

## Hits we actually took

- `JsonUtility` missing → rewrite Save to PlayerPrefs. Do not add `com.unity.modules.jsonserialize` mid-Safe-Mode.
- `StrikeDirector.Active` / `OnResolved` missing → add the property and method on the callee.
- `CityFactory.Strip` missing → add `Strip()`, do not rewrite every road call.
- `FindObjectOfType` obsolete → `FindFirstObjectByType`.

## Protocol

1. Group Console errors by missing type vs missing method.
2. Patch the callee with the smallest method that matches the caller.
3. Exit Safe Mode. Confirm 0 red.
4. Then Play paused. Never Play from Safe Mode.

DirMon yellows under Hub `Editor\\Data\\Resources\\PackageManager` are Hub file locks. Ignore them.
