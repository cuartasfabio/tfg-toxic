using Backend.Localization;

namespace Gameplay.Tiles.TileEntities
{
    public class GroundEntity: ITileEntity
    {
        public TileType GetCardId()
        {
            return TileType.Ground;
        }

        // public string GetCardName()
        // {
        //     return StringBank.GetStringRaw("CARD_GROUND");
        // }
    }
}