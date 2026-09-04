using UnityEngine;

namespace Bastion
{
    public sealed class DoorCut : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Attach()
        {
            if (Object.FindFirstObjectByType<DoorCut>()) return;
            new GameObject("DoorCut").AddComponent<DoorCut>();
        }

        void Start() { Invoke(nameof(Cut), 0.3f); }

        void Cut()
        {
            var hq = GameObject.Find("HQ");
            if (!hq) return;
            if (hq.transform.Find("LobbyFloor")) return;

            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "LobbyFloor";
            floor.transform.SetParent(hq.transform, false);
            floor.transform.localPosition = new Vector3(0f, 0.08f, 0f);
            floor.transform.localScale = new Vector3(6.4f, 0.12f, 6.4f);
            floor.GetComponent<Renderer>().sharedMaterial = BastionArt.Dirt();

            var door = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door.name = "SouthDoor";
            door.transform.SetParent(hq.transform, false);
            door.transform.localPosition = new Vector3(0f, 1.1f, -3.35f);
            door.transform.localScale = new Vector3(1.8f, 2.2f, 0.28f);
            door.GetComponent<Renderer>().sharedMaterial = BastionArt.Wood();
            var col = door.GetComponent<Collider>();
            col.isTrigger = true;

            var table = GameObject.CreatePrimitive(PrimitiveType.Cube);
            table.name = "MapTable";
            table.transform.SetParent(hq.transform, false);
            table.transform.localPosition = new Vector3(0f, 0.62f, 0.2f);
            table.transform.localScale = new Vector3(1.8f, 0.14f, 1.1f);
            table.GetComponent<Renderer>().sharedMaterial = BastionGfx.Mat(new Color(0.72f, 0.42f, 0.14f), 0.4f);
            Object.Destroy(table.GetComponent<Collider>());
        }
    }
}
