using System.Runtime.Serialization.Formatters;
using UnityEngine;

namespace TerrainGeneration {
    public static class HeightmapGenerator {
        public static float[,] GenerateHeightmap(NoiseSettings noiseSettings, int size, bool normalize = true) {
            var map = new float[size, size];
            var prng = new System.Random(noiseSettings.seed);
            var noise = new Noise();

            var offsets = new Vector2[noiseSettings.numLayers];
            for (int layer = 0; layer < noiseSettings.numLayers; layer++) {
                offsets[layer] = new Vector2((float)prng.NextDouble() * 2 - 1, (float)prng.NextDouble() * 2 - 1) * 10000;
            }

            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;

            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    float frequency = noiseSettings.scale;
                    float amplitude = 1.0f;
                    float height = 0.0f;

                    for (int layer = 0; layer < noiseSettings.numLayers; layer++) {
                        double sampleX = (double)x / size * frequency + offsets[layer].x + noiseSettings.offset.x;
                        double sampleY = (double)y / size * frequency + offsets[layer].y + noiseSettings.offset.y;
                        height += (float)noise.Evaluate(sampleX, sampleY) * amplitude;
                        frequency *= noiseSettings.lacunarity;
                        amplitude *= noiseSettings.persistence;
                    }
                    map[x, y] = Mathf.Pow(height * noiseSettings.fudgeFactor, noiseSettings.redistribution);
                    minHeight = Mathf.Min(minHeight, height);
                    maxHeight = Mathf.Max(maxHeight, height);
                }
            }

            if (normalize) {
                for (int x = 0; x < size; x++) {
                    for (int y = 0; y < size; y++) {
                        map[x, y] = Mathf.InverseLerp(minHeight, maxHeight, map[x, y]);
                    }
                }

            }
            return map;
        }
    }
}
