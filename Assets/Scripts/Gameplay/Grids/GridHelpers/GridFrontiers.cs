using System.Collections.Generic;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;

namespace Gameplay.Grids.GridHelpers
{
	/// <summary>
	/// Keeps track of the frontier tiles and creates new frontier when a tile is placed.
	/// </summary>
	public class GridFrontiers
	{
		private HashSet<HexCoordinates> _frontiersHash;
		private List<HexCoordinates> _frontiersList;

		public void Init()
		{
			_frontiersHash = new HashSet<HexCoordinates>();
			_frontiersList = new List<HexCoordinates>();
		}
		
		public void UpdateFrontiers(HexCoordinates coords)
		{
			if (_frontiersHash.Contains(coords)) _frontiersHash.Remove(coords);
            
			List<HexCoordinates> borders = HexFunctions.GetNullNeighborsCoordinates(coords);
            
			// actualizar fronteras
			for (int i = 0; i < borders.Count; i++)
			{
				if (_frontiersHash.Add(borders[i]))
				{
					_frontiersList.Add(borders[i]);
					
					ObjectCache.Current.HexGrid.PlaceTypeAtCoords(TileType.Frontier, borders[i]);
				}
			}
		}
	}
}