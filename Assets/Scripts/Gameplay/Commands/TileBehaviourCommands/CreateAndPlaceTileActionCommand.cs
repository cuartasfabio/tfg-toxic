using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;
using Gameplay.Visitors.VECommands;

namespace Gameplay.Commands.TileBehaviourCommands
{
    public class CreateAndPlaceTileActionCommand: ITileActionCommand
    {
        private readonly float _delay;
        public HexCoordinates TileToReplace { get; }
        public TileType NewType { get; }
        public bool SameAsHover { get; }


        public CreateAndPlaceTileActionCommand(HexCoordinates coords, TileType type, float delay, bool sameAsHover = false)
        {
            _delay = delay;
            TileToReplace = coords;
            NewType = type;
            SameAsHover = sameAsHover;
        }
        
        public void Execute()
        {
            TileBehaviour behaviour = ObjectCache.Current.TileBehaviourPool.GetNewTile(NewType, TileToReplace);
            
            // List<IOnPlaceCommand> cardCommands = new OnPlaceVisitor().GetBehaviourCommands(behaviour);
            // ObjectCache.Current.CommandBuffer.EnqueueBehaviourCommands(cardCommands);
            
            ObjectCache.Current.HexGrid.PlaceBehaviourAtCoords(behaviour, TileToReplace);
            
            behaviour.RunPlaceAnimation();
        }

        public float GetDelay()
        {
            return _delay;
        }

        public void Accept(AbstractActionVisitor commandVisitor)
        {
            commandVisitor.Visit(this);
        }

    }
}