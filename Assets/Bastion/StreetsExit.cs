using UnityEngine;

namespace Bastion
{
    public sealed class StreetsExit : MonoBehaviour
    {
        bool _pushed;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<StreetsExit>()) return;
            new GameObject("StreetsExit").AddComponent<StreetsExit>();
        }

        void LateUpdate()
        {
            if (_pushed) return;
            PersonActor poss = null;
            var all = Object.FindObjectsByType<PersonActor>(FindObjectsSortMode.None);
            for (int i = 0; i < all.Length; i++)
                if (all[i] && all[i].Job == Job.Possessed) { poss = all[i]; break; }
            if (!poss) return;
            var p = poss.transform.position;
            if (Mathf.Abs(p.x) < 4f && Mathf.Abs(p.z) < 4f)
            {
                poss.transform.position = new Vector3(0f, 0.02f, -6.2f);
                poss.HasTarget = false;
            }
            _pushed = true;
        }
    }
}
