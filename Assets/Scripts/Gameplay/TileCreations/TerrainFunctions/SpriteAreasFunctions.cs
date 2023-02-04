using System.Collections.Generic;

namespace Gameplay.TileCreations.TerrainFunctions
{
    public static class SpriteAreasFunctions
    {
        public static bool IsAreaClear(PropPlacer.PropArea area, List<PropPlacer.PropArea> areasToCheck)
        {
            for (int i = 0; i < areasToCheck.Count; i++)
            {
                if (CircleOverlaps(area, areasToCheck[i]))
                    return false;
            }
            return true;
        }

        private static bool CircleOverlaps(PropPlacer.PropArea area1, PropPlacer.PropArea area2)
        {
            double xDistance = area1.SampleX - area2.SampleX;
            double yDistance = area1.SampleY - area2.SampleY;

            double sumOfRadii = area1.Radius + area2.Radius;
            double distanceSquared = xDistance * xDistance + yDistance * yDistance;

            bool isOverlapping = distanceSquared  < sumOfRadii * sumOfRadii;
            
            return isOverlapping;
        }
    }
}