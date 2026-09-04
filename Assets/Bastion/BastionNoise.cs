using UnityEngine;

namespace Bastion
{
    public static class BastionNoise
    {
        static readonly int[] Perm = new int[512];
        static bool _init;

        public static void Seed(int seed = 1337)
        {
            var rng = new System.Random(seed);
            var p = new int[256];
            for (int i = 0; i < 256; i++) p[i] = i;
            for (int i = 255; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                int t = p[i]; p[i] = p[j]; p[j] = t;
            }
            for (int i = 0; i < 512; i++) Perm[i] = p[i & 255];
            _init = true;
        }

        static void Ensure() { if (!_init) Seed(1337); }

        static float Fade(float t) => t * t * t * (t * (t * 6f - 15f) + 10f);
        static float Lerp(float a, float b, float t) => a + t * (b - a);

        static float Grad(int h, float x, float y)
        {
            int g = h & 7;
            float u = (g < 4) ? x : y;
            float v = (g < 4) ? y : x;
            return ((g & 1) == 0 ? u : -u) + ((g & 2) == 0 ? v : -v);
        }

        public static float Hash(int x, int y)
        {
            Ensure();
            int n = Perm[(x + Perm[y & 255]) & 255];
            return n / 255f;
        }

        public static float Value(float x, float y)
        {
            int x0 = Mathf.FloorToInt(x);
            int y0 = Mathf.FloorToInt(y);
            float fx = x - x0;
            float fy = y - y0;
            float u = Fade(fx);
            float v = Fade(fy);
            float a = Hash(x0, y0);
            float b = Hash(x0 + 1, y0);
            float c = Hash(x0, y0 + 1);
            float d = Hash(x0 + 1, y0 + 1);
            return Lerp(Lerp(a, b, u), Lerp(c, d, u), v);
        }

        public static float Perlin(float x, float y)
        {
            Ensure();
            int xi = Mathf.FloorToInt(x) & 255;
            int yi = Mathf.FloorToInt(y) & 255;
            float xf = x - Mathf.Floor(x);
            float yf = y - Mathf.Floor(y);
            float u = Fade(xf);
            float v = Fade(yf);
            int aa = Perm[Perm[xi] + yi];
            int ab = Perm[Perm[xi] + yi + 1];
            int ba = Perm[Perm[xi + 1] + yi];
            int bb = Perm[Perm[xi + 1] + yi + 1];
            float x1 = Lerp(Grad(aa, xf, yf), Grad(ba, xf - 1f, yf), u);
            float x2 = Lerp(Grad(ab, xf, yf - 1f), Grad(bb, xf - 1f, yf - 1f), u);
            return Lerp(x1, x2, v) * 0.5f + 0.5f;
        }

        public static float Value2(float x, float y, int seed = 0) { Seed(seed == 0 ? 1337 : seed); return Value(x, y); }
        public static float Perlin2(float x, float y, int seed = 0) { Seed(seed == 0 ? 1337 : seed); return Perlin(x, y); }

        public static float Fbm(float x, float y, int octaves = 4, float lacunarity = 2f, float gain = 0.5f)
        {
            float amp = 0.5f;
            float freq = 1f;
            float sum = 0f;
            float norm = 0f;
            for (int i = 0; i < octaves; i++)
            {
                sum += Perlin(x * freq, y * freq) * amp;
                norm += amp;
                amp *= gain;
                freq *= lacunarity;
            }
            return norm > 0f ? sum / norm : 0f;
        }

        public static float Ridged(float x, float y, int octaves = 4)
        {
            float amp = 0.5f;
            float freq = 1f;
            float sum = 0f;
            float norm = 0f;
            for (int i = 0; i < octaves; i++)
            {
                float n = 1f - Mathf.Abs(Perlin(x * freq, y * freq) * 2f - 1f);
                n *= n;
                sum += n * amp;
                norm += amp;
                amp *= 0.5f;
                freq *= 2.1f;
            }
            return norm > 0f ? sum / norm : 0f;
        }

        public static float Turbulence(float x, float y, int octaves = 4)
        {
            float amp = 0.5f;
            float freq = 1f;
            float sum = 0f;
            float norm = 0f;
            for (int i = 0; i < octaves; i++)
            {
                sum += Mathf.Abs(Perlin(x * freq, y * freq) * 2f - 1f) * amp;
                norm += amp;
                amp *= 0.5f;
                freq *= 2f;
            }
            return norm > 0f ? sum / norm : 0f;
        }

        public static Vector2 Warp(float x, float y, float strength = 1.4f)
        {
            float wx = Fbm(x + 19.1f, y + 4.7f, 3);
            float wy = Fbm(x + 71.3f, y + 12.8f, 3);
            return new Vector2(x + (wx - 0.5f) * strength, y + (wy - 0.5f) * strength);
        }

        public static float WarpedFbm(float x, float y, int octaves = 4)
        {
            var w = Warp(x, y);
            return Fbm(w.x, w.y, octaves);
        }

        public static float ZeroOne(float n) => Mathf.Clamp01(n);
        public static float ZeroOne(float x, float y) => Mathf.Clamp01(Fbm(x, y, 4));
        public static float ZeroOne(float x, float y, int octaves) => Mathf.Clamp01(Fbm(x, y, octaves));

        public static Color Tint(Color a, Color b, float t) => Color.Lerp(a, b, Mathf.Clamp01(t));
        public static Color Tint(Color baseColor, float n) => Color.Lerp(baseColor, Color.white, Mathf.Clamp01(n) * 0.25f);

        public static Color Lerp3(Color a, Color b, Color c, float t)
        {
            t = Mathf.Clamp01(t);
            if (t < 0.5f) return Color.Lerp(a, b, t * 2f);
            return Color.Lerp(b, c, (t - 0.5f) * 2f);
        }

        public static Texture2D Bake(int size, System.Func<float, float, Color> sample)
        {
            size = Mathf.Clamp(size, 8, 256);
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.wrapMode = TextureWrapMode.Repeat;
            tex.filterMode = FilterMode.Bilinear;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                tex.SetPixel(x, y, sample(x / (float)size, y / (float)size));
            tex.Apply();
            return tex;
        }
    }
}
