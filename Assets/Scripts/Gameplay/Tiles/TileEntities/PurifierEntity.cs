using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class PurifierEntity : ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Purifier;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_PURIFIER");
		// }
	}
}