using UnityEngine;

namespace Bastion
{
    public sealed class LobbyProps : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<LobbyProps>()) return;
            new GameObject("LobbyProps").AddComponent<LobbyProps>();
        }

        void Start() { Invoke(nameof(Dress), 0.45f); }

        void Dress()
        {
            var hq = GameObject.Find("HQ");
            if (!hq || hq.transform.Find("Radio")) return;
            var t = hq.transform;
            var wood = BastionArt.Wood();
            var metal = BastionArt.Metal();
            var amber = BastionGfx.Mat(new Color(0.72f, 0.42f, 0.14f), 0.35f);
            var slate = BastionGfx.Mat(new Color(0.16f, 0.18f, 0.16f), 0.15f);

            Box(t, "ChairA", new Vector3(-1.4f, 0.45f, -0.6f), new Vector3(0.55f, 0.9f, 0.55f), wood);
            Box(t, "ChairB", new Vector3(1.4f, 0.45f, -0.6f), new Vector3(0.55f, 0.9f, 0.55f), wood);
            Box(t, "Radio", new Vector3(0.7f, 0.95f, -1.5f), new Vector3(0.5f, 0.28f, 0.35f), metal);
            Box(t, "Slate", new Vector3(-2.6f, 1.6f, 0.2f), new Vector3(0.08f, 1.4f, 2.2f), slate);
            Box(t, "Crate", new Vector3(2.4f, 0.35f, 1.2f), new Vector3(0.7f, 0.7f, 0.7f), wood);

            var table = t.Find("MapTable") ?? GameObject.Find("MapTable")?.transform;
            if (table)
            {
                var r = table.GetComponent<Renderer>();
                if (r) r.sharedMaterial = amber;
            }
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
