using UnityEngine;

namespace Bastion
{
    public sealed class CityLife : MonoBehaviour
    {
        static readonly string[] ExtraCiv =
        {
            "Lena Croft", "Pik Sol", "Edda Wren", "Moss Vale", "Jori Kade",
            "Ansel Pitt", "Vee Holm", "Tarn Beck", "Noll Glass", "Ivy Penn"
        };

        Transform _root;
        bool _done;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<CityLife>()) return;
            new GameObject("CityLife").AddComponent<CityLife>();
        }

        void Start() { Invoke(nameof(Build), 0.55f); }

        void Build()
        {
            if (_done) return;
            _done = true;
            _root = GameObject.Find("Haven")?.transform ?? GameObject.Find("CITY")?.transform;
            if (_root == null) _root = new GameObject("HavenLife").transform;

            Market(new Vector3(14f, 0f, -14f));
            Workshop(new Vector3(-28f, 0f, 0f));
            Clinic(new Vector3(28f, 0f, 14f));
            Silo(new Vector3(-14f, 0f, -28f));
            Ruin(new Vector3(28f, 0f, -28f));
            Ruin(new Vector3(-28f, 0f, 28f));
            WatchPost(new Vector3(0f, 0f, 28f));
            FillFarm();
            Crowd();
        }

        void Market(Vector3 p)
        {
            var root = Node("Market", p);
            Box(root, "Awning", new Vector3(0f, 2.2f, 0f), new Vector3(8.4f, 0.12f, 4.2f), BastionArt.Canvas());
            Box(root, "PostA", new Vector3(-4f, 1.1f, -2f), new Vector3(0.16f, 2.2f, 0.16f), BastionArt.Wood());
            Box(root, "PostB", new Vector3(4f, 1.1f, -2f), new Vector3(0.16f, 2.2f, 0.16f), BastionArt.Wood());
            Box(root, "PostC", new Vector3(-4f, 1.1f, 2f), new Vector3(0.16f, 2.2f, 0.16f), BastionArt.Wood());
            Box(root, "PostD", new Vector3(4f, 1.1f, 2f), new Vector3(0.16f, 2.2f, 0.16f), BastionArt.Wood());
            Box(root, "Counter", new Vector3(0f, 0.55f, 0f), new Vector3(6.4f, 0.9f, 1.4f), BastionArt.Wood());
            Box(root, "CrateL", new Vector3(-2.2f, 0.35f, 1.4f), new Vector3(0.8f, 0.7f, 0.8f), BastionArt.Wood());
            Box(root, "CrateR", new Vector3(2.2f, 0.35f, 1.4f), new Vector3(0.8f, 0.7f, 0.8f), BastionArt.Wood());
            Sign(root, "MARKET", new Vector3(0f, 2.6f, -2.1f));
            Civ(p + new Vector3(-1.4f, 0f, 2.2f), 20);
            Civ(p + new Vector3(1.6f, 0f, 2.4f), 21);
            Civ(p + new Vector3(0f, 0f, -2.6f), 22);
        }

        void Workshop(Vector3 p)
        {
            var root = Node("Workshop", p);
            Box(root, "Hall", new Vector3(0f, 1.8f, 0f), new Vector3(6.2f, 3.6f, 5.2f), BastionArt.Metal());
            Box(root, "Roof", new Vector3(0f, 3.7f, 0f), new Vector3(6.6f, 0.18f, 5.6f), BastionArt.Roof());
            Box(root, "Door", new Vector3(0f, 1.0f, 2.7f), new Vector3(1.4f, 2.0f, 0.12f), BastionArt.Wood());
            Box(root, "Stack", new Vector3(-1.6f, 4.6f, -1.2f), new Vector3(0.5f, 1.6f, 0.5f), BastionArt.Rust());
            Box(root, "Bench", new Vector3(1.4f, 0.5f, 1.2f), new Vector3(2.2f, 0.8f, 0.8f), BastionArt.Wood());
            Sign(root, "WORKS", new Vector3(0f, 3.0f, 2.75f));
            Civ(p + new Vector3(2.6f, 0f, 3.2f), 23);
        }

        void Clinic(Vector3 p)
        {
            var root = Node("Clinic", p);
            Box(root, "Hall", new Vector3(0f, 1.7f, 0f), new Vector3(5.4f, 3.4f, 4.8f), BastionGfx.Mat(new Color(0.62f, 0.58f, 0.52f)));
            Box(root, "Cross", new Vector3(0f, 2.6f, 2.45f), new Vector3(0.7f, 0.18f, 0.08f), BastionGfx.Mat(new Color(0.72f, 0.16f, 0.14f), 0.4f));
            Box(root, "CrossV", new Vector3(0f, 2.6f, 2.45f), new Vector3(0.18f, 0.7f, 0.08f), BastionGfx.Mat(new Color(0.72f, 0.16f, 0.14f), 0.4f));
            Box(root, "Door", new Vector3(0f, 0.95f, 2.45f), new Vector3(1.2f, 1.9f, 0.1f), BastionArt.Wood());
            Sign(root, "CLINIC", new Vector3(0f, 3.4f, 2.5f));
            Civ(p + new Vector3(-2.2f, 0f, 3.0f), 24);
        }

        void Silo(Vector3 p)
        {
            var root = Node("GrainSilo", p);
            Cyl(root, "Drum", new Vector3(0f, 3.2f, 0f), 2.2f, 6.4f, BastionArt.Metal());
            Cyl(root, "Cap", new Vector3(0f, 6.5f, 0f), 2.4f, 0.4f, BastionArt.Rust());
            Box(root, "Pipe", new Vector3(1.6f, 1.2f, 0f), new Vector3(1.8f, 0.22f, 0.22f), BastionArt.Metal());
            Sign(root, "SILO", new Vector3(0f, 1.4f, 2.4f));
        }

        void Ruin(Vector3 p)
        {
            var root = Node("Ruin", p);
            Box(root, "WallA", new Vector3(-1.2f, 1.1f, 0f), new Vector3(0.3f, 2.2f, 3.6f), BastionArt.Brick());
            Box(root, "WallB", new Vector3(1.4f, 0.7f, -0.6f), new Vector3(2.4f, 1.4f, 0.3f), BastionArt.Brick());
            Box(root, "Rubble", new Vector3(0.2f, 0.25f, 0.8f), new Vector3(1.6f, 0.5f, 1.2f), BastionArt.Dirt());
        }

        void WatchPost(Vector3 p)
        {
            var root = Node("NorthWatch", p);
            Box(root, "Deck", new Vector3(0f, 3.4f, 0f), new Vector3(2.4f, 0.16f, 2.4f), BastionArt.Wood());
            Box(root, "Pole", new Vector3(0f, 1.7f, 0f), new Vector3(0.22f, 3.4f, 0.22f), BastionArt.Metal());
            Box(root, "Rail", new Vector3(0f, 3.7f, 1.1f), new Vector3(2.2f, 0.12f, 0.12f), BastionArt.Wood());
        }

        void FillFarm()
        {
            var farm = GameObject.Find("Farm") ?? GameObject.Find("GreenTongue") ?? GameObject.Find("FarmBeds");
            if (!farm || farm.transform.Find("Bed0")) return;
            for (int i = 0; i < 6; i++)
            {
                int row = i / 3;
                int col = i % 3;
                Box(farm.transform, "Bed" + i,
                    new Vector3((col - 1) * 2.1f, 0.12f, (row - 0.5f) * 2.4f),
                    new Vector3(1.8f, 0.18f, 1.6f), BastionArt.Dirt());
                Box(farm.transform, "Crop" + i,
                    new Vector3((col - 1) * 2.1f, 0.38f, (row - 0.5f) * 2.4f),
                    new Vector3(1.5f, 0.32f, 1.3f), BastionArt.Glass());
            }
        }

        void Crowd()
        {
            var spots = new Vector3[]
            {
                new Vector3(6f, 0f, 6f), new Vector3(-6f, 0f, 8f), new Vector3(8f, 0f, -8f),
                new Vector3(-10f, 0f, -4f), new Vector3(18f, 0f, 4f), new Vector3(-18f, 0f, -6f),
                new Vector3(4f, 0f, 18f), new Vector3(-4f, 0f, -18f)
            };
            for (int i = 0; i < spots.Length; i++)
                Civ(spots[i], 30 + i);
        }

        void Civ(Vector3 pos, int idx)
        {
            var p = PersonActor.Spawn(_root, Role.Civilian, pos, idx);
            p.DisplayName = ExtraCiv[idx % ExtraCiv.Length];
            p.Job = Job.Work;
        }

        Transform Node(string n, Vector3 p)
        {
            var t = new GameObject(n).transform;
            t.SetParent(_root, false);
            t.position = p;
            return t;
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

        static void Cyl(Transform parent, string n, Vector3 local, float r, float h, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = n;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = local;
            go.transform.localScale = new Vector3(r * 2f, h * 0.5f, r * 2f);
            go.GetComponent<Renderer>().sharedMaterial = mat;
            Object.Destroy(go.GetComponent<Collider>());
        }

        static void Sign(Transform parent, string label, Vector3 local)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Sign:" + label;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = local;
            go.transform.localScale = new Vector3(2.2f, 0.4f, 0.08f);
            go.GetComponent<Renderer>().sharedMaterial = BastionArt.Wood();
            Object.Destroy(go.GetComponent<Collider>());
        }
    }
}
