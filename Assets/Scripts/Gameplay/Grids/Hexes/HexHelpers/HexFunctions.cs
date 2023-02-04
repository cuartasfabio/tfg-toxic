using System.Collections.Generic;
using Gameplay.Tiles;
using UnityEngine;

namespace Gameplay.Grids.Hexes.HexHelpers
{
    public static class HexFunctions
    {
        // private static readonly Random _random = new Random();
        
        /*
         * Conversiones de coordenadas --------------------------------------------------------------------------------
         * 
         */
        
        /* Transforma coordenadas globales en HexCoordinates */
        public static HexCoordinates GetCoordinatesFromPosition(Vector3 position)
        {
            Vector3 res = ObjectCache.Current.HexGrid.transform.InverseTransformPoint(position);
            return HexCoordinates.ToCoordinates(res);
        }
        
        /* Redondea unas coordenadas globales al centro de la celda más próxima */
        // public static Vector3 SnapToGrid(Vector3 position)
        // {
        //     return HexCoordinates.ToPosition(GetCoordinatesFromPosition(position));
        // }
        public static Vector3 SnapToGrid(HexCoordinates coords)
        {
            return HexCoordinates.ToPosition(coords);
        }
        
        /*
         * Vecinos --------------------------------------------------------------------------------
         * 
         */
        
        /* */
        public static List<HexCoordinates> GetNeighborsInAdjacency(List<HexCoordinates> cellNeighbors) // todo revisar
        {
            List<HexCoordinates> res = new List<HexCoordinates>();
            
            for (int i = 0; i < cellNeighbors.Count; i++)
            {
                HexCell cell = ObjectCache.Current.HexGrid.Lists.GetCellForCoord(cellNeighbors[i]);
                List<HexCell> nns = GetNotNullNeighbors(cell);
                // si entre nns esta algun vecino de neighbors
                for (int j = 0; j < cellNeighbors.Count; j++)
                    if (i != j)
                        for (int k = 0; k < nns.Count; k++)
                            if (nns[k].Coordinates.Equals(cellNeighbors[j]))
                            {
                                res.Add(cellNeighbors[i]);
                                res.Add(cellNeighbors[j]);
                                return res;
                            }
            }
            return res;
        }

        /*Recibe una HEXCELL, escoge aleatoriamente uno de sus coordenadas vecinas LIBRES
         y devuelve las HexCoordinates ABSOLUTAS en de esa coordenada vecina (con centro 0,0,0)*/
        // public static HexCoordinates GetRandomNullNeighborsCoordinates(HexCoordinates coords)
        // {
        //     List<HexCoordinates> res = GetNullNeighborsCoordinates(coords);
        //     
        //     return res[Random.Range(0, res.Count)];
        // }
        
         /* Devuelve una lista con las HexCoordinates LIBRES de sus vecinos  */
        public static List<HexCoordinates> GetNullNeighborsCoordinates(HexCoordinates coords)
        {
            List<HexCoordinates> res = new List<HexCoordinates>();
            
            for (int i = 0; i < 6; i++)
            {
                HexCoordinates neighbor = AddCoordinates(coords, HexMetrics.CellNeighbors[i]);
                if (ObjectCache.Current.HexGrid.Lists.GetCellForCoord(neighbor) == null)
                {
                    res.Add(neighbor);
                } else if (ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(neighbor) == TileType.Frontier)
                {
                    res.Add(neighbor);
                }
            }
            
            return res;
        }
        
        /* Devuelve las HexCoordinates de vecinos no null */
        // public static List<HexCoordinates> GetNotNullNeighbors(HexCoordinates coords)
        // {
        //     List<HexCoordinates> res = new List<HexCoordinates>();
        //     
        //     HexCell cell = ObjectCache.Current.HexGrid.Lists.GetCellForCoord(coords);
        //     
        //     List<HexCell> notNulls = GetNotNullNeighbors(cell);
        //     for (int i = 0; i < notNulls.Count; i++)
        //     {
        //         res.Add(notNulls[i].Coordinates);
        //     }
        //     return res;
        // }
        
        /* Devuelve las HexCells vecinas no null */
        public static List<HexCell> GetNotNullNeighbors(HexCell cell)
        {
            //HexCell cell = ObjectCache.Current.HexGrid.Lists.CoordinatesCells[coords].Cell;

            List<HexCell> notNulls = new List<HexCell>();
            
            for (int i = 0; i < 6; i++)
            {
                if (cell.Neighbors[i] != null)
                {
                    notNulls.Add(cell.Neighbors[i]);
                }
            }
            return notNulls;
        }
        
