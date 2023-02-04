using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
    public class MeadowEntity: ITileEntity
    {
        public TileType GetCardId()
        {
            return TileType.Meadow;
        }

        // public string GetCardName()
        // {
        //     return StringBank.GetStringRaw("CARD_MEADOW");
        // }
    }
}