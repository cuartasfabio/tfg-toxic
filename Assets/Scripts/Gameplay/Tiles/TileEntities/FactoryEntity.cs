using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class FactoryEntity : ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Factory;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_FACTORY");
		// }
	}
}