using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
    public class LakeEntity: ITileEntity
    {
        public TileType GetCardId()
        {
            return TileType.Lake;
        }

        // public string GetCardName()
        // {
        //     return StringBank.GetStringRaw("CARD_LAKE");
        // }
    }
}