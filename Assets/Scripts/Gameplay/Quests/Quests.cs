using Backend.Localization;
using Backend.Persistence;
using Cysharp.Text;
using Gameplay.Levels;
using Gameplay.Tiles;

namespace Gameplay.Quests
{
	public static class Quests // todo rework tocho - todo a ScriptableObjs
	{
		#region DEFAULT_QUESTS
		public class ExploreTiles : IQuest
		{
			private bool _completed;
			private readonly int _level;
			private QuestId _id;

			private int _lastExploredTiles;
			private int _objective;
			
			
			private readonly PlayerStats _playerStats;
			private readonly LevelStats _levelStats;
			public ExploreTiles(QuestId id, int level, int objective)
			{
				_id = id;
				_level = level;

				_objective = objective;
				
				_playerStats = GameManager.Get().PlayerStats;
				_levelStats = GameManager.Get().RunManager.LevelManager.LevelStats;
			}

			public bool CheckCompletion()
			{
				if (_completed) return true;

				_lastExploredTiles = _levelStats.ExploredTiles;
				if ( _lastExploredTiles >= _objective * _level)
				{
					_completed = true;
					// Debug.Log("Explore quest completed!");
					_playerStats.GenericQuestsCompleted++;
					return true;
				}
				return false;
			}

			public string GetProgress()
			{
				string ret = ZString.Concat(_lastExploredTiles, "/", _objective);
				return ret;
			}

			public void SetCompleted()
			{
				_completed = true;
			}

			public QuestId GetQuestId()
			{
				return _id;
			}

			public override string ToString()
			{
				string ret = StringBank.GetStringBaker("QUEST_DEFAULT_EXPLORE_TILES").ReplaceValue("amount", _objective)
					.Bake();
				return ret;
			}
		}
		
		public class BuildCamps : IQuest
		{
			private bool _completed;
			private readonly int _level;
			private QuestId _id;

			private int _lastCampsBuilt;
			private int _objective;
			
			
			private readonly PlayerStats _playerStats;
			private readonly LevelStats _levelStats;
			public BuildCamps(QuestId id, int level, int objective)
			{
				_id = id;
				_level = level;

				_objective = objective;
				
				_playerStats = GameManager.Get().PlayerStats;
				_levelStats = GameManager.Get().RunManager.LevelManager.LevelStats;
			}

			public bool CheckCompletion()
			{
				if (_completed) return true;
				
				_lastCampsBuilt = _levelStats.CampsitesPlaced;
				
				if ( _lastCampsBuilt >= _objective * _level)
				{
					_completed = true;
					// Debug.Log("Build quest completed!");
					_playerStats.GenericQuestsCompleted++;
					return true;
				}
				return false;	
			}
			
			public string GetProgress()
			{
				string ret = ZString.Concat(_lastCampsBuilt, "/", _objective);
				return ret;
			}
			
			public void SetCompleted()
			{
				_completed = true;
			}

			public QuestId GetQuestId()
			{
				return _id;
			}

			public override string ToString()
			{
				string ret = StringBank.GetStringBaker("QUEST_DEFAULT_BUILD_CAMPS").ReplaceValue("amount", _objective)
					.Bake();
				return ret;
			}
		}
		
		public class ScoreInOneTurn : IQuest
		{
			private bool _completed;
			private readonly int _level;
			private QuestId _id;

			private int _objective;


			private readonly PlayerStats _playerStats;
			private readonly LevelStats _levelStats;
			public ScoreInOneTurn(QuestId id, int level, int objective)
			{
				_id = id;
				_level = level;

				_objective = objective;

				_playerStats = GameManager.Get().PlayerStats;
				_levelStats = GameManager.Get().RunManager.LevelManager.LevelStats;
			}

			public bool CheckCompletion()
			{
				if (_completed) return true;
				
				if (_levelStats.LastTurnScore >= _objective * _level)
				{
					_completed = true;
					// Debug.Log("Score quest completed!");
					_playerStats.GenericQuestsCompleted++;
					return true;
				}
				return false;	
			}
			
			public string GetProgress()
			{
				return "";
			}
			
			public void SetCompleted()
			{
				_completed = true;
			}
			
			public QuestId GetQuestId()
			{
				return _id;
			}

			public override string ToString()
			{
				string ret = StringBank.GetStringBaker("QUEST_DEFAULT_SCORE_IN1TURNS").ReplaceValue("amount", _objective)
					.Bake();
				return ret;
			}
		}

		#endregion

		#region WORLD_1_QUESTS

		public class PlaceFarms : IQuest
		{
			private bool _completed;
			private readonly int _level;
			private QuestId _id;

			private int _lastCheck;
			private int _objective;
			
			
			private readonly PlayerStats _playerStats;
			private readonly LevelStats _levelStats;
			public PlaceFarms(QuestId id, int level, int objective)
			{
				_id = id;
				_level = level;

				_objective = objective;
				
				_playerStats = GameManager.Get().PlayerStats;
				_levelStats = GameManager.Get().RunManager.LevelManager.LevelStats;
			}

