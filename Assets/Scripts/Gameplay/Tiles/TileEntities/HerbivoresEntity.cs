using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
    public class HerbivoresEntity: ITileEntity
    {
        public TileType GetCardId()
        {
            return TileType.Herbivores;
        }

        // public string GetCardName()
        // {
        //     return StringBank.GetStringRaw("CARD_HERBIVORES");
        // }
    }
}