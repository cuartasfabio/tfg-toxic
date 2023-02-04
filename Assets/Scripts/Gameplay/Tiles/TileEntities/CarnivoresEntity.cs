using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
    public class CarnivoresEntity: ITileEntity
    {
        
        public TileType GetCardId()
        {
            return TileType.Carnivores;
        }

        // public string GetCardName()
        // {
        //     return StringBank.GetStringRaw("CARD_CARNIVORES");
        // }
    }
}