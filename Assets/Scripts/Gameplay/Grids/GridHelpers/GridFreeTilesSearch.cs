using System.Collections.Generic;
using Gameplay.Tiles;

namespace Gameplay.Grids.GridHelpers
{
	public static class GridFreeTilesSearch
	{
		/// <summary>
		/// Checks if there are available spots to place any of the cards in hand.
		/// </summary>
		/// <param name="hand">The list of the current card(types) in hand.</param>
		/// <returns>The number of free tiles.</returns>
		public static int CheckFreeTiles(List<TileType> hand)
		{
			// si se puede colocar una tile en none (fuera de la grid) siempre va a poder colocarse
			
			HashSet<TileType> typesToCount = new HashSet<TileType>();

			for (int i = 0; i < hand.Count; i++)
			{
				// recorrer toda su fila de PlacingMatrix
				// for (int j = 0; j < TileTypes.TileTypeCount; j++)
				// {
				// 	if (TileTypes.PlacingMatrix[(int)hand[i],j] == 1)
				// 	{
				// 		typesToCount.Add((TileType) j);
				// 	}
				// }

				TileType[] placeableTypes = TileTypes.PlacingRules[(int) hand[i]];

				for (int j = 0; j < placeableTypes.Length; j++)
				{
					typesToCount.Add(placeableTypes[j]);
				}
			}

			int freeTileNum = 0;

			List<TileType> typeList = new List<TileType>(typesToCount);

			for (int i = 0; i <  typeList.Count; i++)
			{
				if(typeList[i] == TileType.None)
				{
					freeTileNum += 1;
				}
				else
				{
					freeTileNum += ObjectCache.Current.HexGrid.Lists.GetCellsOfType(typeList[i]).Count;
				}
			}

			return freeTileNum;

		}
	}
}