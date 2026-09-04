using UnityEngine;

namespace Bastion
{
    public sealed class NightLook : MonoBehaviour
    {
        Camera _cam;
        Light _sun;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<NightLook>()) return;
            new GameObject("NightLook").AddComponent<NightLook>();
        }

        void Start()
        {
            _cam = Camera.main ?? Object.FindFirstObjectByType<Camera>();
            _sun = Object.FindFirstObjectByType<Light>();
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
        }

        void LateUpdate()
        {
            float day = Mathf.Repeat(Time.time * 0.015f, 1f);
            float dusk = Mathf.Clamp01(Mathf.Sin(day * Mathf.PI * 2f) * 0.5f + 0.5f);
            var warm = Color.Lerp(new Color(0.55f, 0.38f, 0.28f), new Color(1f, 0.78f, 0.55f), dusk);
            var fog = Color.Lerp(new Color(0.06f, 0.05f, 0.07f), new Color(0.22f, 0.18f, 0.14f), dusk);
            RenderSettings.ambientLight = warm * (0.22f + dusk * 0.18f);
            RenderSettings.fogColor = fog;
            RenderSettings.fogDensity = 0.010f + (1f - dusk) * 0.012f;
            if (_sun)
            {
                _sun.color = warm;
                _sun.intensity = 0.35f + dusk * 0.85f;
                _sun.shadows = LightShadows.Soft;
            }
            if (_cam) _cam.backgroundColor = fog;
        }
    }
}
