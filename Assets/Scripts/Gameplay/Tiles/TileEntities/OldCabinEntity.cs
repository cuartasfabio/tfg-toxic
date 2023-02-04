using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class OldCabinEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.OldCabin;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_OLDCABIN");
		// }
	}
}