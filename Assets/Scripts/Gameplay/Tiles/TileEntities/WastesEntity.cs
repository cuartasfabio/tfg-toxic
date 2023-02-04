using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class WastesEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Wastes;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_WASTES");
		// }
	}
}