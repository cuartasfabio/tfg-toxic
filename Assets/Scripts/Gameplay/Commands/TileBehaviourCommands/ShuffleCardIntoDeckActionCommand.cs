using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Visitors.VECommands;

namespace Gameplay.Commands.TileBehaviourCommands
{
	public class ShuffleCardIntoDeckActionCommand : ITileActionCommand
	{
		private readonly float _delay;
		public HexCoordinates OriginCoordinates { get; }
		public TileType TypeToShuffle { get; }

		public ShuffleCardIntoDeckActionCommand( TileType typeToShuffle, float delay, HexCoordinates coords = new HexCoordinates())
		{
			_delay = delay;
			OriginCoordinates = coords;
			TypeToShuffle = typeToShuffle;
		}
		
		public void Execute()
		{
			ObjectCache.Current.CardDeck.ShuffleIntoDeck(TypeToShuffle);
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