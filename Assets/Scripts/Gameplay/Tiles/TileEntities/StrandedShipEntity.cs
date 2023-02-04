using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class StrandedShipEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.StrandedShip;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_STRANDEDSHIP");
		// }
	}
}