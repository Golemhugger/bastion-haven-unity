using UnityEngine;

namespace Bastion
{
    public sealed class CropGrow : MonoBehaviour
    {
        bool _grown;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<CropGrow>()) return;
            new GameObject("CropGrow").AddComponent<CropGrow>();
        }

        void Update()
        {
            if (_grown) return;
            bool farm = GameObject.Find("Farm") || GameObject.Find("FarmBeds") || FarmFlag();
            if (!farm) return;
            var green = BastionGfx.Mat(new Color(0.22f, 0.48f, 0.20f), 0.15f);
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None))
            {
                if (t == null || !t.name.StartsWith("Crop")) continue;
                var r = t.GetComponent<Renderer>();
                if (r) r.sharedMaterial = green;
                t.localScale = new Vector3(1.6f, 0.7f, 1.4f);
            }
            _grown = true;
        }

        static bool FarmFlag()
        {
            var mbs = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            for (int i = 0; i < mbs.Length; i++)
            {
                var mb = mbs[i];
                if (mb == null) continue;
                var t = mb.GetType();
                var f = t.GetField("Sim") ?? t.GetField("sim") ?? t.GetField("_sim");
                if (f == null) continue;
                var sim = f.GetValue(mb);
                if (sim == null) continue;
                var st = sim.GetType();
                var built = st.GetField("FarmBuilt") ?? st.GetField("FarmQueued");
                if (built == null) continue;
                var v = built.GetValue(sim);
                if (v is bool b && b) return true;
            }
            return false;
        }
    }
}
