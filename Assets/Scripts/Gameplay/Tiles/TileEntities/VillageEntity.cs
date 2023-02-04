using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class VillageEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Village;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_VILLAGE");
		// }
	}
}