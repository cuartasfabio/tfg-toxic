using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class FossilsEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Fossils;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_FOSSILS");
		// }
	}
}