---
name: unity-runtime-art
description: Readable Unity look with primitives, runtime Texture2D, and noise when there is no FBX. Use when the city is boxes, tents vanish, materials go pink, windows blow out white, or jpgs fail to load.
metadata:
  author: bastion-haven
  short-description: Procedural look without the importer
---

# Unity Runtime Art

Pink is a shader miss. White-hot cubes are emit too high. Flat is a missing map. Dead Play is compile. Fix in that order.

## Meshes

Cubes and cylinders. Roof slab. Door recess. Window slits. One emissive accent ≤ 0.6 or it reads as a white cube from city cam.
People = box silhouette (coat/visor vs scarf vs plates) + blob shadow.

Parent props in **local space** after `SetParent(parent, false)`. World-space children of a moved parent land at the origin — that is why Camp West tents vanished.

Hull colliders on enterable buildings are triggers. Keep a floor. South-face door. Do not hide a map table inside an opaque cylinder.

City-cam VFX: scale ≥ 2, y ≥ 2.5, life ≥ 1.2s, Point light range 8+.

## Textures

Runtime `Texture2D` + `BastionNoise` (Value, Perlin, Fbm, Ridged, ZeroOne, Tint).
StreamingAssets jpgs optional. Missing jpg → noise fallback, never magenta.
`LoadImage` needs `com.unity.modules.imageconversion` or reflection.

Shader find: URP Lit → URP Simple Lit → Standard → Unlit/Color → Sprites/Default.

## Names

`GameObject.Find` is exact. `GreenTongue` ≠ `Green Tongue`. Match CityFactory lot names.

## Density

Empty lots need stalls, fences, wrecks, crates, signs, farm rows. A 5×5 grid of pads is not a city.
