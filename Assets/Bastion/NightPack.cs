using UnityEngine;

namespace Bastion
{
    public sealed class NightPack : MonoBehaviour
    {
        static bool PropsDone;
        static readonly string[] Districts =
        {
            "Rook End", "Kiln", "Wire Street", "Ash Row", "West Gate",
            "Silo Reach", "Old Market", "HQ Plaza"
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<NightPack>()) return;
            new GameObject("NightPack").AddComponent<NightPack>();
        }

        void Start() { Invoke(nameof(Dress), 0.25f); }

        void Dress()
        {
            DressCamp();
            DressHydros();
            ScatterProps();
            PlaceSigns();
        }

        void DressCamp()
        {
            var camp = GameObject.Find("CampWest");
            if (!camp || camp.transform.Find("TentBigA")) return;
            Tent(camp.transform, "TentBigA", new Vector3(-2.6f, 1.5f, 1.4f), 22f);
            Tent(camp.transform, "TentBigB", new Vector3(2.8f, 1.4f, -1.0f), -16f);
            Tent(camp.transform, "TentBigC", new Vector3(0.1f, 1.35f, 3.0f), 8f);
            Child(camp.transform, "ScrapWall", new Vector3(0f, 0.75f, -2.8f), new Vector3(6.4f, 1.5f, 0.3f), BastionArt.Metal());
            Child(camp.transform, "BarrelA", new Vector3(3.6f, 0.45f, 1.6f), new Vector3(0.7f, 0.9f, 0.7f), BastionArt.Metal());
            Child(camp.transform, "FlagPole", new Vector3(0f, 2.5f, 0f), new Vector3(0.09f, 5.0f, 0.09f), BastionArt.Metal());
            Child(camp.transform, "Flag", new Vector3(0.7f, 4.6f, 0f), new Vector3(1.3f, 0.6f, 0.07f),
                BastionGfx.Mat(new Color(0.62f, 0.16f, 0.12f), 0.5f));
        }

        void DressHydros()
        {
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None))
            {
                if (t.name != "Hydro" || t.Find("Rib0")) continue;
                for (int i = 0; i < 5; i++)
                    Child(t, "Rib" + i, new Vector3((i - 2) * 1.15f, 1.8f, 0f), new Vector3(0.14f, 0.22f, 5.1f), BastionArt.Metal());
                Child(t, "HydroCap", new Vector3(0f, 2.15f, 0f), new Vector3(6.9f, 0.18f, 5.1f), BastionArt.Glass());
            }
        }

        void ScatterProps()
        {
            if (PropsDone) return;
            PropsDone = true;
            var rng = new System.Random(42);
            for (int i = 0; i < 24; i++)
            {
                float x = (float)(rng.NextDouble() * 56 - 28);
                float z = (float)(rng.NextDouble() * 56 - 28);
                if (Mathf.Abs(x) < 5f && Mathf.Abs(z) < 5f) continue;
                int k = rng.Next(0, 4);
                if (k == 0) World("Stall", new Vector3(x, 1.3f, z), new Vector3(2.4f, 0.08f, 1.6f), BastionArt.Canvas());
                else if (k == 1) World("Fence", new Vector3(x, 0.55f, z), new Vector3(3.4f, 1.1f, 0.12f), BastionArt.Wood());
                else if (k == 2) World("Wreck", new Vector3(x, 0.55f, z), new Vector3(3.0f, 1.0f, 1.3f), BastionArt.Metal());
                else World("CratePile", new Vector3(x, 0.4f, z), new Vector3(0.8f, 0.8f, 0.8f), BastionArt.Wood());
            }
        }

        void PlaceSigns()
        {
            for (int i = 0; i < 8; i++)
            {
                float ang = i * Mathf.PI * 0.25f;
                var p = new Vector3(Mathf.Cos(ang) * 18f, 2.1f, Mathf.Sin(ang) * 18f);
                World("Sign:" + Districts[i], p, new Vector3(1.7f, 0.45f, 0.08f), BastionArt.Wood());
            }
        }

        static void Tent(Transform parent, string n, Vector3 local, float yaw)
        {
            var b = Child(parent, n, local, new Vector3(3.6f, 2.8f, 2.6f), BastionArt.Canvas());
            b.localRotation = Quaternion.Euler(0f, yaw, 0f);
            Child(parent, n + "Ridge", local + Vector3.up * 1.5f, new Vector3(3.8f, 0.18f, 0.22f), BastionArt.Wood());
        }

        static Transform Child(Transform parent, string n, Vector3 local, Vector3 scale, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = n;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = local;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            Object.Destroy(go.GetComponent<Collider>());
            return go.transform;
        }

        static void World(string n, Vector3 pos, Vector3 scale, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = n;
            go.transform.position = pos;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            Object.Destroy(go.GetComponent<Collider>());
        }
    }
}
