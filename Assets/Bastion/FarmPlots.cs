using UnityEngine;

namespace Bastion
{
    public sealed class FarmPlots : MonoBehaviour
    {
        bool _planted;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<FarmPlots>()) return;
            new GameObject("FarmPlots").AddComponent<FarmPlots>();
        }

        void Update()
        {
            if (_planted) return;
            var farm = GameObject.Find("Farm")
                       ?? GameObject.Find("FarmBeds")
                       ?? GameObject.Find("Green Tongue")
                       ?? GameObject.Find("GreenTongue");
            if (!farm)
            {
                farm = new GameObject("Green Tongue");
                farm.transform.position = new Vector3(-14f, 0f, 0f);
            }
            Plant(farm.transform);
            _planted = true;
        }

        static void Plant(Transform parent)
        {
            if (parent.Find("Bed0")) return;
            for (int i = 0; i < 6; i++)
            {
                int row = i / 3;
                int col = i % 3;
                var local = new Vector3((col - 1) * 2.2f, 0.16f, (row - 0.4f) * 2.5f);
                Box(parent, "Bed" + i, local, new Vector3(2.0f, 0.22f, 1.8f), BastionArt.Dirt());
                Box(parent, "Crop" + i, local + Vector3.up * 0.28f, new Vector3(1.6f, 0.42f, 1.4f), BastionArt.Glass());
            }
        }

        static void Box(Transform parent, string n, Vector3 local, Vector3 scale, Material mat)
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
