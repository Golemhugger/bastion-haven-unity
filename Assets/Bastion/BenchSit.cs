using UnityEngine;

namespace Bastion
{
    public sealed class BenchSit : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<BenchSit>()) return;
            new GameObject("BenchSit").AddComponent<BenchSit>();
        }

        void Start() { Invoke(nameof(Sit), 1.1f); }

        void Sit()
        {
            var root = GameObject.Find("Haven")?.transform ?? GameObject.Find("HavenLife")?.transform;
            if (!root) root = transform;
            int n = 0;
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None))
            {
                if (!t || t.name != "Bench") continue;
                var p = t.position + new Vector3(0f, 0f, 0.35f);
                var civ = PersonActor.Spawn(root, Role.Civilian, p, 40 + n);
                civ.Job = Job.Idle;
                civ.Home = p;
                civ.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                n++;
                if (n >= 6) break;
            }
        }
    }
}
