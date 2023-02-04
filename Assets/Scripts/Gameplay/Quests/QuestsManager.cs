using System;
using System.Collections.Generic;
using Backend.Persistence;
using Utils;

namespace Gameplay.Quests
{
	public class QuestsManager : IPersistable
	{
		private List<IQuest> _currentQuests;
		private List<bool> _questsCompleted;
		private Dictionary<QuestId, Func<IQuest>> _questIdDict;

		private bool _allCompleted;

		public QuestsManager()
		{
			_currentQuests = new List<IQuest>();
			_questsCompleted = new List<bool>();
			
			_questIdDict = new Dictionary<QuestId, Func<IQuest>>()
			{
				{QuestId.DEFAULT_EXPLORE_TILES_1, () => new Quests.ExploreTiles(QuestId.DEFAULT_EXPLORE_TILES_1, 1, 4)},
				{QuestId.DEFAULT_BUILD_CAMPS_1, () => new Quests.BuildCamps(QuestId.DEFAULT_BUILD_CAMPS_1, 1, 2)},
				{QuestId.DEFAULT_SCORE_IN1TURN_1, () => new Quests.ScoreInOneTurn(QuestId.DEFAULT_SCORE_IN1TURN_1, 1, 20)},
				
				{QuestId.DEFAULT_EXPLORE_TILES_2, () => new Quests.ExploreTiles(QuestId.DEFAULT_EXPLORE_TILES_2, 1, 8)},
				{QuestId.DEFAULT_BUILD_CAMPS_2, () => new Quests.BuildCamps(QuestId.DEFAULT_BUILD_CAMPS_2, 1, 3)},
				{QuestId.DEFAULT_SCORE_IN1TURN_2, () => new Quests.ScoreInOneTurn(QuestId.DEFAULT_SCORE_IN1TURN_2, 1, 30)},
				
				{QuestId.WORLD1_TEST_1, () => new Quests.PlaceFarms(QuestId.WORLD1_TEST_1, 1, 2)},
				{QuestId.WORLD1_TEST_2, () => new Quests.PlaceMeadows(QuestId.WORLD1_TEST_2, 1, 2)},
				{QuestId.WORLD2_TEST_1, () => new Quests.PlaceDunes(QuestId.WORLD2_TEST_1, 1, 2)},
				{QuestId.WORLD2_TEST_2, () => new Quests.DiscoverShardMines(QuestId.WORLD2_TEST_2, 1, 2)},
				
			};
		}
		
		// game starts. chooses random quests from a pool of quest for the current level
		public void ChooseQuests(QuestPoolsData poolsData)
		{
			// shuffle pools and pick as many as required

			QuestId[] def = poolsData.DefaultQuests;
			RandomTf.KFYShuffle(def);
			for (int i = 0; i < poolsData.NumberOfDefaultQuests; i++)
			{
				_currentQuests.Add( _questIdDict[def[i]].Invoke());	
				_questsCompleted.Add( false);	
			}
			
			QuestId[] spe = poolsData.SpecificQuests;
			RandomTf.KFYShuffle(spe);
			for (int i = 0; i < poolsData.NumberOfSpecificQuests; i++)
			{
				_currentQuests.Add( _questIdDict[spe[i]].Invoke());
				_questsCompleted.Add( false);
			}
			
			// update the UI
			ObjectCache.Current.UIQuestList.FillQuestList(_currentQuests);
		}
		
		// every turn end, check the quests for completion
		// if every quests is completed -> game ends
		public void CheckForCompletion()
		{
			if (_allCompleted) return;
			
			bool everyQuestChecked = true;
			
			for (int i = 0; i < _currentQuests.Count; i++)
			{
				
				if (_currentQuests[i].CheckCompletion())
				{
					
					if (!_questsCompleted[i])
					{
						CompleteQuest(i);
					}
						
				}
				else
				{
					everyQuestChecked = false;
				}
				
				ObjectCache.Current.UIQuestList.UpdateProgressOfQuest(i);
					
			}

			if (everyQuestChecked && _currentQuests.Count > 0)
			{
				_allCompleted = true;
				ObjectCache.Current.UIQuestList.EnableNextWorldButton();
			}
		}

		// -------------------------------------------------------------------------------------------------------------

		public void Save(GameDataWriter writer)
		{
			writer.Write(_currentQuests.Count);
			for (int i = 0; i < _currentQuests.Count; i++)
			{
				writer.Write((int)_currentQuests[i].GetQuestId());
			}

			for (int i = 0; i < _currentQuests.Count; i++)
			{
				writer.Write(_questsCompleted[i]);
			}
		}

		public void Load(GameDataReader reader)
		{
			int questNum = reader.ReadInt();
			for (int i = 0; i < questNum; i++)
			{
				QuestId id = (QuestId) reader.ReadInt();
				_currentQuests.Add(_questIdDict[id].Invoke());
			}

			ObjectCache.Current.UIQuestList.FillQuestList(_currentQuests);
			
			for (int i = 0; i < questNum; i++)
			{
				bool completed = reader.ReadBool();
				_questsCompleted.Add(false);
				if (completed) _currentQuests[i].SetCompleted();
			}
			CheckForCompletion();
		}

		private void CompleteQuest(int i)
		{
			ObjectCache.Current.UIQuestList.MarkQuestAsCompleted(i);
			_questsCompleted[i] = true;
		}

		public void CompleteQuestsCommand()
		{
			for (int i = 0; i < _currentQuests.Count; i++)
			{
				if (!_questsCompleted[i])
				{
					CompleteQuest(i);
				}
			}
			ObjectCache.Current.UIQuestList.EnableNextWorldButton();
		}
	}
}