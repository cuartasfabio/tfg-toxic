using System.Collections.Generic;
using Gameplay.Grids.Hexes;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;
using Utils;

namespace Gameplay.Grids
{
    public class GridLists
    {
        private struct CoordContents
        {
            public HexCell Cell;
            public TileType CellType;
        }

        public List<HexCell> CellList { get; private set; }
        
        private Dictionary<HexCoordinates, CoordContents> _coordsCells;
        public Dictionary<HexCoordinates, TileBehaviour> CoordinatesBehaviours { get; private set; } 
        public Dictionary<TileBehaviour, HexCoordinates> BehaviourCoordinates { get; private set; }  
        
        private List<HexCell>[] _cellsByCardType;

        // private HashSet<HexCoordinates> _hashBorders; 
        // private HashSet<HexCell> _hashBorders;
        // private HashSet<HexCell> _cellsWithBorder;
        
        // list of behaviours subscribed to OnTurnEndVisitor
        public List<TileBehaviour> TilesWithTurnEnd { get; private set; } 

        public void Init()
        {
            if (CellList != null)
            {
                while (CellList.Count > 0)
                {
                    RemoveCell(CellList[0]);
                }
            }
            CellList = new List<HexCell>();
            _coordsCells = new Dictionary<HexCoordinates, CoordContents>();
            CoordinatesBehaviours = new Dictionary<HexCoordinates, TileBehaviour>();
            BehaviourCoordinates = new Dictionary<TileBehaviour, HexCoordinates>();

            _cellsByCardType = new List<HexCell>[TileTypes.TileTypeCount];
            for (int i = 0; i < TileTypes.TileTypeCount; i++)
            {
                _cellsByCardType[i] = new List<HexCell>();
            }

            // _hashBorders = new HashSet<HexCoordinates>();
            // _hashBorders = new HashSet<HexCell>();
            // _cellsWithBorder = new HashSet<HexCell>();

            TilesWithTurnEnd = new List<TileBehaviour>();
        }

        public TileType GetTypeForCoord(HexCoordinates coords)
        {
            if (_coordsCells.ContainsKey(coords))
                return _coordsCells[coords].CellType;
            return TileType.None;
        }

        public HexCell GetCellForCoord(HexCoordinates coords)
        {
            if (_coordsCells.ContainsKey(coords))
                return _coordsCells[coords].Cell;
            return null;
        }
        
        public List<HexCell> GetCellsOfType(TileType type)
        {
            return _cellsByCardType[(int) type - 1];
        }

        public List<HexCell> GetCellsOfTypes(TileType[] types)
        {
            List<HexCell> res = new List<HexCell>();

            for (int i = 0; i < types.Length; i++)
            {
                List<HexCell> ofTypei = GetCellsOfType(types[i]);
                for (int j = 0; j < ofTypei.Count; j++)
                {
                    res.Add(ofTypei[j]);
                }
            }

            return res;
        }

        public void RegisterNewCell(HexCell cell)
        {
            CellList.Add(cell);
            
            CoordContents cc = new CoordContents() 
            {
                Cell = cell,
                CellType = TileType.None
            };
            
            _coordsCells.Add(cell.Coordinates, cc);
        }
        
        public void ReplaceBehaviour(HexCoordinates coords, TileBehaviour behaviour)
        {
            RemoveBehaviour(coords);
            RegisterNewPlacedBehaviour(coords, behaviour);
        }
        
        private void RemoveBehaviour(HexCoordinates coords)
        {
            if (CoordinatesBehaviours.ContainsKey(coords))
            {
                _cellsByCardType[(int)CoordinatesBehaviours[coords].TileEntity.GetCardId() -1].Remove(_coordsCells[coords].Cell);
                BehaviourCoordinates.Remove(CoordinatesBehaviours[coords]);
                TilesWithTurnEnd.Remove(CoordinatesBehaviours[coords]);
                CoordinatesBehaviours[coords].DeleteSelf();
                CoordinatesBehaviours.Remove(coords);
            }
        }
        
        private void RegisterNewPlacedBehaviour(HexCoordinates coords, TileBehaviour behaviour)
        {
            CoordinatesBehaviours.Add(coords,behaviour);
            BehaviourCoordinates.Add(behaviour,coords);
            _cellsByCardType[(int)behaviour.TileEntity.GetCardId() -1].Add(_coordsCells[coords].Cell);
            
            CoordContents cc = new CoordContents() 
            {
                Cell = _coordsCells[coords].Cell,
                CellType = behaviour.TileEntity.GetCardId()
            };

            _coordsCells[coords] = cc;
        }

        private void RemoveCell(HexCell cell)
        {
            HexCoordinates coords = cell.Coordinates;
            CellList.Remove(cell);
            
            // if(_hashBorders.Remove(cell)) _cellsWithBorder.Remove(cell);
            
            RemoveBehaviour(coords);
            
            if (_coordsCells.ContainsKey(coords))
                _coordsCells.Remove(coords);
        }

        
        // Es necesaria este lista de cellsWithBorder??? al colocar las frontier
        // estas SIEMPRE serían las cells con borde //todo algunas Frontier NO tienen border eh!
        // public void UpdateCellWithBorder(HexCell cell)
        // {
        //     int borders = HexFunctions.GetNumberOfNullNeighbors(cell);
        //
        //     if (borders <= 0) // si ya no tiene bordes y estaba en la lista => fuera
        //     {
        //         // if(_cellsWithBorder.Contains(cell)) _cellsWithBorder.Remove(cell);
        //     }
        //     else
        //     {
        //         // si tiene bordes y no estaba en la lista => pa dentro
        //         // if (!_cellsWithBorder.Contains(cell)) _cellsWithBorder.Add(cell);
        //     }
        // }


        public List<(HexCoordinates, TileType)> ListCurrentTiles()
        {
            List<(HexCoordinates, TileType)> res = new List<(HexCoordinates, TileType)>();
            foreach (var tile in _coordsCells)
            {
                res.Add((tile.Key, tile.Value.CellType));
            }
            return res;
        }

        public HexCoordinates? GetRandomCoordOfType(TileType type)
        {
            List<HexCell> cells = GetCellsOfType(type);

            if (cells.Count == 0)
                return null;

            return cells[RandomTf.Rng.Next(cells.Count)].Coordinates;
        }
        
        public HexCoordinates? GetRandomCoordOfTypes(TileType[] types)
        {
            List<HexCell> cells = new List<HexCell>();
            for (int i = 0; i < types.Length; i++)
            {
               cells.AddRange(GetCellsOfType(types[i])); 
            }

            if (cells.Count == 0)
                return null;

            return cells[RandomTf.Rng.Next(cells.Count)].Coordinates;
        }
    }
}