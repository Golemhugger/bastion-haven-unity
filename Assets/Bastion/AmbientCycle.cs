using UnityEngine;

namespace Bastion
{
    public sealed class AmbientCycle : MonoBehaviour
    {
        Light _sun;
        float _t;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<AmbientCycle>()) return;
            new GameObject("AmbientCycle").AddComponent<AmbientCycle>();
        }

        void Start()
        {
            _sun = Object.FindFirstObjectByType<Light>();
            if (_sun == null)
            {
                var go = new GameObject("Sun");
                _sun = go.AddComponent<Light>();
                _sun.type = LightType.Directional;
            }
            _sun.color = new Color(1f, 0.72f, 0.48f);
            _sun.intensity = 1.05f;
            RenderSettings.ambientLight = new Color(0.28f, 0.24f, 0.22f);
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.18f, 0.16f, 0.14f);
            RenderSettings.fogDensity = 0.012f;
        }

        void Update()
        {
            _t += Time.deltaTime * 0.04f;
            float pulse = 0.92f + 0.12f * Mathf.Sin(_t);
            if (_sun)
            {
                _sun.intensity = pulse;
                _sun.transform.rotation = Quaternion.Euler(28f + Mathf.Sin(_t * 0.35f) * 6f, 210f, 0f);
            }
        }
    }
}
