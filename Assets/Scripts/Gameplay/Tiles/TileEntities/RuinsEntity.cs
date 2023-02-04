using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
    public class RuinsEntity: ITileEntity
    {
        public TileType GetCardId()
        {
            return TileType.Ruins;
        }

        // public string GetCardName()
        // {
        //     return StringBank.GetStringRaw("CARD_RUINS");
        // }
    }
}