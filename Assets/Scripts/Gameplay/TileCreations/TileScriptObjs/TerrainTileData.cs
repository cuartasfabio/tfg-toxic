using System;
using Gameplay.Tiles;
using UnityEngine;

namespace Gameplay.TileCreations.TileScriptObjs
{
    [Serializable, CreateAssetMenu(fileName = "New TerrainTileData", menuName = "Terrain/TerrainTileData")]
    public class TerrainTileData: ScriptableObject
    {
        /// <summary>
        /// Type of the Tile to appear.
        /// </summary>
        [SerializeField] public TileType Type;
        /// <summary>
        /// Scale of the noise.
        /// </summary>
        [SerializeField] public float Scale;
        /// <summary>
        /// Min Percentage to filter noise.
        /// </summary>
        [SerializeField] public float MinPercentage;
        /// <summary>
        /// Max Percentage to filter noise.
        /// </summary>
        [SerializeField] public float MaxPercentage;
    }
}