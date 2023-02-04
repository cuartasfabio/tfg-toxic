using System;
using Gameplay.Cards;
using Gameplay.Quests;
using Gameplay.TileCreations.TileScriptObjs;
using UnityEngine;

namespace Gameplay.Levels
{
	[Serializable, CreateAssetMenu(fileName = "New LevelData", menuName = "Levels/LevelData")]
	public class LevelData : ScriptableObject
	{
		// Order of generators matter, last has higher priority!
		[SerializeField] public TerrainTileData[] TileGenerationConfigs;

		[SerializeField] public QuestPoolsData questPoolsData;

		[SerializeField] public CardDeckData cardDeckData;

		[Tooltip("1 hard, 1.5 easy")]
		[SerializeField, Range(1.0f, 1.5f)] public float DifficultyRatio;
		
		[Tooltip("0 hard, 0.15 easy")]
		[SerializeField, Range(0.0f, 0.15f)] public float DiscoveryRate;

		[Space] [SerializeField] public Material BackgroundGridMat;

		// todo	ref to it's SOUNDTRACK
	}
}