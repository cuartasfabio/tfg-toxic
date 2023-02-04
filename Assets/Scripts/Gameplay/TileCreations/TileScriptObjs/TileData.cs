using System;
using Gameplay.Tiles;
using UnityEngine;

namespace Gameplay.TileCreations.TileScriptObjs
{
    [Serializable, CreateAssetMenu(fileName = "New TileData", menuName = "Terrain/TileData")]
    public class TileData: ScriptableObject
    {
        [SerializeField] public Tile.Prop[] Props;

        [SerializeField] public bool HasCentralProp;
        [SerializeField] public Tile.Prop CentralProp;
        /// <summary>
        /// Material for the hexagonal base.
        /// </summary>
        [Space(20)][SerializeField] public Material TileBaseMaterial;

        /// <summary>
        /// Radius for the prop placement.
        /// </summary>
        [SerializeField] public float Radius = 1.7f; 
        
        /// <summary>
        /// Extra separation between props.
        /// </summary>
        [SerializeField] public float MinSeparation = 0.1f;
        
    }
}