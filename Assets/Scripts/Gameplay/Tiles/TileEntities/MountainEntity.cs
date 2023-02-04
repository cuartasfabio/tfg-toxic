using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class MountainEntity : ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Mountain;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_MOUNTAIN");
		// }
	}
}