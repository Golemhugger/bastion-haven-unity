using UnityEngine;

namespace Bastion
{
    public static class InteriorSet
    {
        public static void HQ(Transform parent, Vector3 origin)
        {
            Floor(parent, origin, 7.2f, 7.2f);
            Prop(parent, origin + new Vector3(0f, 0.55f, 0.4f), new Vector3(1.8f, 0.12f, 1.1f), new Color(0.55f, 0.32f, 0.12f));
            Prop(parent, origin + new Vector3(0f, 0.62f, 0.4f), new Vector3(1.4f, 0.04f, 0.8f), new Color(0.72f, 0.42f, 0.14f));
            Prop(parent, origin + new Vector3(-2.2f, 0.7f, -1.8f), new Vector3(0.9f, 1.4f, 0.28f), new Color(0.18f, 0.16f, 0.14f));
        }

        public static void Hab(Transform parent, Vector3 origin)
        {
            Floor(parent, origin, 4.6f, 4.6f);
            Prop(parent, origin + new Vector3(-1.1f, 0.35f, -0.8f), new Vector3(1.6f, 0.4f, 0.8f), new Color(0.32f, 0.22f, 0.16f));
            Prop(parent, origin + new Vector3(1.2f, 0.45f, 1.0f), new Vector3(0.7f, 0.7f, 0.7f), new Color(0.28f, 0.24f, 0.18f));
        }

        public static void Barracks(Transform parent, Vector3 origin)
        {
            Floor(parent, origin, 8.2f, 5.2f);
            for (int i = 0; i < 4; i++)
                Prop(parent, origin + new Vector3(-2.4f + i * 1.6f, 0.35f, -1.2f), new Vector3(1.4f, 0.4f, 0.7f), new Color(0.16f, 0.22f, 0.22f));
        }

        static void Floor(Transform parent, Vector3 origin, float w, float d)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Floor";
            go.transform.SetParent(parent, false);
            go.transform.position = origin + new Vector3(0f, 0.06f, 0f);
            go.transform.localScale = new Vector3(w, 0.08f, d);
            go.GetComponent<Renderer>().sharedMaterial = BastionArt.Dirt();
        }

        static void Prop(Transform parent, Vector3 pos, Vector3 scale, Color c)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Prop";
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = BastionGfx.Mat(c);
            Object.Destroy(go.GetComponent<Collider>());
        }
    }
}
