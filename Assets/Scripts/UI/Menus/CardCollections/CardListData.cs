using System;
using Gameplay.Cards;
using Gameplay.Tiles;
using UnityEngine;

namespace UI.Menus.CardCollections
{
	[Serializable, CreateAssetMenu(fileName = "New CardListData", menuName = "Cards/CardListData")]
	public class CardListData : ScriptableObject
	{
		[Serializable]
		public class CardConfig
		{
			public UICardData cardData;
			// public TileType CardId;
			// public Sprite CardIllus;
			public CardCategory Category; // to pick Sprite from CardListMenu._categoryBorders
			public CardStatus DefaultStatus;
			// public string Description;
		}

		[SerializeField] public CardConfig[] Cards;
	}
	
	public enum CardStatus
	{
		BLOCKED,
		UNLOCKED
	}
	
	public enum CardCategory
	{
		PLACEABLE,
		GENERATED,
		TERRAIN
	}
}