using UnityEngine;

namespace Bastion
{
    public sealed class StreetDress : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<StreetDress>()) return;
            new GameObject("StreetDress").AddComponent<StreetDress>();
        }

        void Start() { Invoke(nameof(Dress), 0.6f); }

        void Dress()
        {
            if (GameObject.Find("StreetDressRoot")) return;
            var root = new GameObject("StreetDressRoot").transform;
            var rng = new System.Random(77);
            for (int i = 0; i < 16; i++)
            {
                float x = (float)(rng.NextDouble() * 52 - 26);
                float z = (float)(rng.NextDouble() * 52 - 26);
                if (Mathf.Abs(x) < 6f && Mathf.Abs(z) < 6f) continue;
                int k = rng.Next(0, 6);
                if (k == 0) Shack(root, x, z);
                else if (k == 1) Wreck(root, x, z);
                else if (k == 2) Fence(root, x, z, rng.Next(0, 2) == 0);
                else if (k == 3) Crates(root, x, z, rng.Next(2, 5));
                else if (k == 4) Bench(root, x, z);
                else Poster(root, x, z);
            }
        }

        static void Shack(Transform root, float x, float z)
        {
            Box(root, "Shack", new Vector3(x, 1.1f, z), new Vector3(3.4f, 2.2f, 3.0f), BastionArt.Brick());
            Box(root, "ShackRoof", new Vector3(x, 2.3f, z), new Vector3(3.7f, 0.16f, 3.3f), BastionArt.Roof());
            Box(root, "ShackDoor", new Vector3(x, 0.8f, z + 1.55f), new Vector3(0.9f, 1.6f, 0.08f), BastionArt.Wood());
        }

        static void Wreck(Transform root, float x, float z)
        {
            Box(root, "WreckBody", new Vector3(x, 0.55f, z), new Vector3(3.4f, 1.0f, 1.5f), BastionArt.Rust());
            Box(root, "WreckCab", new Vector3(x - 1.2f, 1.15f, z), new Vector3(1.1f, 0.8f, 1.4f), BastionArt.Metal());
        }

        static void Fence(Transform root, float x, float z, bool ns)
        {
            var s = ns ? new Vector3(0.12f, 1.15f, 4.2f) : new Vector3(4.2f, 1.15f, 0.12f);
            Box(root, "Fence", new Vector3(x, 0.55f, z), s, BastionArt.Wood());
        }

        static void Crates(Transform root, float x, float z, int n)
        {
            for (int i = 0; i < n; i++)
                Box(root, "Crate", new Vector3(x + (i % 2) * 0.75f, 0.32f + i * 0.30f, z + (i / 2) * 0.2f),
                    new Vector3(0.7f, 0.6f, 0.7f), BastionArt.Wood());
        }

        static void Bench(Transform root, float x, float z)
        {
            Box(root, "Bench", new Vector3(x, 0.32f, z), new Vector3(1.8f, 0.18f, 0.45f), BastionArt.Wood());
            Box(root, "BenchLegA", new Vector3(x - 0.7f, 0.16f, z), new Vector3(0.08f, 0.32f, 0.4f), BastionArt.Wood());
            Box(root, "BenchLegB", new Vector3(x + 0.7f, 0.16f, z), new Vector3(0.08f, 0.32f, 0.4f), BastionArt.Wood());
        }

        static void Poster(Transform root, float x, float z)
        {
            Box(root, "PosterPole", new Vector3(x, 1.2f, z), new Vector3(0.08f, 2.4f, 0.08f), BastionArt.Metal());
            Box(root, "Poster", new Vector3(x, 2.1f, z + 0.06f), new Vector3(1.1f, 0.7f, 0.05f), BastionArt.Canvas());
        }

        static void Box(Transform parent, string n, Vector3 pos, Vector3 scale, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = n;
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            Object.Destroy(go.GetComponent<Collider>());
        }
    }
}
