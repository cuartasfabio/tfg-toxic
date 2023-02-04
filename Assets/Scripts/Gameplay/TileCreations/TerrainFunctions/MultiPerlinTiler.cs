using Gameplay.TileCreations.TileScriptObjs;
using Gameplay.Tiles;
using UnityEngine;

namespace Gameplay.TileCreations.TerrainFunctions
{
    public class MultiPerlinTiler
    {
        private float _offset1;
        private float _offset2;
        private float _offset3;
        
        /// <summary>
        /// Checks the future TileType of a given HexCoordinate. Using 3 perlin noise passes,
        /// with the configuration from a TerrainTileData.
        /// </summary>
        /// <param name="seed">The seed of the current level.</param>
        /// <param name="terrainTileData"></param>
        /// <param name="x">X hexagonal coordinate.</param>
        /// <param name="z">Z hexagonal coordinate.</param>
        /// <returns>The chosen TileType.</returns>
        public TileType CheckTilePerlinValue(int seed, TerrainTileData terrainTileData, float x, float z)
        {
            ProcessSeed(terrainTileData, seed);
            
            float rawPerlinNormal =  Mathf.PerlinNoise((x - _offset1),(z - _offset1));
            float rawPerlinInverted =  Mathf.PerlinNoise((z - _offset2),(x - _offset2));
            float rawPerlin =  Mathf.PerlinNoise((rawPerlinNormal * x - _offset3) / terrainTileData.Scale,
                (rawPerlinInverted * z - _offset3) / terrainTileData.Scale); 
            float clampPerlin = Mathf.Clamp01(rawPerlin);
            
            if (clampPerlin >= terrainTileData.MinPercentage && clampPerlin <= terrainTileData.MaxPercentage)
            {
                return terrainTileData.Type;
            }
            
            return TileType.Ground;
        }
        
        // Uses Scale, the percentages and the Seed to randomize the offsets
        private void ProcessSeed(TerrainTileData terrainTileData, int seed)
        {
            _offset1 = (seed/3f) * (terrainTileData.Scale / terrainTileData.MinPercentage) + Mathf.Pow(2.7182f,2);
            _offset2 = (seed/3f) * Mathf.PI + Mathf.Pow(3.1415f,3);
            _offset3 = (seed/3f) + Mathf.Sqrt(seed * terrainTileData.MaxPercentage);
        }
    }
}