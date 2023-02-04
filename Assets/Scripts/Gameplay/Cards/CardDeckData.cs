using System;
using Gameplay.Tiles;
using UnityEngine;

namespace Gameplay.Cards
{
	[Serializable, CreateAssetMenu(fileName = "New CardDeckData", menuName = "Cards/CardDeckData")]
	public class CardDeckData : ScriptableObject
	{
		/// <summary>
		/// The size of the deck.
		/// </summary>
		[SerializeField] public int PoolSize;
		/// <summary>
		/// TileTypes of the NormalTier (have the same rarity or chance of appearance).
		/// </summary>
		[SerializeField] public TileType[] NormalTier;
        
		
		[Serializable]
		public class SpecialTier
		{
			public TileType[] CardTypes;
			/// <summary>
			/// A NormalPerSpecial=2 means Cards in this Tier have half the chances of appearance.
			/// </summary>
			public float NormalPerSpecial;
		}
        
		[SerializeField] public SpecialTier[] SpecialTiers;
	}
}