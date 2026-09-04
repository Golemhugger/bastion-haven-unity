using System.Reflection;
using UnityEngine;

namespace Bastion
{
    public sealed class DoctrineLook : MonoBehaviour
    {
        Transform _pax, _iron;
        Doctrine _shown;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<DoctrineLook>()) return;
            new GameObject("DoctrineLook").AddComponent<DoctrineLook>();
        }

        void Start() { Invoke(nameof(Build), 0.8f); }

        void Build()
        {
            var hq = GameObject.Find("HQ");
            var parent = hq ? hq.transform : transform;
            _pax = Flag(parent, "FlagPax", new Color(0.35f, 0.55f, 0.62f), -1.6f);
            _iron = Flag(parent, "FlagIron", new Color(0.42f, 0.16f, 0.12f), 1.6f);
            _pax.gameObject.SetActive(false);
            _iron.gameObject.SetActive(false);
        }

        void Update()
        {
            if (_pax == null) return;
            var d = ReadDoctrine();
            if (d == _shown) return;
            _shown = d;
            _pax.gameObject.SetActive(d == Doctrine.PaxHaven);
            _iron.gameObject.SetActive(d == Doctrine.IronHaven);
            if (d == Doctrine.PaxHaven) Lanterns();
            if (d == Doctrine.IronHaven) Barricades();
        }

        static Doctrine ReadDoctrine()
        {
            var mbs = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            for (int i = 0; i < mbs.Length; i++)
            {
                var mb = mbs[i];
                if (mb == null) continue;
                var t = mb.GetType();
                var f = t.GetField("Tech") ?? t.GetField("tech") ?? t.GetField("_tech");
                if (f == null) continue;
                var tree = f.GetValue(mb) as TechTree;
                if (tree != null) return tree.Doctrine;
            }
            return Doctrine.None;
        }

        static Transform Flag(Transform parent, string n, Color c, float x)
        {
            var pole = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pole.name = n;
            pole.transform.SetParent(parent, false);
            pole.transform.localPosition = new Vector3(x, 5.4f, -2.2f);
            pole.transform.localScale = new Vector3(0.08f, 4.2f, 0.08f);
            pole.GetComponent<Renderer>().sharedMaterial = BastionArt.Metal();
            Object.Destroy(pole.GetComponent<Collider>());
            var cloth = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cloth.name = n + "Cloth";
            cloth.transform.SetParent(pole.transform, false);
            cloth.transform.localPosition = new Vector3(4f, 0.35f, 0f);
            cloth.transform.localScale = new Vector3(8f, 0.35f, 0.08f);
            cloth.GetComponent<Renderer>().sharedMaterial = BastionGfx.Mat(c, 0.25f);
            Object.Destroy(cloth.GetComponent<Collider>());
            return pole.transform;
        }

        static void Lanterns()
        {
            if (GameObject.Find("PaxLantern0")) return;
            var root = GameObject.Find("Haven")?.transform ?? GameObject.Find("HQ")?.transform;
            if (!root) return;
            for (int i = 0; i < 6; i++)
            {
                float a = i * Mathf.PI * 2f / 6f;
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.name = "PaxLantern" + i;
                go.transform.position = new Vector3(Mathf.Cos(a) * 8f, 2.2f, Mathf.Sin(a) * 8f);
                go.transform.localScale = new Vector3(0.25f, 0.35f, 0.25f);
                go.GetComponent<Renderer>().sharedMaterial = BastionGfx.Mat(new Color(1f, 0.72f, 0.38f), 0.5f);
                Object.Destroy(go.GetComponent<Collider>());
                var light = go.AddComponent<Light>();
                light.type = LightType.Point;
                light.color = new Color(1f, 0.75f, 0.4f);
                light.range = 6f;
                light.intensity = 1.4f;
            }
        }

        static void Barricades()
        {
            if (GameObject.Find("IronBar0")) return;
            for (int i = 0; i < 4; i++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.name = "IronBar" + i;
                go.transform.position = new Vector3((i - 1.5f) * 3.2f, 0.55f, -6.4f);
                go.transform.localScale = new Vector3(2.6f, 1.1f, 0.28f);
                go.GetComponent<Renderer>().sharedMaterial = BastionArt.Rust();
                Object.Destroy(go.GetComponent<Collider>());
            }
        }
    }
}
