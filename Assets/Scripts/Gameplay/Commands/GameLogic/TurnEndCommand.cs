using System.Collections.Generic;
using Backend.Localization;
using Controls;
using Gameplay.Grids.GridHelpers;
using Gameplay.Tiles;
using Gameplay.Visitors.Tiles;
using Scenes;

namespace Gameplay.Commands.GameLogic
{
	public class TurnEndCommand : IGameCommand
	{
		public void Execute()
		{
			
			
			// check the QuestManager ...
			GameManager.Get().RunManager.LevelManager.QuestsManager.CheckForCompletion();
			
			// if objective reached, give cards
			
			
			// Checks for game over conditions
			// ObjectCache.Current.CommandBuffer.EnqueueCommand(new CheckFreeTilesCommand());
			List<TileType> hand = ObjectCache.Current.UiCardHand.GetHandTypes();

			// asegurarse que no quedan commands por ejecutar
			if (!ObjectCache.Current.CommandBuffer.IsQueueEmpty())
			{
				ObjectCache.Current.CommandBuffer.EnqueueCommand( new TurnEndCommand());
				return;
			}

			// if (hand.Count > 0 && FreeTileChecker.CheckFreeTiles(hand) < 1)
			if (GridFreeTilesSearch.CheckFreeTiles(hand) < 1)
			{
				// trigger game over
				string reason = hand.Count <= 0
					? StringBank.GetStringRaw("GAME_OVER_OUT_OF_CARDS")
					: StringBank.GetStringRaw("GAME_OVER_OUT_OF_SPACE");
				GameSceneManager.Current.TriggerGameOver(reason);
			}

			// ObjectCache.Current.CommandBuffer.EnqueueCommand(new EnableControlsCommand(true));
			GameControls.EnableControls(true);
			
			ObjectCache.Current.PlayerTurnManager.EndPlacing();
			
			
			
			GameManager.Get().SaveRun();
		}

		public float GetDelay()
		{
			return 0.0f;
		}
	}
}