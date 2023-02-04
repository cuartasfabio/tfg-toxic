﻿using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Visitors.Tiles;

namespace Gameplay.Tiles.TileBehaviours
{
    public class CampsiteBehaviour: TileBehaviour
    {
        
        public override void SetCurrentCoordinates(HexCoordinates hexCoordinates)
        {
            base.SetCurrentCoordinates(hexCoordinates);
            ObjectCache.Current.HexGrid.Lists.TilesWithTurnEnd.Add(this);
            GameManager.Get().RunManager.LevelManager.LevelStats.CampsitesPlaced += 1; //todo smells...
        }
        
        public override void Accept(AbstractTileVisitor tileVisitor)
        {
            tileVisitor.Visit(this);
        }
        
    }
}