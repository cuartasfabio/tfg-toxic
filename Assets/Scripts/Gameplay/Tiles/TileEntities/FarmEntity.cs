using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class FarmEntity : ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Farm;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_FARM");
		// }
	}
}