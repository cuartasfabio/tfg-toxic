using System.Collections;
using Backend.Persistence;
using Gameplay.Quests;
using UnityEngine;

namespace Gameplay.Levels
{
	public class LevelManager : IPersistable
	{
		private readonly RunManager _runManager;
		private readonly LevelData _levelData;
		
		public QuestsManager QuestsManager { get; private set; }
		public LevelStats LevelStats { get; private set; }


		public LevelManager(RunManager runManager, LevelData levelData)
		{
			_runManager = runManager;
			_levelData = levelData;
		}
		
		public IEnumerator LoadNextLevel()
		{
			LevelStats = new LevelStats();

			ObjectCache.Current.UIQuestList.Init();
			QuestsManager = new QuestsManager(); 
			if (_levelData.questPoolsData != null)
				QuestsManager.ChooseQuests(_levelData.questPoolsData);
			
			ObjectCache.Current.HexBackground.GetComponent<MeshRenderer>().material =
				_levelData.BackgroundGridMat;
			
			ObjectCache.Current.RaycastManager.Init();
			
			ObjectCache.Current.CameraControl.Init();
			
			ObjectCache.Current.HexGrid.Init();
			ObjectCache.Current.TerrainBuilder.Init(_runManager.GetCurrentLevelSeed(), _levelData.TileGenerationConfigs);
			ObjectCache.Current.GridSizeManager.Init(19,_levelData.DiscoveryRate); // 19 noraml // 256 terrain test
			
			ObjectCache.Current.PlayerTurnManager.Init();
			
			ObjectCache.Current.ScoreManager.Init(_levelData.DifficultyRatio);
			
			ObjectCache.Current.CardPool.Init();
			ObjectCache.Current.CardDeck.Init(_levelData.cardDeckData);
			ObjectCache.Current.UiCardHand.CleanHand();
			yield return new WaitForSeconds(3f);
			ObjectCache.Current.UiCardHand.Init();
			
			yield return null;
		}


		// ----------------------------------------------------------------------
		
		public void Save(GameDataWriter writer)
		{
			LevelStats.Save(writer);
			
			if (_levelData.questPoolsData != null)
				QuestsManager.Save(writer);
			
			ObjectCache.Current.HexGrid.Save(writer);
			
			ObjectCache.Current.FormationsRegister.Save(writer);
			
			ObjectCache.Current.ScoreManager.Save(writer);
			
			ObjectCache.Current.UiCardHand.Save(writer);
			
			ObjectCache.Current.CardDeck.Save(writer);
		}

		public void Load(GameDataReader reader)
		{
			LevelStats = new LevelStats();
			LevelStats.Load(reader);

			ObjectCache.Current.UIQuestList.Init();
			QuestsManager = new QuestsManager(); 
			if (_levelData.questPoolsData != null)
				QuestsManager.Load(reader);
			
			ObjectCache.Current.HexBackground.GetComponent<MeshRenderer>().material =
				_levelData.BackgroundGridMat;
			
			ObjectCache.Current.RaycastManager.Init();
			
			ObjectCache.Current.CameraControl.Init();
			
			ObjectCache.Current.HexGrid.Init();
			ObjectCache.Current.TerrainBuilder.Init(_runManager.GetCurrentLevelSeed(), _levelData.TileGenerationConfigs);
			ObjectCache.Current.GridSizeManager.Init(0,_levelData.DiscoveryRate);
			ObjectCache.Current.HexGrid.Load(reader);
			
			ObjectCache.Current.FormationsRegister.Load(reader);
			
			ObjectCache.Current.PlayerTurnManager.Init();
			
			ObjectCache.Current.ScoreManager.Init(_levelData.DifficultyRatio);
			ObjectCache.Current.ScoreManager.Load(reader);
			
			ObjectCache.Current.CardPool.Init();
			ObjectCache.Current.CardDeck.Init(_levelData.cardDeckData);
			
			ObjectCache.Current.UiCardHand.CleanHand();
			ObjectCache.Current.UiCardHand.Load(reader);
			
			ObjectCache.Current.CardDeck.Load(reader);
		}
	}
}