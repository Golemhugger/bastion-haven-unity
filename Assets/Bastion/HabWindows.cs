using UnityEngine;

namespace Bastion
{
    public sealed class HabWindows : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<HabWindows>()) return;
            new GameObject("HabWindows").AddComponent<HabWindows>();
        }

        void Start() { Invoke(nameof(Fix), 0.7f); }

        void Fix()
        {
            var glass = BastionGfx.Mat(new Color(0.55f, 0.42f, 0.22f), 0.45f);
            var frame = BastionArt.Wood();
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None))
            {
                if (t.name != "Win" && t.name != "Window") continue;
                var r = t.GetComponent<Renderer>();
                if (r) r.sharedMaterial = glass;
                if (t.Find("Frame")) continue;
                var f = GameObject.CreatePrimitive(PrimitiveType.Cube);
                f.name = "Frame";
                f.transform.SetParent(t, false);
                f.transform.localPosition = Vector3.zero;
                f.transform.localScale = new Vector3(1.15f, 1.15f, 0.35f);
                f.GetComponent<Renderer>().sharedMaterial = frame;
                Object.Destroy(f.GetComponent<Collider>());
            }
        }
    }
}
