using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class SwampEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Swamp;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_SWAMP");
		// }
	}
}