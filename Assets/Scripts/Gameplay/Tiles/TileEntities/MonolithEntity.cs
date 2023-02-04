using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class MonolithEntity : ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Monolith;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_MONOLITH");
		// }
	}
}