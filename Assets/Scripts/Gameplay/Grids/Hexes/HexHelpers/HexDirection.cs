namespace Gameplay.Grids.Hexes.HexHelpers
{
    public enum HexDirection
    {
        NE, E, SE ,SW, W, NW // vecinos en sentido agujas reloj
    }

    public static class HexDirectionExtensions
    {
        public static HexDirection Opposite(this HexDirection direction)
        {
            return (int) direction < 3 ? (direction + 3) : (direction - 3);
        }
    }
}