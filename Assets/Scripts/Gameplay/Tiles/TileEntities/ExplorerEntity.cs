using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class ExplorerEntity : ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Explorer;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_EXPLORER");
		// }
	}
}