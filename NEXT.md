# Next steps after you add the Unity project

1. Unity Hub → New project → **Universal 3D** (Unity 6).
2. Copy this repo’s `Assets/Bastion` folder into the project `Assets/` folder.
3. Open any scene. Press **Play**. `BastionBoot` attaches itself.
4. Do not bake NavMesh yet. People steer. Do not import Cinemachine yet.
5. After Play works: Window → Rendering → Lighting → generate lighting is optional. Dusk is set in code.

## What this pass adds

- Walkable interiors (HQ lobby, hab, barracks)
- Camp West as a scene (column, flashes, raiders drop)
- Tech / doctrine (Pax Haven vs Iron Haven)
- F5 save / F9 load (`persistentDataPath/bastion-save.json`)
- Command HUD + first-two-minutes beats

## After Play is confirmed

Paint facades, swap primitive people for FBX, add Cinemachine virtual cameras, bake NavMesh on the ground cube, add an Audio Mixer. Still no gacha.
