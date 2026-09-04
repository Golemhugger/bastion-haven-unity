using UnityEngine;
using UnityEngine.Rendering;

namespace Bastion
{
    public sealed class PinkFix : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<PinkFix>()) return;
            new GameObject("PinkFix").AddComponent<PinkFix>();
        }

        void Start() { Invoke(nameof(Sweep), 0.05f); Invoke(nameof(Sweep), 0.6f); }

        void Sweep()
        {
            if (GraphicsSettings.currentRenderPipeline != null) return;
            var sh = Shader.Find("Standard")
                     ?? Shader.Find("Unlit/Color")
                     ?? Shader.Find("Sprites/Default");
            if (sh == null) return;
            var renderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
            for (int i = 0; i < renderers.Length; i++)
            {
                var r = renderers[i];
                if (r == null || r.sharedMaterial == null) continue;
                var cur = r.sharedMaterial.shader;
                bool urp = cur != null && cur.name.IndexOf("Universal", System.StringComparison.OrdinalIgnoreCase) >= 0;
                bool pink = r.sharedMaterial.HasProperty("_Color") && r.sharedMaterial.color.r > 0.9f && r.sharedMaterial.color.g < 0.15f && r.sharedMaterial.color.b > 0.9f;
                if (!urp && !pink && cur != null) continue;
                var src = r.sharedMaterial;
                var m = new Material(sh);
                if (src.HasProperty("_Color") && m.HasProperty("_Color")) m.color = src.color;
                if (src.HasProperty("_BaseColor") && m.HasProperty("_Color")) m.color = src.GetColor("_BaseColor");
                if (src.HasProperty("_BaseMap") && src.GetTexture("_BaseMap") && m.HasProperty("_MainTex"))
                    m.mainTexture = src.GetTexture("_BaseMap");
                r.sharedMaterial = m;
            }
        }
    }
}
