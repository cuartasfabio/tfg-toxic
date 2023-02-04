using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class LumberjackEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Lumberjack;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_LUMBERJACK");
		// }
	}
}