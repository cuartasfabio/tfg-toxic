using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.TileCreations.Formations;
using Gameplay.Tiles;
using Gameplay.Visitors.VECommands;

namespace Gameplay.Commands.TileBehaviourCommands.Formations
{
	public class RemoveFromFormationActionCommand : ITileActionCommand
	{
		private readonly float _delay;
		private readonly HexCoordinates _behCoordinates;
		private readonly TileType _type;

		public RemoveFromFormationActionCommand(HexCoordinates behCoordinates, TileType type, float delay)
		{
			_delay = delay;
			_behCoordinates = behCoordinates;
			_type = type;
		}

		public void Execute()
		{
			// Remove from formation
			ObjectCache.Current.FormationsRegister.RemoveLakeFromFormation(_behCoordinates);
			AdjacencySpritePicker.RecalculateNeighbourTilesSprites(_behCoordinates, _type,
				ObjectCache.Current.FormationsRegister.LakeVariants);
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