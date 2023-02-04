using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
    public class CrystalsEntity: ITileEntity
    {
        public TileType GetCardId()
        {
            return TileType.Crystals;
        }

        // public string GetCardName()
        // {
        //     return StringBank.GetStringRaw("CARD_CRYSTALS");
        // }
    }
}