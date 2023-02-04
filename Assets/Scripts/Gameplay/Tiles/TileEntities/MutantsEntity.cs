using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class MutantsEntity : ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.Mutants;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_MUTANTS");
		// }
	}
}