using Backend.Persistence;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Levels;
using Gameplay.Visitors.Tiles;

namespace Gameplay.Tiles.TileBehaviours
{
    public class ForestBehaviour: TileBehaviour
    {
        public override void SetCurrentCoordinates(HexCoordinates hexCoordinates)
        {
            base.SetCurrentCoordinates(hexCoordinates);
            // ObjectCache.Current.HexGrid.Lists.TilesWithTurnEnd.Add(this);
            LevelStats levelStats = GameManager.Get().RunManager.LevelManager.LevelStats;
            levelStats.IncrementNaturalTiles();
            levelStats.IncrementForestsPlaced();
        }

        public override void Accept(AbstractTileVisitor tileVisitor)
        {
            tileVisitor.Visit(this);
        }
        
    }
}