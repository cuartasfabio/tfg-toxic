using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
    public class ForestEntity: ITileEntity
    {
        public TileType GetCardId()
        {
            return TileType.Forest;
        }

        // public string GetCardName()
        // {
        //     return StringBank.GetStringRaw("CARD_FOREST");
        // }
    }
}