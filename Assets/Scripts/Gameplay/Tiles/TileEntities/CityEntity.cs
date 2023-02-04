using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class CityEntity : ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.City;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_CITY");
		// }
	}
}