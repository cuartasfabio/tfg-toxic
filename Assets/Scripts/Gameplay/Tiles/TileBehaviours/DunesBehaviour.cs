using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Visitors.Tiles;

namespace Gameplay.Tiles.TileBehaviours
{
	public class DunesBehaviour: TileBehaviour
	{
		public override void SetCurrentCoordinates(HexCoordinates hexCoordinates)
		{
			base.SetCurrentCoordinates(hexCoordinates);
			// ObjectCache.Current.HexGrid.Lists.TilesWithTurnEnd.Add(this);
			GameManager.Get().RunManager.LevelManager.LevelStats.IncrementNaturalTiles();
			GameManager.Get().RunManager.LevelManager.LevelStats.DunesPlaced += 1; //todo smells...
		}
		
		public override void Accept(AbstractTileVisitor tileVisitor)
		{
			tileVisitor.Visit(this);
		}
	}
}