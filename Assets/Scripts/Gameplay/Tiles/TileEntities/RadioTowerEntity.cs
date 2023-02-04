using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class RadioTowerEntity : ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.RadioTower;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_RADIOTOWER");
		// }
	}
}