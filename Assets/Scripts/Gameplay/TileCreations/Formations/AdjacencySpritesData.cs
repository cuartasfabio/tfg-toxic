using System;
using UnityEngine;

namespace Gameplay.TileCreations.Formations
{
    /// <summary>
    /// Contains all the sprite variations for every possible neighbour combination of lakes.
    ///
    ///
    /// 1. Given a certain Tile, we want to pick a sprite for it, based on the neighbour lake tiles.
    ///      (As we have and hexagonal grid, the number of neighbour lake combination is 2^6=64).
    /// 2. We can codify a neighbour combination with a "1" for every lake neighbour and a "0" por every non lake neighbour.
    /// 3. Starting from the north-east neighbour we have for example: 101010.
    /// 4. Turning these binary numbers to decimal we can use them as index to store the sprite variations.
    /// 
    /// </summary>
    [Serializable, CreateAssetMenu(fileName = "New AdjacencySpritesData", menuName = "Terrain/AdjacencySpritesData")]
    public class AdjacencySpritesData: ScriptableObject
    {
        [SerializeField] public Material Material;
        /// <summary>
        /// List containing the variations at their index.
        /// </summary>
        [SerializeField] public Sprite[] Variations;
    }
}