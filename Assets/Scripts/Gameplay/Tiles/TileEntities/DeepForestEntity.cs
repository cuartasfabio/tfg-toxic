using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
    public class DeepForestEntity : ITileEntity
    {
        public TileType GetCardId()
        {
            return TileType.DeepForest;
        }

        // public string GetCardName()
        // {
        //     return StringBank.GetStringRaw("CARD_DEEPFOREST");
        // }
    }
}