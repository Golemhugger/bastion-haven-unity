using UnityEngine;

namespace Bastion
{
    public sealed class DoorCut : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<DoorCut>()) return;
            new GameObject("DoorCut").AddComponent<DoorCut>();
        }

        void Start() { Invoke(nameof(Cut), 0.35f); }

        void Cut()
        {
            var hq = GameObject.Find("HQ");
            if (!hq || hq.transform.Find("HQLamp")) return;

            var hull = hq.GetComponent<Renderer>();
            if (hull) hull.enabled = false;
            var hullCol = hq.GetComponent<Collider>();
            if (hullCol) hullCol.isTrigger = true;

            var lobby = GameObject.Find("HQLobby");
            if (lobby)
            {
                var lr = lobby.GetComponent<Renderer>();
                if (lr) lr.enabled = false;
                var lc = lobby.GetComponent<Collider>();
                if (lc) lc.isTrigger = true;
            }

            var steel = BastionArt.Metal();
            var wood = BastionArt.Wood();
            var warm = BastionGfx.Mat(new Color(0.85f, 0.48f, 0.16f), 1.6f);
            var floorMat = BastionArt.Dirt();

            Part(hq.transform, "LobbyFloor", new Vector3(0f, 0.08f, 0f), new Vector3(7.2f, 0.14f, 7.2f), floorMat, false);
            Part(hq.transform, "WallN", new Vector3(0f, 1.6f, 3.3f), new Vector3(7.0f, 3.2f, 0.28f), steel, false);
            Part(hq.transform, "WallE", new Vector3(3.3f, 1.6f, 0f), new Vector3(0.28f, 3.2f, 6.6f), steel, false);
            Part(hq.transform, "WallW", new Vector3(-3.3f, 1.6f, 0f), new Vector3(0.28f, 3.2f, 6.6f), steel, false);
            Part(hq.transform, "RoofSlab", new Vector3(0f, 3.3f, 0.2f), new Vector3(7.2f, 0.18f, 6.8f), steel, false);
            Part(hq.transform, "JambL", new Vector3(-1.15f, 1.2f, -3.35f), new Vector3(0.28f, 2.4f, 0.28f), wood, false);
            Part(hq.transform, "JambR", new Vector3(1.15f, 1.2f, -3.35f), new Vector3(0.28f, 2.4f, 0.28f), wood, false);
            Part(hq.transform, "MapTable", new Vector3(0f, 0.72f, -1.6f), new Vector3(2.2f, 0.18f, 1.3f), warm, false);

            var lamp = new GameObject("HQLamp");
            lamp.transform.SetParent(hq.transform, false);
            lamp.transform.localPosition = new Vector3(0f, 2.4f, -1.4f);
            var light = lamp.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(1f, 0.72f, 0.38f);
            light.range = 8f;
            light.intensity = 3.2f;
            Part(hq.transform, "LampShade", new Vector3(0f, 2.45f, -1.4f), new Vector3(0.35f, 0.18f, 0.35f), warm, false);
        }

        static void Part(Transform parent, string n, Vector3 local, Vector3 scale, Material mat, bool collide)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = n;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = local;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            var col = go.GetComponent<Collider>();
            if (!collide) Object.Destroy(col);
            else col.isTrigger = true;
        }
    }
}
