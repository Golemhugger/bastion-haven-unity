using System.Reflection;
using UnityEngine;

namespace Bastion
{
    public sealed class FarmGate : MonoBehaviour
    {
        object _sim;
        bool _planted;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<FarmGate>()) return;
            new GameObject("FarmGate").AddComponent<FarmGate>();
        }

        void OnGUI()
        {
            Bind();
            if (_sim == null) return;
            bool queued = Flag("CisternQueued");
            bool built = Flag("CisternBuilt");
            bool farmQ = Flag("FarmQueued");
            bool farmB = Flag("FarmBuilt");
            if (!(queued || built) || farmQ || farmB) return;
            if (GUI.Button(new Rect(18f, 430f, 220f, 28f), "Raise farm (16)"))
                QueueFarm();
        }

        void Bind()
        {
            if (_sim != null) return;
            var mbs = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            for (int i = 0; i < mbs.Length; i++)
            {
                var mb = mbs[i];
                if (mb == null) continue;
                var f = mb.GetType().GetField("Sim") ?? mb.GetType().GetField("sim");
                if (f == null) continue;
                _sim = f.GetValue(mb);
                if (_sim != null) return;
            }
        }

        bool Flag(string name)
        {
            if (_sim == null) return false;
            var f = _sim.GetType().GetField(name);
            if (f == null) return false;
            return f.GetValue(_sim) is bool b && b;
        }

        void QueueFarm()
        {
            var t = _sim.GetType();
            var m = t.GetMethod("QueueFarm") ?? t.GetMethod("RaiseFarm");
            if (m != null) m.Invoke(_sim, null);
            else
            {
                var scrap = t.GetField("Scrap");
                if (scrap != null && scrap.GetValue(_sim) is float s && s >= 16f)
                {
                    scrap.SetValue(_sim, s - 16f);
                    var q = t.GetField("FarmQueued") ?? t.GetField("FarmBuilt");
                    if (q != null) q.SetValue(_sim, true);
                    var toast = t.GetMethod("ToastNow");
                    toast?.Invoke(_sim, new object[] { "Crew turning Green Tongue." });
                }
            }
            if (!_planted)
            {
                Plant();
                _planted = true;
            }
        }

        static void Plant()
        {
            var lot = GameObject.Find("Green Tongue") ?? GameObject.Find("GreenTongue") ?? GameObject.Find("Farm");
            var parent = lot ? lot.transform : new GameObject("Green Tongue").transform;
            if (!lot) parent.position = new Vector3(-14f, 0f, 0f);
            if (parent.Find("Seed0")) return;
            var dirt = BastionArt.Dirt();
            var crop = BastionGfx.Mat(new Color(0.22f, 0.46f, 0.18f), 0.12f);
            for (int i = 0; i < 6; i++)
            {
                int row = i / 3;
                int col = i % 3;
                var local = new Vector3((col - 1) * 2.2f, 0.18f, (row - 0.4f) * 2.5f);
                Cube(parent, "Seed" + i, local, new Vector3(2.0f, 0.22f, 1.8f), dirt);
                Cube(parent, "SeedCrop" + i, local + Vector3.up * 0.32f, new Vector3(1.6f, 0.55f, 1.4f), crop);
            }
        }

        static void Cube(Transform parent, string n, Vector3 local, Vector3 scale, Material mat)
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
