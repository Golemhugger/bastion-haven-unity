using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Bastion
{
    public sealed class JuiceAudio : MonoBehaviour
    {
        AudioSource _src;
        AudioClip _blip, _thud, _drone;
        int _log;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<JuiceAudio>()) return;
            new GameObject("JuiceAudio").AddComponent<JuiceAudio>();
        }

        void Start()
        {
            _src = gameObject.AddComponent<AudioSource>();
            _src.playOnAwake = false;
            _src.spatialBlend = 0f;
            _blip = Tone(880f, 0.07f, 0.18f);
            _thud = Tone(110f, 0.18f, 0.28f);
            _drone = Tone(55f, 1.8f, 0.04f);
            _src.clip = _drone;
            _src.loop = true;
            _src.volume = 0.12f;
            _src.Play();
        }

        void Update()
        {
            int n = LogCount();
            if (n > _log)
            {
                _log = n;
                OneShot(_blip, 0.35f);
            }
        }

        static int LogCount()
        {
            var mbs = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            for (int i = 0; i < mbs.Length; i++)
            {
                var mb = mbs[i];
                if (mb == null) continue;
                var f = mb.GetType().GetField("Sim") ?? mb.GetType().GetField("sim");
                if (f == null) continue;
                var sim = f.GetValue(mb);
                if (sim == null) continue;
                var logF = sim.GetType().GetField("Log");
                if (logF == null) continue;
                if (logF.GetValue(sim) is List<string> log) return log.Count;
            }
            return 0;
        }

        void OneShot(AudioClip clip, float vol)
        {
            if (!_src || !clip) return;
            _src.PlayOneShot(clip, vol);
        }

        static AudioClip Tone(float hz, float seconds, float amp)
        {
            int rate = 22050;
            int samples = Mathf.Max(256, (int)(rate * seconds));
            var data = new float[samples];
            for (int i = 0; i < samples; i++)
            {
                float t = i / (float)rate;
                float env = 1f - i / (float)samples;
                data[i] = Mathf.Sin(2f * Mathf.PI * hz * t) * amp * env;
            }
            var clip = AudioClip.Create("tone" + hz, samples, 1, rate, false);
            clip.SetData(data, 0);
            return clip;
        }
    }
}
