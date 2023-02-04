using System;
using Gameplay.Tiles;
using UnityEngine;

namespace Gameplay.Cards
{
    [Serializable, CreateAssetMenu(fileName = "New UICardData", menuName = "Cards/UICardData")]
    public class UICardData: ScriptableObject
    {
        [SerializeField] public TileType tileId;

        [SerializeField] public Sprite CardSprite;

        [SerializeField] public Color DetailColor;
        
        [SerializeField] public Material CardBaseMat;

        [SerializeField] public string CardName;

        [SerializeField] public string Synergies;
        
        // [SerializeField] public string[] Synergies;
    }
}