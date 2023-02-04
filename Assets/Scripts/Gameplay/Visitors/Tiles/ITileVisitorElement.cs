namespace Gameplay.Visitors.Tiles
{
    /// <summary>
    /// Visitor Element interface for the TileBehaviour hierarchy.
    /// </summary>
    public interface ITileVisitorElement
    {
        void Accept(AbstractTileVisitor tileVisitor);
    }
}