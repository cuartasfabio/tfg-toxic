using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class WarpedWoodsEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.WarpedWoods;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_WARPEDWOODS");
		// }
	}
}