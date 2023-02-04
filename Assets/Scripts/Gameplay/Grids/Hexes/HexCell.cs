using System.Collections.Generic;
using Gameplay.Grids.Hexes.HexHelpers;

namespace Gameplay.Grids.Hexes
{
    public class HexCell
    {
        public HexCoordinates Coordinates { get; private set; }
        public HexCell[] Neighbors { get; private set; }

        public HexCell(HexCoordinates coord)
        {
            Neighbors = new HexCell[6];
            Coordinates = coord;
        }

        public HexCell GetNeighbor(HexDirection direction)
        {
            return Neighbors[(int) direction];
        }

        public void SetNeighbor(HexDirection direction, HexCell cell)
        {
            Neighbors[(int) direction] = cell;
            cell.Neighbors[(int) direction.Opposite()] = this;
        }
        
        public void RemoveNeighbor(HexDirection direction)
        {
            Neighbors[(int) direction].Neighbors[(int) direction.Opposite()] = null;
            Neighbors[(int) direction] = null;
        }
        
        /*public void DeleteSelf()
        {
            Destroy(gameObject);
        }*/
        
        public float SortHeuristic(HexCell origin, int numberOfCells, List<HexCell> res)
        {
            return HexFunctions.GetNumberOfNullNeighbors(this) +
                   (numberOfCells - HexFunctions.DistanceFromCoord(Coordinates, origin.Coordinates)) * 1.5f +
                   HexFunctions.NumberOfNeighborsInList(res, this) * 0.2f;
        }
    }
}