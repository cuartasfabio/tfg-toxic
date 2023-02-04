using System.Collections.Generic;
using Backend.Localization;
using Gameplay.Grids.GridHelpers;
using Gameplay.Tiles;
using Scenes;

namespace Gameplay.Commands.GameLogic
{
	public class CheckFreeTilesCommand : IGameCommand
	{
		/// <summary>
		/// Checks if there are tiles to put the current cards in hand. If not, triggers the Game Over screen.
		/// </summary>
		public void Execute()
		{
            List<TileType> hand = ObjectCache.Current.UiCardHand.GetHandTypes();

            // asegurarse que no quedan commands por ejecutar
            if (!ObjectCache.Current.CommandBuffer.IsQueueEmpty())
            {
	            ObjectCache.Current.CommandBuffer.EnqueueCommand( new CheckFreeTilesCommand());
            }
            else
            {
	            // if (hand.Count > 0 && FreeTileChecker.CheckFreeTiles(hand) < 1)
	            if (GridFreeTilesSearch.CheckFreeTiles(hand) < 1)
	            {
		            // trigger game over
		            string reason = hand.Count <= 0
			            ? StringBank.GetStringRaw("GAME_OVER_OUT_OF_CARDS")
			            : StringBank.GetStringRaw("GAME_OVER_OUT_OF_SPACE");
		            GameSceneManager.Current.TriggerGameOver(reason);
	            }
            }
		}

		public float GetDelay()
		{
			return 0.0f;
		}
	}
}