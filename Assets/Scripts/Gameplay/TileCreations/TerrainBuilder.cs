using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.TileCreations.TerrainFunctions;
using Gameplay.TileCreations.TileScriptObjs;
using Gameplay.Tiles;
using UnityEngine;

namespace Gameplay.TileCreations
{
    public class TerrainBuilder: MonoBehaviour
    {
        private int _seed;
        private TerrainTileData[] _configs;
        
        private MultiPerlinTiler _perlin;

        public void Init(int seed, TerrainTileData[] configs)
        {
            _seed = seed;
            _configs = configs;
            _perlin = new MultiPerlinTiler();
        }
        
        /// <summary>
        /// Return a TileType for a given HexCoordinate based on the TerrainTileData.
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public TileType GetTerrainTileForCoords(HexCoordinates coords)
        {
            TileType type = TileType.None;
            Vector3 position = HexCoordinates.ToPosition(coords);
            
            for (int i = 0; i < _configs.Length; i++)
            {
                type = _perlin.CheckTilePerlinValue(_seed, _configs[i], position.x, position.z);
                if (type != TileType.Ground)
                    break;
            }
            
            return type;
        }
    }
}