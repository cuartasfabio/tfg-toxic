using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Visitors.Tiles;

namespace Gameplay.Tiles.TileBehaviours
{
	public class VillageBehaviour : TileBehaviour
	{
		
		public override void SetCurrentCoordinates(HexCoordinates hexCoordinates)
		{
			base.SetCurrentCoordinates(hexCoordinates);
			GameManager.Get().RunManager.LevelManager.LevelStats.CampsitesPlaced += 1;
		}
		
		public override void Accept(AbstractTileVisitor tileVisitor)
		{
			tileVisitor.Visit(this);
		}
	}
}