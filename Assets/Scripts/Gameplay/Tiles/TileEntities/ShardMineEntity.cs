using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
	public class ShardMineEntity: ITileEntity
	{
		public TileType GetCardId()
		{
			return TileType.ShardMine;
		}

		// public string GetCardName()
		// {
		// 	return StringBank.GetStringRaw("CARD_SHARDMINE");
		// }
	}
}