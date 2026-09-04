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
            var farm = GameObject.Find("Farm") ?? GameObject.Find("GreenTongue") ?? GameObject.Find("FarmBeds");
            if (!farm) return;
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
                var bed = GameObject.CreatePrimitive(PrimitiveType.Cube);
                bed.name = "Bed" + i;
                bed.transform.SetParent(parent, false);
                bed.transform.localPosition = new Vector3((col - 1) * 2.1f, 0.12f, (row - 0.5f) * 2.4f);
                bed.transform.localScale = new Vector3(1.8f, 0.18f, 1.6f);
                bed.GetComponent<Renderer>().sharedMaterial = BastionArt.Dirt();
                Object.Destroy(bed.GetComponent<Collider>());

                var crop = GameObject.CreatePrimitive(PrimitiveType.Cube);
                crop.name = "Crop" + i;
                crop.transform.SetParent(parent, false);
                crop.transform.localPosition = bed.transform.localPosition + new Vector3(0f, 0.28f, 0f);
                crop.transform.localScale = new Vector3(1.5f, 0.35f, 1.3f);
                crop.GetComponent<Renderer>().sharedMaterial = BastionArt.Glass();
                Object.Destroy(crop.GetComponent<Collider>());
            }
        }
    }
}
