using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class FrontierEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Frontier;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_FRONTIER");
		// }
	}
}