        /* Devuelve el número de HexCoordinates vecinas que son NULL */
        public static int GetNumberOfNullNeighbors(HexCell cell)
        {
            return 6 - GetNumberOfNotNullNeighbors(cell);
        }
        
        /* Devuelve el número de HexCoordinates vecinas que NO SON NULL */
        public static int GetNumberOfNotNullNeighbors(HexCell cell)
        {
            return GetNotNullNeighbors(cell).Count;
        }

        /* Devuelve las HexCoordinates de los vecinos */
        public static List<HexCoordinates> GetNeighborCoords(HexCoordinates coords)
        {
            List<HexCoordinates> res = new List<HexCoordinates>();
            HexCoordinates[] neighborCoords = HexMetrics.CellNeighbors;
            
            for (int i = 0; i < neighborCoords.Length; i++)
                res.Add(AddCoordinates(coords, neighborCoords[i]));
            
            return res;
        }
        
        /* Aplica un Algoritmo breadth-first search para explorar el grafo y devolver numberOfCells celdas */
        /*public static List<HexCoordinates> BFSGetNeighbors(HexCoordinates origin, int numberOfCells, List<TileType> types) 
        {
            HexCell originCell = ObjectCache.Current.HexGrid.Lists.CoordinatesCells[origin].Cell;
            
            List<HexCoordinates> res = new List<HexCoordinates>();
            
            List<HexCell> visitedList = new List<HexCell>();
            HashSet<HexCell> visited = new HashSet<HexCell>();
            
            List<HexCell> queue = new List<HexCell>();
            queue.Add(originCell);

            while (visited.Count < numberOfCells && queue.Count > 0)
            {
                HexCell hexCell = queue.OrderBy(c => c.SortHeuristic(originCell,numberOfCells,visitedList)).Last(); 
                queue.Remove(hexCell);

                if (visited.Contains(hexCell))
                    continue;
                
                visited.Add(hexCell);
                visitedList.Add(hexCell); 
                res.Add(hexCell.Coordinates);

                for (int i = 0; i < 6; i++)
                {
                    if (hexCell.Neighbors[i] != null && !visited.Contains(hexCell.Neighbors[i]))
                        if (!queue.Contains(hexCell.Neighbors[i]))
                        {
                            if (types.Contains(ObjectCache.Current.HexGrid.Lists.CoordinatesCells[hexCell.Neighbors[i].Coordinates].CellType))
                                queue.Add(hexCell.Neighbors[i]);
                        }
                }
            }
            // ahora visited tiene los numberOfCell vecinos sin ordenar
            
            return res;
        }*/

        /*
         * Algoritmos grid --------------------------------------------------------------------------------
         * 
         */
        
        /* Distancia (en celdas) entre dos celdas */
        public static int DistanceFromCoord(HexCoordinates fromCoord, HexCoordinates toCoord)
        {
            int res = (Mathf.Abs(toCoord.X - fromCoord.X) 
                       + Mathf.Abs(toCoord.Y - fromCoord.Y) 
                       + Mathf.Abs(toCoord.Z - fromCoord.Z)) / 2;
            return res;
        }
        
        /* Devuelve TODAS las celdas en un rango (radius) al rededor de center */
        public static List<HexCoordinates> CellsInRadius(HexCoordinates center, int radius)
        {
            List<HexCoordinates> res = new List<HexCoordinates>();

            for (int x = -radius; x <= radius; x++)
            {
                for (int z = Mathf.Max(-radius, -x-radius); z <= Mathf.Min(radius, -x+radius); z++)
                {
                    res.Add(AddCoordinates(center, new HexCoordinates(x,z)));
                }
            }
            return res;
        }
        
        // public static List<HexCoordinates> CellsToDiscoverInRadius(HexCoordinates center, int radius)
        // {
        //     List<HexCoordinates> res = new List<HexCoordinates>();
        //     List<HexCoordinates> inRadius = CellsInRadius(center,radius);
        //
        //     for (int i = 0; i < inRadius.Count; i++)
        //     {
        //         if (ObjectCache.Current.HexGrid.Lists.GetCellForCoord(inRadius[i]) == null)
        //            res.Add(inRadius[i]);
        //     }
        //     
        //     
        //     return res;
        // }

        /* Devuele (uno) de los caminos más cortos (lista de celdas) que unen dos celdas */
        // public static List<HexCell> PathBetweenCells(HexCell startCell, HexCell endCell)
        // {
        //     return new List<HexCell>(); 
        // }
        
