---
name: unity-runtime-art
description: Build readable Unity look with primitives, runtime Texture2D, and BastionNoise when there is no FBX importer. Use when the city looks like boxes, tents vanish, materials go pink, or the user wants facades without packages.
metadata:
  author: bastion-haven
  short-description: Procedural look without the importer
---

# Unity Runtime Art

Pink is a shader miss. Flat is a missing map. Dead Play is a compile miss. Fix in that order.

## Meshes

Cubes and cylinders. Roof slab. Door recess. Window slits. One emissive accent. Role silhouette on people (coat/visor vs scarf vs plates) plus a blob shadow.

Parent props in **local space**. World-space children of a moved parent land at the origin.

Hull colliders on enterable buildings are triggers. Keep a floor. South-face door.

City-cam VFX: scale ≥ 1.4, y ≥ 2.5, life ≥ 1.2s, Point light range 8+.

## Textures

Prefer `Texture2D` painted at runtime with `BastionNoise` (Value, Perlin, Fbm, Ridged, Turbulence, Warp).
StreamingAssets jpgs are optional. Missing jpg must fall back to a baked noise texture, not magenta.

Shader find order: URP Lit → URP Simple Lit → Standard → Unlit/Color → Sprites/Default.

## Density

Empty lots need stalls, fences, wrecks, crate stacks, signs. A 5x5 grid of pads is not a city.
