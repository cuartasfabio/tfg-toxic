using System.Collections.Generic;
using Audio;
using Controls;
using Gameplay.Commands.GameLogic;
using Gameplay.Commands.HoverPreviewCommands;
using Gameplay.Commands.TileBehaviourCommands;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.PlayerTurn;
using Gameplay.Visitors.Tiles;
using Gameplay.Visitors.VECommands;

namespace Gameplay.PlayerControllers
{ 
    public class PlayerTurnStatePlacing : IPlayerTurnState
    {
        private readonly PlayerTurnManager _playerTurnManager;
        private HexCoordinates _previousHoveredCoords;
        // private TileBehaviour _previousHoveredTile;
        private List<IActionPreviewCommand> _visualEffCommands;
        private OnHoverVisualEffectsVisitor _visualEffectsVisitor;
        private HoverCommandsRecord _hoverCommandsRecord;
        private OnTurnEndVisitor _turnEndVisitor;
        private List<ITileActionCommand> _onTurnEndCommands;
        private List<IActionPreviewCommand> _onTurnEndEffectsCommands;
        // sustain time
        private readonly float _hoverMinSustainTime;
        private float _sustainTime;



        public PlayerTurnStatePlacing(PlayerTurnManager playerTurnManager)
        {
            _playerTurnManager = playerTurnManager;
            // sustain time
            _hoverMinSustainTime = 0.03f;
            _sustainTime = 0.0f;
            
            _hoverCommandsRecord = new HoverCommandsRecord();
            _onTurnEndCommands = new List<ITileActionCommand>();
            _onTurnEndEffectsCommands = new List<IActionPreviewCommand>();

            _turnEndVisitor = new OnTurnEndVisitor();
        }
        
        public void Enter()
        {
            _previousHoveredCoords = ObjectCache.Current.HexGrid.GridBounds.GetOutOfCameraCoordinate();
            _visualEffectsVisitor = new OnHoverVisualEffectsVisitor();
            _visualEffCommands = new List<IActionPreviewCommand>();
            ObjectCache.Current.UiCardHand.DisableCards();
            
            // get and preview all turn end actions
            _onTurnEndCommands = _turnEndVisitor.GetBehaviourCommands(ObjectCache.Current.HexGrid.Lists.TilesWithTurnEnd);
            _onTurnEndEffectsCommands =  _visualEffectsVisitor.GetPreviewCommands(_onTurnEndCommands);
            ObjectCache.Current.CommandBuffer.EnqueueVisualEffectCommands(_onTurnEndEffectsCommands);
            // _playerHand.SelectedCard.ShowCard();
            // _previousHoveredTile = null;
        }
    
        public void UpdateState()
        {
            ObjectCache.Current.RaycastManager.GetRaycastOnGrid();
            
            // Hex coordinates of hovered cell
            HexCoordinates hoveredCoords = HexFunctions.GetCoordinatesFromPosition(ObjectCache.Current.RaycastManager.LastHitOnGrid);
            
            // Sticks the Behaviour to the Tile
            _playerTurnManager.GetBehaviourPreview().transform.position = HexFunctions.SnapToGrid(hoveredCoords);
            
            // Hover changes
            if (!hoveredCoords.Equals(_previousHoveredCoords) && _sustainTime > _hoverMinSustainTime)
            {
                _sustainTime = 0.0f;

                ShowLastHoveredTile(true);
                
                _previousHoveredCoords = hoveredCoords;

                ShowLastHoveredTile(false);

                // AudioController.Get().PlaySfx(AudioId.SFX_TileHover);
                // Update tile border position
                _playerTurnManager.HoveredTileBorder.Move(hoveredCoords);

                UndoVisualEffectPreviews(_visualEffCommands);
                // ObjectCache.Current.CommandBuffer.ClearQueue();

                List<ITileActionCommand> onPlaceCommands = 
                    _hoverCommandsRecord.
                        GetCommandsForCardInTile(_playerTurnManager.SelectedCard.GetCardTileType(), hoveredCoords, _playerTurnManager.GetBehaviourPreview());

                _visualEffCommands = _visualEffectsVisitor.GetPreviewCommands(onPlaceCommands);
                
                ObjectCache.Current.CommandBuffer.EnqueueVisualEffectCommands(_visualEffCommands);

            }
            else
            {
                _sustainTime += 0.01f;
            }
            
            if (GameControls.IsUiMouseLeftButtonPressed(out int lClickPressed) && _canBePlaced)
            {
                
                AudioController.Get().PlaySfx(AudioId.SFX_TilePlacingGeneric);
                
                GameControls.EnableControls(false);
                
                _playerTurnManager.GetBehaviourPreview().EnableView(false);
                
                // Reverses all visual effects from grid and ui
                UndoVisualEffectPreviews(_visualEffCommands);
                UndoVisualEffectPreviews(_onTurnEndEffectsCommands);
                
                // First card consume animation
                ObjectCache.Current.CommandBuffer.EnqueueCommand(new ConsumePlacedCardCommand(_playerTurnManager.SelectedCard, 0.5f));

                // Enqueues commands to apply placement
                ObjectCache.Current.CommandBuffer.EnqueueBehaviourCommands(
                    _hoverCommandsRecord.GetCommandsForCardInTile(
                        _playerTurnManager.SelectedCard.GetCardTileType(), 
                        hoveredCoords, 
                        _playerTurnManager.GetBehaviourPreview()));
                
                
                // Run OnTurnEndVisitor
                ObjectCache.Current.CommandBuffer.
                	EnqueueBehaviourCommands(_onTurnEndCommands);
                
                ObjectCache.Current.CommandBuffer.EnqueueCommand(new TurnEndCommand());

                _hoverCommandsRecord.Reset();
                
                // ObjectCache.Current.PlayerHand.EndPlacing();
            }
            
            if (GameControls.IsUiMouseRightButtonPressed(out int rClickPressed))
            {
                UndoVisualEffectPreviews(_visualEffCommands);
                UndoVisualEffectPreviews(_onTurnEndEffectsCommands);
                AudioController.Get().PlaySfx(AudioId.SFX_CancelPlacing);
                _playerTurnManager.CancelPlacing();
            }
            
        }
        
        public void Exit()
        {
            ShowLastHoveredTile(true);
        }
        
        private void UndoVisualEffectPreviews(List<IActionPreviewCommand> commands)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                commands[i].SkipExecution(true);
                commands[i].Undo();
            }
            commands.Clear();
        }
        
        private bool _canBePlaced;

        public void SetCanBePlaced(bool b)
        {
            _canBePlaced = b;
        }

        private void ShowLastHoveredTile(bool show)
        {
            if (ObjectCache.Current.HexGrid.Lists.CoordinatesBehaviours.ContainsKey(_previousHoveredCoords))
            {
                ObjectCache.Current.HexGrid.Lists.CoordinatesBehaviours[_previousHoveredCoords].gameObject.SetActive(show);
            }
        }
        
    }
}


