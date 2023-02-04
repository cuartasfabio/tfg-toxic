using System.Collections.Generic;
using Gameplay.Commands.TileBehaviourCommands;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Visitors.Tiles;

namespace Gameplay.PlayerControllers
{
	public class HoverCommandsRecord
	{
		// private Dictionary<HexCoordinates, List<IOnPlaceBehaviourCommand>> _commandRecord;
		private Dictionary<TileBehaviour, Dictionary<HexCoordinates, List<ITileActionCommand>>> _cardCommandRecord;

		public HoverCommandsRecord()
		{
			Reset();
		}

		public List<ITileActionCommand> GetCommandsForCardInTile(TileType cardType, HexCoordinates coords, TileBehaviour beh)
		{
			if (!_cardCommandRecord.ContainsKey(beh))
			{
				_cardCommandRecord.Add(beh, new Dictionary<HexCoordinates, List<ITileActionCommand>>());
			}

			if (!_cardCommandRecord[beh].ContainsKey(coords))
			{
				_cardCommandRecord[beh].Add(coords, new OnPlaceVisitor().GetBehaviourCommands(beh));
				return _cardCommandRecord[beh][coords];
			}

			new OnPlaceVisitor().GetBehaviourCommands(beh);
			
			return _cardCommandRecord[beh][coords];
			
			// if (!_commandRecord.ContainsKey(tileCoords))
			// 	_commandRecord.Add(tileCoords, new OnPlaceVisitor().GetBehaviourCommands(behaviour));
			// return _commandRecord[tileCoords];
		}

		public void Reset()
		{
			// _commandRecord = new Dictionary<HexCoordinates, List<IOnPlaceBehaviourCommand>>();
			_cardCommandRecord = new Dictionary<TileBehaviour, Dictionary<HexCoordinates, List<ITileActionCommand>>>();
		}
	}
}