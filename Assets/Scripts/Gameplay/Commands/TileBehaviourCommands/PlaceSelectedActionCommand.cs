using Audio;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;
using Gameplay.Visitors.VECommands;

namespace Gameplay.Commands.TileBehaviourCommands
{
	public class PlaceSelectedActionCommand : ITileActionCommand
	{
		private readonly float _delay;
		private readonly HexCoordinates _tileToReplace;
		private readonly TileBehaviour _behaviour;

		public PlaceSelectedActionCommand(HexCoordinates tileToReplace, TileBehaviour behaviour, float delay)
		{
			_delay = delay;
			_tileToReplace = tileToReplace;
			_behaviour = behaviour;
		}
        
		public void Execute()
		{
			// ObjectCache.Current.PlayerHand.EndPlacing();
			AudioController.Get().PlaySfx(TileTypeAudios.GetAudioType(_behaviour.TileEntity.GetCardId()));

			ObjectCache.Current.HexGrid.PlaceBehaviourAtCoords(_behaviour, _tileToReplace);
			// _behaviour.gameObject.SetActive(true);
			_behaviour.EnableView(true);
			_behaviour.ShowTile();
			_behaviour.RunPlaceAnimation();
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