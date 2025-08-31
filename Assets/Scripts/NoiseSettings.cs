using UnityEngine;

namespace TerrainGeneration {
    [System.Serializable]
    public class NoiseSettings {
        public int seed;
        public float scale = 0.1f;

        public Vector2 offset;
        public int numLayers = 4;
        public float lacunarity = 2.0f;
        public float persistence = 0.5f;
        public float fudgeFactor = 1.2f;
        public float redistribution = 1.0f;
    }
}