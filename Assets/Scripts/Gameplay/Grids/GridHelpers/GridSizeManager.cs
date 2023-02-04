using System.Collections.Generic;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using UnityEngine;

namespace Gameplay.Grids.GridHelpers
{
    public class GridSizeManager: MonoBehaviour
    {
        private HexGrid _grid;
        
        private GridFrontiers _frontiers;
        public GridDiscoveryRate GridDiscoveryRate { get; private set; }

        public void Init(int numberOfCells, float discoveryRate)
        {
            _grid = ObjectCache.Current.HexGrid;
            GridDiscoveryRate = new GridDiscoveryRate(0.7f + discoveryRate);
            
            if (_frontiers == null)
                _frontiers = new GridFrontiers();

            _frontiers.Init();

            List<HexCoordinates> toDiscover = GetCellsToDiscover(numberOfCells, new HexCoordinates(0, 0));
            for (int i = 0; i < toDiscover.Count; i++)
                DiscoverTile(toDiscover[i]);
        }

        /// <summary>
        /// Discover the tile at a specific coords.
        /// </summary>
        /// <param name="coords">HexCoordinates to discover.</param>
        public void DiscoverTile(HexCoordinates coords)
        {
            _frontiers.UpdateFrontiers(coords);
            
            TileType type = ObjectCache.Current.TerrainBuilder.GetTerrainTileForCoords(coords);
            
            _grid.PlaceTypeAtCoords(type, coords);
            
            _grid.Lists.CoordinatesBehaviours[coords].RunPlaceAnimation();
        }
        
        /// <summary>
        /// Picks a list of HexCoordinates to be discovered.
        /// </summary>
        /// <param name="numberOfCells">How many cells to discover.</param>
        /// <param name="origin">Start cell to pick from.</param>
        /// <returns></returns>
        public List<HexCoordinates> GetCellsToDiscover(int numberOfCells, HexCoordinates origin)
        {
            List<HexCoordinates> cellCoordsToCreate = new List<HexCoordinates>();
            
            // se podria sacar a una clase UniqueQueue o parecido
            HashSet<HexCoordinates> addedCoords = new HashSet<HexCoordinates>();
            Queue<HexCoordinates> coordQueue = new Queue<HexCoordinates>();
            
            addedCoords.Add(origin);
            coordQueue.Enqueue(origin);

            List<HexCoordinates> firstNeighbors = HexFunctions.GetNullNeighborsCoordinates(origin);
            
            for (int i = 0; i < firstNeighbors.Count; i++)
            {
                addedCoords.Add(firstNeighbors[i]);
                coordQueue.Enqueue(firstNeighbors[i]);
            }
            
            for (int i = 0; i < numberOfCells; i++) 
            {
                // controlar que haya suficiente huecos para las nuevas celdas
                if (coordQueue.Count < 1)
                   break;
                // sacar coord de queue
                HexCoordinates newCoord = coordQueue.Dequeue();
                // crear nueva celda
                cellCoordsToCreate.Add(newCoord);
                // añadir coords a queue
                List<HexCoordinates> neighbors = HexFunctions.GetNullNeighborsCoordinates(newCoord);
                for (int j = 0; j < neighbors.Count; j++)
                {
                    if (!addedCoords.Contains(neighbors[j]))
                    {
                        addedCoords.Add(neighbors[j]);
                        coordQueue.Enqueue(neighbors[j]);
                    }
                }
            }
            return cellCoordsToCreate;
        }
        
    }
}