using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
    public class CampsiteEntity: ITileEntity
    {
        public TileType GetCardId()
        {
            return TileType.Campsite;
        }

        // public string GetCardName()
        // {
        //     return StringBank.GetStringRaw("CARD_CAMPSITE");
        // }
    }
}