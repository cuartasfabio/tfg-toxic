using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.TileCreations.Formations;
using Gameplay.Tiles;
using Gameplay.Visitors.VECommands;

namespace Gameplay.Commands.TileBehaviourCommands.Formations
{
	public class AddToFormationActionCommand : ITileActionCommand
	{
		private readonly float _delay;
		private readonly TileBehaviour _behaviour;
		private readonly HexCoordinates _behCoords;
		private readonly TileType _type;
		
		public AddToFormationActionCommand(TileBehaviour behaviour, HexCoordinates behCoords, TileType type, float delay)
		{
			_delay = delay;
			_behaviour = behaviour;
			_behCoords = behCoords;
			_type = type;
		}

		public void Execute()
		{
			//HexCoordinates behCoords = ObjectCache.Current.HexGrid.Lists.BehaviourCoordinates[_behaviour];
			
			// recalcular sprite  sus adyacentes
			AdjacencySpritePicker.PickSpriteForTile(_behaviour, _type, ObjectCache.Current.FormationsRegister.LakeVariants);

			// Add to formation
			ObjectCache.Current.FormationsRegister.RegisterLake(_behCoords);
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