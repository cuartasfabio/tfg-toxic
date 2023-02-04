using Gameplay.Commands.TileBehaviourCommands.Formations;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;

namespace Gameplay.Visitors.Tiles
{
	public class OnDeleteVisitor: AbstractTileVisitor
	{
		
        public override void Visit(LakeBehaviour behaviour)
        {
	        HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);
	        // todo solo para debug, meterlo a _commands
	        ObjectCache.Current.CommandBuffer.EnqueueCommand(new RemoveFromFormationActionCommand(behaviourCoords, TileType.Lake, 0.1f));
        }
        
        public override void Visit(ForestBehaviour behaviour)
        {
           
        }
        
        public override void Visit(HerbivoresBehaviour behaviour)
        {
           
        }
        
        public override void Visit(CarnivoresBehaviour behaviour)
        {
          
        }
        
        public override void Visit(CampsiteBehaviour behaviour)
        {

        }
        
        public override void Visit(ExplorerBehaviour behaviour)
        {
	        
        }
        
        public override void Visit(DeepForestBehaviour behaviour)
        {

        }
         
        public override void Visit(WastesBehaviour behaviour)
        {
            
        }
        
    }
	
}