---
name: unity-compile-contract
description: Keep a multi-agent Unity zip compiling. Use before shipping scripts, when adding a helper another file calls, or when Steward reports Safe Mode after a drop.
metadata:
  author: bastion-haven
  short-description: One-zip compile contract
---

# Unity Compile Contract

A drop is one compile unit. Partial zips are how Safe Mode happens.

## Must-ship together

- If Boot calls it, the zip contains it.
- If Textures calls `BastionNoise.ZeroOne`, Noise in that zip has `ZeroOne`.
- If Art calls `BastionGfx.Mat(color, emit, smooth)`, Gfx has the 3-arg overload.
- If anything calls `LoadImage`, either the manifest has `com.unity.modules.imageconversion` or the loader uses reflection + fallback.

## Never

- Insert a statement between two methods in Boot.
- Add URP / Cinemachine / NavMesh to fix compile.
- Replace GoosePc `GameSim.cs` with a shorter file that drops farm / recruit / toast de-dupe.
- Ship `DistrictFill` and `BastionKit` and `CityLife` all building a clinic.
- `GameObject.Find("GreenTongue")` when the lot is named `"Green Tongue"`.

## Manifest modules we actually needed

```
com.unity.modules.imgui          // OnGUI HUD
com.unity.modules.physics        // colliders
com.unity.modules.audio          // later
com.unity.modules.ui             // ugui optional
com.unity.modules.imageconversion // jpg LoadImage
com.unity.modules.jsonserialize   // JsonUtility — prefer PlayerPrefs instead
```

Add modules only when Unity is closed and the last Play was 0 red.
