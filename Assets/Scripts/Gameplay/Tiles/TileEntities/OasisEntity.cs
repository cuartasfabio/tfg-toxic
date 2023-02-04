using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class OasisEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Oasis;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_OASIS");
		// }
	}
}