using UnityEngine;

namespace Bastion
{
    public static class BastionGfx
    {
        static Shader _lit;

        public static Material Mat(Color c, float emit = 0f, float smooth = 0.35f)
        {
            if (_lit == null)
            {
                _lit = Shader.Find("Universal Render Pipeline/Lit")
                       ?? Shader.Find("Universal Render Pipeline/Simple Lit")
                       ?? Shader.Find("Standard")
                       ?? Shader.Find("Unlit/Color")
                       ?? Shader.Find("Sprites/Default");
            }
            var m = new Material(_lit);
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
            if (m.HasProperty("_Color")) m.SetColor("_Color", c);
            m.color = c;
            if (emit > 0f && m.HasProperty("_EmissionColor"))
            {
                m.EnableKeyword("_EMISSION");
                m.SetColor("_EmissionColor", c * emit);
            }
            if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smooth);
            if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", 0.12f);
            return m;
        }
    }

    public sealed class CityFactory
    {
        public const float Cell = 14f;
        public const int Grid = 5;
        public Transform Root;
        public Transform WireStreetPad;
        public Transform AshRow;
        public Transform CampWest;
        public Transform HqDoor;

        static readonly string[] District =
        {
            "Rook End","Kiln","Wire Street","Ash Row","West Gate",
            "Silo Reach","Old Market","HQ Plaza","North Cut","Scrapyard",
            "Low Well","Green Tongue","Cistern Lot","Barracks Yard","Watch",
            "Hearth","Lantern","South Ditch","Rail Spine","Camp Approach",
            "Ruins","Blight","Far Hydro","East Wall","Camp West"
        };

        public void Build(Transform parent)
        {
            Root = parent;
            var ground = BastionGfx.Mat(new Color(0.10f, 0.09f, 0.08f), 0f, 0.22f);
            var road = BastionGfx.Mat(new Color(0.09f, 0.09f, 0.11f), 0f, 0.55f);
            var dash = BastionGfx.Mat(new Color(0.72f, 0.58f, 0.28f));
            var lotOwned = BastionGfx.Mat(new Color(0.22f, 0.23f, 0.26f));
            var lotRuin = BastionGfx.Mat(new Color(0.16f, 0.13f, 0.11f));

            float extent = Grid * Cell * 0.5f + 18f;
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Ground";
            floor.transform.SetParent(parent);
            floor.transform.localPosition = new Vector3(0f, -0.08f, 0f);
            floor.transform.localScale = new Vector3(extent * 2f, 0.16f, extent * 2f);
            floor.GetComponent<Renderer>().sharedMaterial = ground;

            int i = 0;
            for (int z = 0; z < Grid; z++)
            for (int x = 0; x < Grid; x++)
            {
                float wx = (x - Grid * 0.5f + 0.5f) * Cell;
                float wz = (z - Grid * 0.5f + 0.5f) * Cell;
                var lot = GameObject.CreatePrimitive(PrimitiveType.Cube);
                lot.name = District[i];
                lot.transform.SetParent(parent);
                lot.transform.localPosition = new Vector3(wx, 0.02f, wz);
                lot.transform.localScale = new Vector3(Cell - 4.2f, 0.04f, Cell - 4.2f);
                lot.GetComponent<Renderer>().sharedMaterial = (x == 0 && z == 4) ? lotRuin : lotOwned;
                i++;
            }

            BuildRoads(parent, road, dash);
            BuildHq(parent);
            BuildHydro(parent, -Cell, 0f);
            BuildHydro(parent, Cell, Cell);
            BuildHab(parent, -Cell, Cell);
            BuildHab(parent, Cell, -Cell);
            BuildHab(parent, -2f * Cell, -Cell);
            BuildBarracks(parent, 0f, Cell);
            BuildWatch(parent, Cell, 0f);
            WireStreetPad = BuildCisternPad(parent, 0f, -Cell);
            AshRow = Marker(parent, "Ash Row", -Cell, Cell * 2f * 0f + Cell);
            // Ash Row is north-west hab block — place marker on that lot
            AshRow.position = new Vector3(-Cell, 0.1f, Cell);
            CampWest = BuildCamp(parent, -Cell * 2.6f, Cell * 2.1f);
            Lamps(parent);
            Clutter(parent);
        }

        void BuildRoads(Transform parent, Material road, Material dash)
        {
            for (int i = -2; i <= 2; i++)
            {
                Strip(parent, new Vector3(i * Cell, 0.03f, 0f), new Vector3(3.2f, 0.05f, Grid * Cell + 8f), road);
                Strip(parent, new Vector3(0f, 0.03f, i * Cell), new Vector3(Grid * Cell + 8f, 0.05f, 3.2f), road);
            }
            for (int k = -16; k <= 16; k += 2)
            {
                Strip(parent, new Vector3(k, 0.06f, 0f), new Vector3(0.9f, 0.02f, 0.18f), dash);
                Strip(parent, new Vector3(0f, 0.06f, k), new Vector3(0.18f, 0.02f, 0.9f), dash);
            }
        }

        void BuildHq(Transform parent)
        {
            var steel = BastionGfx.Mat(new Color(0.32f, 0.36f, 0.38f));
            var dark = BastionGfx.Mat(new Color(0.12f, 0.14f, 0.16f));
            var emit = BastionGfx.Mat(new Color(1f, 0.72f, 0.38f), 2.2f);
            Cyl(parent, "HQ", Vector3.up * 4.6f, 2.6f, 9.2f, steel);
            Cyl(parent, "HQRing", new Vector3(0f, 6.1f, 0f), 3.1f, 0.18f, dark);
            Box(parent, "Antenna", new Vector3(0.4f, 10.4f, 0.2f), new Vector3(0.12f, 4.2f, 0.12f), dark);
            Box(parent, "HQDoor", new Vector3(0f, 1.1f, 2.55f), new Vector3(1.3f, 2.2f, 0.16f), emit);
            HqDoor = parent.Find("HQDoor");
            // lobby volume
            var lobby = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lobby.name = "HQLobby";
            lobby.transform.SetParent(parent);
            lobby.transform.localPosition = new Vector3(0f, 1.3f, 0.4f);
            lobby.transform.localScale = new Vector3(4.6f, 2.6f, 4.2f);
            var lr = lobby.GetComponent<Renderer>();
            var lm = BastionGfx.Mat(new Color(0.18f, 0.16f, 0.14f));
            lr.sharedMaterial = lm;
            lobby.GetComponent<Collider>().isTrigger = true;
            Box(parent, "MapTable", new Vector3(0f, 0.7f, 0.2f), new Vector3(1.8f, 0.16f, 1.1f), emit);
        }

        void BuildHydro(Transform parent, float x, float z)
        {
            var glass = BastionGfx.Mat(new Color(0.18f, 0.38f, 0.28f, 0.45f));
            var crop = BastionGfx.Mat(new Color(0.22f, 0.48f, 0.22f));
            var frame = BastionGfx.Mat(new Color(0.2f, 0.22f, 0.2f));
            Box(parent, "Hydro", new Vector3(x, 1.6f, z), new Vector3(6.4f, 3.2f, 4.6f), glass);
            Box(parent, "HydroFrame", new Vector3(x, 3.2f, z), new Vector3(6.6f, 0.12f, 4.8f), frame);
            for (int i = -2; i <= 2; i++)
                Box(parent, "Crop", new Vector3(x + i * 0.9f, 0.28f, z), new Vector3(0.18f, 0.5f, 3.2f), crop);
        }

        void BuildHab(Transform parent, float x, float z)
        {
            var brick = BastionGfx.Mat(new Color(0.38f, 0.32f, 0.26f));
            var win = BastionGfx.Mat(new Color(1f, 0.72f, 0.42f), 1.6f);
            Box(parent, "Hab", new Vector3(x, 2.2f, z), new Vector3(5.2f, 4.4f, 4.4f), brick);
            for (int u = -1; u <= 1; u++)
            for (int v = 0; v < 3; v++)
                Box(parent, "Win", new Vector3(x + u * 1.2f, 1.1f + v * 1.1f, z + 2.22f), new Vector3(0.7f, 0.55f, 0.08f), win);
            Box(parent, "HabDoor", new Vector3(x, 0.95f, z + 2.28f), new Vector3(1.1f, 1.9f, 0.1f), BastionGfx.Mat(new Color(0.12f, 0.1f, 0.09f)));
        }

        void BuildBarracks(Transform parent, float x, float z)
        {
            var teal = BastionGfx.Mat(new Color(0.22f, 0.32f, 0.32f));
            Box(parent, "Barracks", new Vector3(x, 2.0f, z), new Vector3(8.4f, 4.0f, 4.2f), teal);
            Box(parent, "Rack", new Vector3(x - 2.4f, 0.9f, z + 1.6f), new Vector3(2.2f, 1.4f, 0.3f), BastionGfx.Mat(new Color(0.15f, 0.15f, 0.16f)));
        }

        void BuildWatch(Transform parent, float x, float z)
        {
            var steel = BastionGfx.Mat(new Color(0.28f, 0.3f, 0.32f));
            Cyl(parent, "WatchPole", new Vector3(x, 3.1f, z), 0.22f, 6.2f, steel);
            Box(parent, "WatchCab", new Vector3(x, 6.4f, z), new Vector3(2.0f, 1.3f, 2.0f), steel);
        }

        Transform BuildCisternPad(Transform parent, float x, float z)
        {
            var pad = Box(parent, "WireStreetPad", new Vector3(x, 0.06f, z), new Vector3(6.2f, 0.08f, 6.2f),
                BastionGfx.Mat(new Color(0.15f, 0.28f, 0.32f), 0.4f));
            return pad;
        }

        public void FinishCistern(Transform parent)
        {
            var tank = BastionGfx.Mat(new Color(0.28f, 0.42f, 0.48f));
            var p = WireStreetPad.position;
            Cyl(parent, "TankA", p + new Vector3(-1.4f, 1.7f, 0f), 1.15f, 3.4f, tank);
            Cyl(parent, "TankB", p + new Vector3(1.4f, 1.7f, 0f), 1.15f, 3.4f, tank);
            Box(parent, "Pipe", p + new Vector3(0f, 0.4f, 1.6f), new Vector3(3.2f, 0.16f, 0.16f), tank);
        }

        public void Scaffold(Transform parent, bool on)
        {
            var old = parent.Find("Scaffold");
            if (old) Object.Destroy(old.gameObject);
            if (!on || WireStreetPad == null) return;
            var rust = BastionGfx.Mat(new Color(0.45f, 0.32f, 0.16f));
            var g = new GameObject("Scaffold");
            g.transform.SetParent(parent);
            var p = WireStreetPad.position;
            for (int i = 0; i < 4; i++)
            {
                float sx = (i % 2 == 0) ? -2.2f : 2.2f;
                float sz = (i < 2) ? -2.2f : 2.2f;
                Box(g.transform, "Post", p + new Vector3(sx, 1.6f, sz), new Vector3(0.12f, 3.2f, 0.12f), rust);
            }
            Box(g.transform, "Plank", p + new Vector3(0f, 2.4f, 0f), new Vector3(4.6f, 0.08f, 0.3f), rust);
        }

        Transform BuildCamp(Transform parent, float x, float z)
        {
            var rust = BastionGfx.Mat(new Color(0.38f, 0.2f, 0.14f));
            var fire = BastionGfx.Mat(new Color(1f, 0.4f, 0.12f), 3.5f);
            var root = new GameObject("CampWest").transform;
            root.SetParent(parent);
            root.position = new Vector3(x, 0f, z);
            Box(root, "TentA", new Vector3(-1.6f, 1.1f, 0.8f), new Vector3(2.4f, 2.0f, 2.0f), rust);
            Box(root, "TentB", new Vector3(1.8f, 1.0f, -0.6f), new Vector3(2.2f, 1.8f, 1.8f), rust);
            Box(root, "TentC", new Vector3(0.2f, 0.9f, 2.1f), new Vector3(2.0f, 1.6f, 1.6f), rust);
            Cyl(root, "Fire", new Vector3(0f, 0.35f, 0f), 0.45f, 0.5f, fire);
            return root;
        }

        void Lamps(Transform parent)
        {
            var pole = BastionGfx.Mat(new Color(0.12f, 0.12f, 0.13f));
            var bulb = BastionGfx.Mat(new Color(1f, 0.72f, 0.38f), 3.2f);
            for (int x = -2; x <= 2; x++)
            for (int z = -2; z <= 2; z++)
            {
                if ((x + z) % 2 != 0) continue;
                var p = new Vector3(x * Cell * 0.5f, 0f, z * Cell * 0.5f);
                Box(parent, "LampPole", p + Vector3.up * 1.6f, new Vector3(0.08f, 3.2f, 0.08f), pole);
                var b = Cyl(parent, "LampBulb", p + Vector3.up * 3.25f, 0.16f, 0.22f, bulb);
                b.gameObject.AddComponent<LampFlicker>();
            }
        }

        void Clutter(Transform parent)
        {
            var wood = BastionGfx.Mat(new Color(0.32f, 0.24f, 0.16f));
            var rust = BastionGfx.Mat(new Color(0.3f, 0.18f, 0.12f));
            var rng = new System.Random(11);
            for (int i = 0; i < 18; i++)
            {
                float x = (float)(rng.NextDouble() * 50 - 25);
                float z = (float)(rng.NextDouble() * 50 - 25);
                if (Mathf.Abs(x) < 4 && Mathf.Abs(z) < 4) continue;
                Box(parent, "Crate", new Vector3(x, 0.35f, z), new Vector3(0.7f, 0.7f, 0.7f), wood);
            }
            Box(parent, "Wreck", new Vector3(18f, 0.6f, -11f), new Vector3(3.4f, 1.1f, 1.4f), rust);
            Box(parent, "Barricade", new Vector3(-16f, 0.5f, 8f), new Vector3(3.8f, 1.0f, 0.35f), rust);
        }

        Transform Marker(Transform parent, string n, float x, float z)
        {
            var t = new GameObject(n).transform;
            t.SetParent(parent);
            t.position = new Vector3(x, 0.1f, z);
            return t;
        }

        static Transform Box(Transform parent, string n, Vector3 pos, Vector3 scale, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = n;
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            return go.transform;
        }

        static Transform Cyl(Transform parent, string n, Vector3 pos, float r, float h, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = n;
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = new Vector3(r * 2f, h * 0.5f, r * 2f);
            go.GetComponent<Renderer>().sharedMaterial = mat;
            return go.transform;
        }
    }

    public sealed class LampFlicker : MonoBehaviour
    {
        Material _m;
        float _i;
        void Start()
        {
            var r = GetComponent<Renderer>();
            if (!r) return;
            _m = r.material;
            _i = UnityEngine.Random.Range(0f, 12f);
        }
        void Update()
        {
            if (!_m) return;
            float e = 2.4f + Mathf.Sin(Time.time * 9f + _i) * 0.35f;
            if (UnityEngine.Random.value < 0.002f) e += 1.1f;
            if (_m.HasProperty("_EmissionColor"))
                _m.SetColor("_EmissionColor", new Color(1f, 0.72f, 0.38f) * e);
        }
    }
}
