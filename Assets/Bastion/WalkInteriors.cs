using UnityEngine;

namespace Bastion
{
    public sealed class WalkInteriors : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<WalkInteriors>()) return;
            new GameObject("WalkInteriors").AddComponent<WalkInteriors>();
        }

        void Start() { Invoke(nameof(Carve), 0.9f); }

        void Carve()
        {
            Room("Clinic", new Color(0.62f, 0.58f, 0.52f), new Color(0.72f, 0.16f, 0.14f));
            Room("Workshop", BastionArt.Metal().color, new Color(0.45f, 0.32f, 0.16f));
            Room("Barracks", new Color(0.22f, 0.32f, 0.32f), new Color(0.16f, 0.22f, 0.22f));
            foreach (var hab in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None))
            {
                if (hab && hab.name == "Hab") OpenHull(hab.gameObject);
            }
        }

        static void Room(string name, Color wall, Color accent)
        {
            var go = GameObject.Find(name);
            if (!go) return;
            OpenHull(go);
            var t = go.transform;
            if (t.Find("InFloor")) return;
            Box(t, "InFloor", new Vector3(0f, 0.08f, 0f), new Vector3(4.6f, 0.12f, 4.2f), BastionArt.Dirt());
            Box(t, "InTable", new Vector3(0f, 0.62f, 0.2f), new Vector3(1.6f, 0.14f, 0.9f), BastionArt.Wood());
            Box(t, "InLamp", new Vector3(0f, 2.1f, 0f), new Vector3(0.28f, 0.16f, 0.28f), BastionGfx.Mat(new Color(1f, 0.72f, 0.38f), 0.45f));
            Box(t, "InAccent", new Vector3(-1.8f, 1.4f, 0.2f), new Vector3(0.08f, 1.1f, 1.6f), BastionGfx.Mat(accent, 0.2f));
            var light = new GameObject("InLight");
            light.transform.SetParent(t, false);
            light.transform.localPosition = new Vector3(0f, 2.0f, 0f);
            var l = light.AddComponent<Light>();
            l.type = LightType.Point;
            l.color = new Color(1f, 0.75f, 0.45f);
            l.range = 7f;
            l.intensity = 1.6f;
        }

        static void OpenHull(GameObject go)
        {
            var col = go.GetComponent<Collider>();
            if (col) col.isTrigger = true;
        }

        static void Box(Transform parent, string n, Vector3 local, Vector3 scale, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = n;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = local;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            Object.Destroy(go.GetComponent<Collider>());
        }
    }
}
