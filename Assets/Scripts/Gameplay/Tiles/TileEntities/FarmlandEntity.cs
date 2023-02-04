using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class FarmlandEntity : ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Farmland;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_FARMLAND");
		// }
	}
}