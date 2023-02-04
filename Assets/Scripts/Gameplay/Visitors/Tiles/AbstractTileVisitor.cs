using System.Collections.Generic;
using Gameplay.Commands.TileBehaviourCommands;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;

namespace Gameplay.Visitors.Tiles
{
    /// <summary>
    /// Visitor interface for the TileBehaviour object structure.
    /// </summary>
    public abstract class AbstractTileVisitor
    {
        protected List<ITileActionCommand> _commands;

        public List<ITileActionCommand> GetBehaviourCommands(TileBehaviour tile)
        {
            _commands = new List<ITileActionCommand>();
                tile.Accept(this);
            return _commands;
        } 
        
        public List<ITileActionCommand> GetBehaviourCommands(List<TileBehaviour> tiles)
        {
            _commands = new List<ITileActionCommand>();
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Accept(this);
            }
            return _commands;
        } 
        
        public virtual void Visit(GroundBehaviour behaviour) { }
        public virtual void Visit(RuinsBehaviour behaviour) {  }
        public virtual void Visit(CrystalsBehaviour behaviour) {  }
        public virtual void Visit(LakeBehaviour behaviour) {  }
        public virtual void Visit(ForestBehaviour behaviour) {  }
        public virtual void Visit(MeadowBehaviour behaviour) {  }
        public virtual void Visit(HerbivoresBehaviour behaviour) {  }
        public virtual void Visit(CarnivoresBehaviour behaviour) {  }
        public virtual void Visit(CampsiteBehaviour behaviour) {  }
        public virtual void Visit(ExplorerBehaviour behaviour) {  }
        public virtual void Visit(WastesBehaviour behaviour) {  }
        public virtual void Visit(FrontierBehaviour behaviour) {  }
        public virtual void Visit(DeepForestBehaviour behaviour) {  }
        public virtual void Visit(VillageBehaviour behaviour) {  }
        public virtual void Visit(CityBehaviour behaviour) {  }
        public virtual void Visit(MountainBehaviour behaviour) {  }
        public virtual void Visit(DunesBehaviour behaviour) {  }
        public virtual void Visit(FactoryBehaviour behaviour) {  }
        public virtual void Visit(FarmBehaviour behaviour) {  }
        public virtual void Visit(FarmlandBehaviour behaviour) {  }
        public virtual void Visit(FossilsBehaviour behaviour) {  }
        public virtual void Visit(LumberjackBehaviour behaviour) {  }
        public virtual void Visit(MonolithBehaviour behaviour) {  }
        public virtual void Visit(MutantsBehaviour behaviour) {  }
        public virtual void Visit(OasisBehaviour behaviour) {  }
        public virtual void Visit(OldCabinBehaviour behaviour) {  }
        public virtual void Visit(PurifierBehaviour behaviour) {  }
        public virtual void Visit(RadioTowerBehaviour behaviour) {  }
        public virtual void Visit(ShardMineBehaviour behaviour) {  }
        public virtual void Visit(StrandedShipBehaviour behaviour) {  }
        public virtual void Visit(SwampBehaviour behaviour) {  }
        public virtual void Visit(WarpedWoodsBehaviour behaviour) {  }
        
        /*public NewTileVisitor()
        {
           
        }
		
        public override void Visit(LakeBehaviour behaviour)
        {
	        
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
        
        public override void Visit(FrontierBehaviour behaviour)
        {
            
        }*/
        
    }
}

