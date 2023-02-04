using System;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.PlayerTurn;
using Gameplay.Tiles;
using Gameplay.Visitors.Tiles;
using UI.InGames.TileBorders;

namespace Gameplay.PlayerControllers
{
    /// <summary>
    /// </summary>
    public class PlayerTurnStateIdle : IPlayerTurnState
    {
        private HexCoordinates _previousHoveredCoords;
        private TileBehaviour _previoursHoveredTile;
        private readonly PlayerTurnManager _playerTurnManager;

        public PlayerTurnStateIdle(PlayerTurnManager playerTurnManager)
        {
            _playerTurnManager = playerTurnManager;
            
            // _outCoord = new HexCoordinates(Int32.MaxValue, Int32.MaxValue);
        }
    
        public void Enter()
        {
            _previousHoveredCoords = new HexCoordinates(Int32.MaxValue, Int32.MaxValue);
            _previoursHoveredTile = null;
        }
        
        public void UpdateState()
        {
            ObjectCache.Current.RaycastManager.GetRaycastOnGrid();
            
            HexCoordinates hoveredCoords = HexFunctions.GetCoordinatesFromPosition(ObjectCache.Current.RaycastManager.LastHitOnGrid); 
            
            if (!hoveredCoords.Equals(_previousHoveredCoords))
            {
                _previousHoveredCoords = hoveredCoords;

                // AudioController.Get().PlayAudio(AudioType.SFX_TileHover);
                _playerTurnManager.HoveredTileBorder.Move(hoveredCoords);
                _playerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.Idle);

                if (ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(hoveredCoords) != TileType.None)
                {
                    if (_previoursHoveredTile != null) _previoursHoveredTile.RunHoverExitAnimation();
                    _previoursHoveredTile = ObjectCache.Current.HexGrid.Lists.CoordinatesBehaviours[hoveredCoords];
                    _previoursHoveredTile.RunHoverEnterAnimation();
                }
                
                ObjectCache.Current.HelpManager.UpdateHelpPanel(
                    ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(hoveredCoords));
            }
        }
        
        public void Exit()
        {
           
        }
    }
}


