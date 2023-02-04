using System;
using UnityEngine;

namespace Gameplay.Levels
{
	[Serializable, CreateAssetMenu(fileName = "New GameModeData", menuName = "Levels/GameModeData")]
	public class GameModeData : ScriptableObject
	{
		[SerializeField] public GameMode GameMode;

		[SerializeField] public LevelData[] Levels;
	}

	public enum GameMode
	{
		RUN_MODE,
		ENDLESS_MODE
	}
}