        /* Devuele la línea de hexagonos entre cell1 y cell2 */
        // public static List<HexCoordinates> LineBetweenCells(HexCoordinates cell1, HexCoordinates cell2)
        // {
        //     List<HexCoordinates> res = new List<HexCoordinates>();
        //
        //     int n = DistanceFromCoord(cell1, cell2);
        //
        //     double step = 1.0f / Mathf.Max(n, 1);
        //
        //     for (int i = 0; i <= n; i++)
        //     {
        //         // res.Add( hex_round(HexLerp(a_nudge, b_nudge, step * i)) );
        //     }
        //     return res; 
        // }
        // private static float Lerp(float a, float b, float t)
        // {
        //     return a * (1 - t) + b * t;
        // }
        // private static Vector3 HexLerp(HexCoordinates a, HexCoordinates b, float t)
        // {
        //     return new Vector3( Lerp(a.X, b.X, t),
        //                         Lerp(a.Y, b.Y, t),
        //                         Lerp(a.Z, b.Z, t));
        // }
        
        /*
         * Operaciones auxiliares --------------------------------------------------------------------------------
         * 
         */
        
        /* Suma dos HexCoordinates */
        private static HexCoordinates AddCoordinates(HexCoordinates coord1, HexCoordinates coord2)
        {
            return new HexCoordinates(coord1.X + coord2.X, coord1.Z + coord2.Z);    
        }

        /*public static HexCoordinates GetGridOuterCoordNextToCoord(HexGrid grid, HexCoordinates coords) 
        {
            List<HexCell> possibleCoords = new List<HexCell>();

            for(int i = 0; i < grid.Lists.CellList.Count; i++)
            {
                if (GetNumberOfNullNeighbors(grid.Lists.CellList[i]) > 0)
                    possibleCoords.Add(grid.Lists.CellList[i]);
            }
            
            HexCell closestCell = possibleCoords[_random.Next(possibleCoords.Count)];
            int bestDistance = DistanceFromCoord(coords, closestCell.Coordinates);
            
            for (int i = 0; i < possibleCoords.Count; i++)
            {
                if (bestDistance < 2) break;
                
                int distance = DistanceFromCoord(coords, possibleCoords[i].Coordinates);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    closestCell = possibleCoords[i];
                }
            }

            return GetRandomNullNeighborsCoordinates(closestCell);
        }*/

        /* Devuelve cuantas celdas de la lista son vecinas de cell (usado en CellHeuristic) */
        public static int NumberOfNeighborsInList(List<HexCell> cellList, HexCell cell)
        {
            int res = 0;

            for (int i = 0; i < 6; i++)
            {
                if (cellList.Contains(cell.Neighbors[i]))
                    res++;
            }

            return res;
        }

        public static List<HexCoordinates> GetNeighborsOfType(HexCoordinates coords, TileType[] types)
        {
            return GetTilesOfTypeInRadius(coords, types, 1);
        }

        // public static List<HexCell> FilterByType(List<HexCell> list, TileType[] types)
        // {
        //     List<HexCell> res = new List<HexCell>();
        //
        //     for (int i = 0; i < list.Count; i++)
        //     {
        //         TileType type = ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(list[i].Coordinates);
        //         for (int j = 0; j < types.Length; j++)
        //         {
        //             if (type == types[j])
        //             {
        //                 res.Add(list[i]);
        //                 break;
        //             }
        //         }
        //     }
        //     return res;
        // }
        
        /*
         * Operaciones auxiliares usadas en visitor --------------------------------------------------------------------------------
         * 
         */

        public static List<HexCoordinates> GetTilesOfTypeInRadius(HexCoordinates center, TileType[] types, int radius)
        {
            List<HexCoordinates> res = new List<HexCoordinates>();
            
            List<HexCoordinates> coords = CellsInRadius(center, radius);
            
            coords.Remove(center);
            for (int i = 0; i < coords.Count; i++)
            {
                TileType type = ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(coords[i]);
                for (int j = 0; j < types.Length; j++)
                {
                    if (type == types[j])
                        res.Add(coords[i]);
                }
            }

            return res;
        }

        // public static bool HasSameNumOfNeighborsOfType(HexCoordinates origin, TileType toPlace, TileType type1, TileType type2)
        // {
        //     int count1 = GetNeighborsOfType(origin, new []{type1}).Count;
        //     int count2 = GetNeighborsOfType(origin, new []{type2}).Count;
        //
        //     if (toPlace.Equals(type1)) count1++;
        //     if (toPlace.Equals(type2)) count2++;
        //
        //     return count1 == count2 && count1 > 0;
        // }

