using System.Collections;
using Backend.Persistence;
using Scenes;
using UnityEngine;
using Utils;

namespace Gameplay.Levels
{
	public class RunManager : IPersistable
	{
		
		public int GlobalScore { get; private set; }
		public int Seed { get; private set; }
		public GameModeData GameModeData { get; private set; }
		
		// --- current level ---
		public int CurrentLevelIndex { get; private set; }
		public LevelManager LevelManager { get; private set; }
		// ---------------------
		
		private readonly PlayerStats _playerStats;

		public RunManager()
		{
			_playerStats = GameManager.Get().PlayerStats;
		}
		
		public RunManager(GameModeData gameModeData)
		{
			GlobalScore = 0;
			GameModeData = gameModeData;
			// Seed = GameModeConfig.TerrainSeed;
			Seed = RandomTf.Rng.Next(256);
			// Seed = 79;
			// Debug.Log(Seed);
			CurrentLevelIndex = -1;

			_playerStats = GameManager.Get().PlayerStats;
			
		}


		public void SetGlobalScore(int score)
		{
			GlobalScore += score;
			if (GameModeData.GameMode == GameMode.RUN_MODE)
			{
				// PlayerStats.SetRunHighScore(GlobalScore);
				_playerStats.SetRunHighScore(GlobalScore);
			}
			else
			{
				// PlayerStats.SetEndlessHighScore(GlobalScore);
				_playerStats.SetEndlessHighScore(GlobalScore);
			}
		}

		public int GetHighScore()
		{
			if (GameModeData.GameMode == GameMode.RUN_MODE)
			{
				return _playerStats.ModeRunHighScore;
			}
			return _playerStats.ModeEndlessHighScore;
		}
		
		private LevelData GetCurrentLevel()
		{
			return GameModeData.Levels[CurrentLevelIndex];
		}

		public int GetCurrentLevelSeed()
		{
			return Seed + CurrentLevelIndex;
		}

		public bool IsThereANextLevel()
		{
			return CurrentLevelIndex < GameModeData.Levels.Length - 1;
		}
		
		// -------------------------------------------

		public void LoadSavedLevel()
		{
			LevelManager = new LevelManager(this, GetCurrentLevel());
			GameManager.Get().PersistentStorage.Load(LevelManager,PersistentStorage.SaveType.LEVEL);
		}
		
		public IEnumerator LoadNextLevel()
		{
			CurrentLevelIndex++;
			LevelManager = new LevelManager(this, GetCurrentLevel());
			yield return GameSceneManager.Current.StartCoroutine(LevelManager.LoadNextLevel());
		}

		
		// -------------------------------------------------------------------------------------------------------------
		
		public void Save(GameDataWriter writer)
		{
			writer.Write(GlobalScore);
			writer.Write(Seed);
			writer.Write((int) GameModeData.GameMode);
			writer.Write(CurrentLevelIndex);
			
			// GameManager.Get().PersistentStorage.Save(LevelManager,PersistentStorage.SaveType.LEVEL);
		}

		public void Load(GameDataReader reader)
		{
			GlobalScore = reader.ReadInt();
			Seed = reader.ReadInt();
			GameModeData = GameManager.Get().GetGameModeConfig((GameMode)reader.ReadInt());
			CurrentLevelIndex = reader.ReadInt();
		}
	}
}