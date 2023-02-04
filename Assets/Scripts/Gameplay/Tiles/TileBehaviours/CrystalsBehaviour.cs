using Gameplay.Visitors.Tiles;

namespace Gameplay.Tiles.TileBehaviours
{
    public class CrystalsBehaviour: TileBehaviour
    {
        public override void Accept(AbstractTileVisitor tileVisitor)
        {
            tileVisitor.Visit(this);
        }
    }
}