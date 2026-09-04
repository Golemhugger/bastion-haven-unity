using UnityEngine;

namespace Bastion
{
    public static class BastionArt
    {
        static Texture2D _brick, _asphalt, _canvas, _metal, _glass, _window, _wood, _dirt, _roof, _rust;

        public static Material Brick() => TexMat(ref _brick, PaintBrick, new Color(0.36f, 0.28f, 0.22f), 0f, 0.28f);
        public static Material Asphalt() => TexMat(ref _asphalt, PaintAsphalt, new Color(0.12f, 0.12f, 0.14f), 0f, 0.55f);
        public static Material Canvas() => TexMat(ref _canvas, PaintCanvas, new Color(0.48f, 0.28f, 0.16f), 0f, 0.18f);
        public static Material Metal() => TexMat(ref _metal, PaintMetal, new Color(0.30f, 0.34f, 0.36f), 0f, 0.62f);
        public static Material Glass() => TexMat(ref _glass, PaintGlass, new Color(0.16f, 0.34f, 0.26f, 0.55f), 0.15f, 0.8f);
        public static Material Window() => TexMat(ref _window, PaintWindow, new Color(1f, 0.72f, 0.38f), 2.2f, 0.4f);
        public static Material Wood() => TexMat(ref _wood, PaintWood, new Color(0.32f, 0.22f, 0.14f), 0f, 0.22f);
        public static Material Dirt() => TexMat(ref _dirt, PaintDirt, new Color(0.18f, 0.15f, 0.12f), 0f, 0.2f);
        public static Material Roof() => TexMat(ref _roof, PaintRoof, new Color(0.24f, 0.22f, 0.20f), 0f, 0.15f);
        public static Material Rust() => TexMat(ref _rust, PaintRust, new Color(0.42f, 0.22f, 0.12f), 0f, 0.25f);

        static Material TexMat(ref Texture2D tex, System.Action<Texture2D> paint, Color fallback, float emit, float smooth)
        {
            if (tex == null)
            {
                tex = new Texture2D(64, 64, TextureFormat.RGBA32, false);
                tex.wrapMode = TextureWrapMode.Repeat;
                tex.filterMode = FilterMode.Bilinear;
                paint(tex);
                tex.Apply();
            }
            var m = BastionGfx.Mat(fallback, emit);
            if (m.HasProperty("_BaseMap")) m.SetTexture("_BaseMap", tex);
            if (m.HasProperty("_MainTex")) m.SetTexture("_MainTex", tex);
            return m;
        }

        static void PaintBrick(Texture2D t)
        {
            var rng = new System.Random(11);
            for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                int row = y / 8;
                int off = (row % 2) * 8;
                bool mortar = y % 8 == 0 || (x + off) % 16 == 0;
                float n = (rng.Next(0, 20) - 10) / 255f;
                t.SetPixel(x, y, mortar
                    ? new Color(0.18f, 0.14f, 0.11f)
                    : new Color(0.36f + n, 0.27f + n, 0.20f + n));
            }
        }

        static void PaintAsphalt(Texture2D t)
        {
            var rng = new System.Random(7);
            for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                float n = rng.Next(0, 18) / 255f;
                var c = new Color(0.10f + n, 0.10f + n, 0.12f + n);
                if (y >= 30 && y <= 33 && (x / 8) % 2 == 0) c = new Color(0.72f, 0.58f, 0.28f);
                t.SetPixel(x, y, c);
            }
        }

        static void PaintCanvas(Texture2D t)
        {
            var rng = new System.Random(19);
            for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                float n = rng.Next(-12, 12) / 255f;
                var c = new Color(0.50f + n, 0.28f + n * 0.5f, 0.14f);
                if (y % 12 < 2) c *= 0.82f;
                t.SetPixel(x, y, c);
            }
        }

        static void PaintMetal(Texture2D t)
        {
            var rng = new System.Random(3);
            for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                float n = rng.Next(0, 22) / 255f;
                var c = new Color(0.28f + n, 0.32f + n, 0.34f + n);
                if (x % 16 < 2) c *= 0.55f;
                t.SetPixel(x, y, c);
            }
        }

        static void PaintGlass(Texture2D t)
        {
            for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                float v = 0.16f + 0.08f * Mathf.Sin(x * 0.2f) + 0.05f * Mathf.Sin(y * 0.15f);
                t.SetPixel(x, y, new Color(0.12f, 0.34f + v, 0.24f, 0.55f));
            }
        }

        static void PaintWindow(Texture2D t)
        {
            for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                bool frame = x < 4 || x > 59 || y < 4 || y > 59 || x == 32 || y == 32;
                t.SetPixel(x, y, frame
                    ? new Color(0.08f, 0.07f, 0.06f)
                    : new Color(1f, 0.74f, 0.40f));
            }
        }

        static void PaintWood(Texture2D t)
        {
            var rng = new System.Random(29);
            for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                float n = rng.Next(-10, 10) / 255f;
                float grain = 0.04f * Mathf.Sin(y * 0.4f + x * 0.02f);
                t.SetPixel(x, y, new Color(0.30f + n + grain, 0.20f + n, 0.12f));
            }
        }

        static void PaintDirt(Texture2D t)
        {
            var rng = new System.Random(5);
            for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                float n = rng.Next(0, 24) / 255f;
                t.SetPixel(x, y, new Color(0.16f + n, 0.13f + n * 0.7f, 0.10f + n * 0.4f));
            }
        }

        static void PaintRoof(Texture2D t)
        {
            var rng = new System.Random(41);
            for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                float n = rng.Next(0, 18) / 255f;
                var c = new Color(0.18f + n, 0.16f + n, 0.14f + n);
                if (y % 8 < 2) c *= 0.7f;
                t.SetPixel(x, y, c);
            }
        }

        static void PaintRust(Texture2D t)
        {
            var rng = new System.Random(17);
            for (int y = 0; y < 64; y++)
            for (int x = 0; x < 64; x++)
            {
                float n = rng.Next(0, 28) / 255f;
                t.SetPixel(x, y, new Color(0.38f + n, 0.18f + n * 0.5f, 0.10f));
            }
        }
    }
}
