using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class DunesEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Dunes;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_DUNES");
		// }
	}
}