        public static HexCoordinates? GetRandomOuterCellInRadius(HexCoordinates origin, int radius)
        {
            List<HexCoordinates> coordsInRadiusToDiscover = CellsInRadius(origin, radius); // celdas al rededor del behaviour
            List<HexCoordinates> outCoords = new List<HexCoordinates>();
            
            for (int i = 0; i < coordsInRadiusToDiscover.Count; i++) // guardar las celdas que estén FUERA de la grid
                if (ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(coordsInRadiusToDiscover[i]) == TileType.None)
                    outCoords.Add(coordsInRadiusToDiscover[i]);
            
            if (outCoords.Count > 0) // si hay alguna celda FUERA
                return outCoords[Random.Range(0,outCoords.Count)]; // escoger una random de estas
            
            return null;
        }

        // public static List<HexCoordinates> GetRandomOuterCoordsAtGridBorders(int amount)
        // {
        //     List<HexCell> borders = ObjectCache.Current.HexGrid.Lists.GetCellsWithBorder();
        //     List<HexCoordinates> res = new List<HexCoordinates>();
        //     for (int i = 0; i < amount; i++)
        //         res.Add(GetRandomNullNeighborsCoordinates(borders[Random.Range(0,borders.Count)].Coordinates)); 
        //     return res;
        // }

        // public static void DiscoverRandomCellsAtRandomBorder(int minTiles, int maxTiles)
        // {
        //     DiscoverCellsAtRandomBorder(Random.Range(minTiles,maxTiles + 1));
        // }
        
        // private static void DiscoverCellsAtRandomBorder(int amount)
        // {
        //     List<HexCoordinates> origin = GetRandomOuterCoordsAtGridBorders(1);
        //     List<HexCoordinates> cellCoordsToDiscover =
        //         ObjectCache.Current.GridSizeManager.GetCellsToDiscover(amount, origin[0]);
        //     for (int i = 0; i < cellCoordsToDiscover.Count; i++)
        //         ObjectCache.Current.CommandBuffer.EnqueueCommand(new DiscoverTileCommand(cellCoordsToDiscover[i], true,
        //             0.2f));
        // }
        
        // todo para preview de tiles a descubrir
        /*public static List<ICardCommand> DiscoverRandomCellsAtRandomBorder(int minTiles, int maxTiles)
        {
            return DiscoverCellsAtRandomBorder(_random.Next(minTiles,maxTiles));
        }
        
        private static List<ICardCommand> DiscoverCellsAtRandomBorder(int amount)
        {
            List<ICardCommand> res = new List<ICardCommand>();
            
            List<HexCoordinates> origin = GetRandomOuterCoordsAtGridBorders(1);
            List<HexCoordinates> cellCoordsToDiscover =
                ObjectCache.Current.HexGridSizeManager.GetCellsToDiscover(amount, origin[0]);
            for (int i = 0; i < cellCoordsToDiscover.Count; i++)
                res.Add(new DiscoverCellCommand(cellCoordsToDiscover[i], true, 0.2f));
            return res;
        }*/

        // Asume que la celda está en la grid (ocupada)
        /*public static List<HexCoordinates> GetTileGroup(int numberOfCells, HexCoordinates origin, List<TileType> types)
        {
            return BFSGetNeighbors(origin, numberOfCells, types);
        }*/
        
        // public static List<HexCoordinates> GetTilesFromRandomBorder(int amount)
        // {
        //     List<HexCell> borders = ObjectCache.Current.HexGrid.Lists.GetCellsWithBorder();
        //     HexCell originPoint = borders[Random.Range(0,borders.Count)];
        //     return GetTileGroup(amount, originPoint.Coordinates);
        // }
        //
        // public static List<HexCoordinates> GetRandomTilesFromRandomBorder(int min, int max)
        // {
        //     List<HexCell> borders = ObjectCache.Current.HexGrid.Lists.GetCellsWithBorder();
        //     HexCell originPoint = borders[Random.Range(0,borders.Count)];
        //     return GetTileGroup(Random.Range(min,max+1), originPoint.Coordinates);
        // }
        
        /*public static List<HexCoordinates> GetRandomTilesFromRandomBorder(int min, int max, List<TileType> types)
        {
            List<HexCell> borders = ObjectCache.Current.HexGrid.Lists.GetCellsWithBorder();
            HexCell originPoint = borders[Random.Range(0,borders.Count)];
            return GetTileGroup(Random.Range(min,max+1), originPoint.Coordinates, types);
        }*/
    }
}

