using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Visitors.VECommands;

namespace Gameplay.Commands.TileBehaviourCommands
{
	public class CardToHandActionCommand : ITileActionCommand
	{
		private readonly float _delay;
		public HexCoordinates OriginCoordinates { get; }
		public TileType TypeToCreate { get; }

		public CardToHandActionCommand( TileType typeToCreate, float delay, HexCoordinates coords = new HexCoordinates())
		{
			_delay = delay;
			OriginCoordinates = coords;
			TypeToCreate = typeToCreate;
		}
		
		public void Execute()
		{
			ObjectCache.Current.UiCardHand.CardToHand(TypeToCreate);
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