			public bool CheckCompletion()
			{
				if (_completed) return true;
				
				_lastCheck = _levelStats.FarmsPlaced;
				
				if ( _lastCheck >= _objective * _level)
				{
					_completed = true;
					_playerStats.World1QuestsCompleted++;
					return true;
				}
				return false;	
			}
			
			public string GetProgress()
			{
				string ret = ZString.Concat(_lastCheck, "/", _objective);
				return ret;
			}
			
			public void SetCompleted()
			{
				_completed = true;
			}

			public QuestId GetQuestId()
			{
				return _id;
			}

			public override string ToString()
			{
				string ret = StringBank.GetStringBaker("QUEST_WORLD1_FARMS").ReplaceValue("amount", _objective)
					.Bake();
				return ret;
			}
		}
		
		public class PlaceMeadows : IQuest
		{
			private bool _completed;
			private readonly int _level;
			private QuestId _id;

			private int _lastCheck;
			private int _objective;
			
			
			private readonly PlayerStats _playerStats;
			private readonly LevelStats _levelStats;
			public PlaceMeadows(QuestId id, int level, int objective)
			{
				_id = id;
				_level = level;

				_objective = objective;
				
				_playerStats = GameManager.Get().PlayerStats;
				_levelStats = GameManager.Get().RunManager.LevelManager.LevelStats;
			}

			public bool CheckCompletion()
			{
				if (_completed) return true;
				
				_lastCheck = _levelStats.MeadowsPlaced;
				
				if ( _lastCheck >= _objective * _level)
				{
					_completed = true;
					_playerStats.World1QuestsCompleted++;
					return true;
				}
				return false;	
			}
			
			public string GetProgress()
			{
				string ret = ZString.Concat(_lastCheck, "/", _objective);
				return ret;
			}
			
			public void SetCompleted()
			{
				_completed = true;
			}

			public QuestId GetQuestId()
			{
				return _id;
			}

			public override string ToString()
			{
				string ret = StringBank.GetStringBaker("QUEST_WORLD1_MEADOWS").ReplaceValue("amount", _objective)
					.Bake();
				return ret;
			}
		}

		#endregion
		
		#region WORLD_2_QUESTS

		public class PlaceDunes : IQuest
		{
			private bool _completed;
			private readonly int _level;
			private QuestId _id;

			private int _lastCheck;
			private int _objective;
			
			
			private readonly PlayerStats _playerStats;
			private readonly LevelStats _levelStats;
			public PlaceDunes(QuestId id, int level, int objective)
			{
				_id = id;
				_level = level;

				_objective = objective;
				
				_playerStats = GameManager.Get().PlayerStats;
				_levelStats = GameManager.Get().RunManager.LevelManager.LevelStats;
			}

			public bool CheckCompletion()
			{
				if (_completed) return true;
				
				_lastCheck = _levelStats.DunesPlaced;
				
				if ( _lastCheck >= _objective * _level)
				{
					_completed = true;
					_playerStats.World2QuestsCompleted++;
					return true;
				}
				return false;	
			}
			
			public string GetProgress()
			{
				string ret = ZString.Concat(_lastCheck, "/", _objective);
				return ret;
			}
			
			public void SetCompleted()
			{
				_completed = true;
			}

			public QuestId GetQuestId()
			{
				return _id;
			}

			public override string ToString()
			{
				string ret = StringBank.GetStringBaker("QUEST_WORLD2_DUNES").ReplaceValue("amount", _objective)
					.Bake();
				return ret;
			}
		}
		
		
		public class DiscoverShardMines : IQuest
		{
			private bool _completed;
			private readonly int _level;
			private QuestId _id;

			private int _lastCheck;
			private int _objective;
			
			
			private readonly PlayerStats _playerStats;
			private readonly LevelStats _levelStats;
			public DiscoverShardMines(QuestId id, int level, int objective)
			{
				_id = id;
				_level = level;

				_objective = objective;
				
				_playerStats = GameManager.Get().PlayerStats;
				_levelStats = GameManager.Get().RunManager.LevelManager.LevelStats;
			}

			public bool CheckCompletion()
			{
				if (_completed) return true;
				
				_lastCheck = ObjectCache.Current.HexGrid.Lists.GetCellsOfType(TileType.ShardMine).Count; // todo remove 
				
				if ( _lastCheck >= _objective * _level)
				{
					_completed = true;
					_playerStats.World2QuestsCompleted++;
					return true;
				}
				return false;	
			}
			
			public string GetProgress()
			{
				string ret = ZString.Concat(_lastCheck, "/", _objective);
				return ret;
			}
			
			public void SetCompleted()
			{
				_completed = true;
			}

			public QuestId GetQuestId()
			{
				return _id;
			}

			public override string ToString()
			{
				string ret = StringBank.GetStringBaker("QUEST_WORLD2_MINES").ReplaceValue("amount", _objective)
					.Bake();
				return ret;
			}
		}

		#endregion
		
		#region WORLD_3_QUESTS

		

		#endregion
	}
}