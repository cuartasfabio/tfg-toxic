using System.Collections.Generic;
using Backend.Persistence;
using Gameplay.Grids.GridHelpers;
using Gameplay.Grids.Hexes;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;
using Gameplay.Visitors.Tiles;
using UnityEngine;

namespace Gameplay.Grids
{
    public class HexGrid: MonoBehaviour, IPersistable
    {
        public GridLists Lists { get; private set; }
        public GridBounds GridBounds { get; private set; }
        
        public void Init()
        {
            GridBounds = new GridBounds();
            
            if (Lists == null)
                Lists = new GridLists();

            Lists.Init();
            
        }
        
        public void PlaceTypeAtCoords(TileType type, HexCoordinates coords)
        {
            // Creates the behaviour
            TileBehaviour beh =
                ObjectCache.Current.TileBehaviourPool.GetNewTile(type, HexCoordinates.ToPosition(coords));
            
           PlaceBehaviourAtCoords(beh, coords);
        }
        
        public void PlaceBehaviourAtCoords(TileBehaviour beh, HexCoordinates coords)
        {
            HexCell cell = Lists.GetCellForCoord(coords);

            if (cell == null)
                CreateCell(coords);

            ReplaceBehaviour(beh, coords);
        }
        
        
        public void CreateCell(HexCoordinates coords)
        {
            HexCell cell = new HexCell(coords);
            
            Lists.RegisterNewCell(cell);
            
            GridBounds.UpdateBounds(coords);
            
            SetCellNeighbors(cell);
            
        }
        
        private void SetCellNeighbors(HexCell cell)
        {
            List<HexCoordinates> neighbors = HexFunctions.GetNeighborCoords(cell.Coordinates);
            
            for (int i = 0; i < neighbors.Count; i++) 
            {
                if (Lists.GetCellForCoord(neighbors[i]) != null)
                {
                    HexCell neighbor = Lists.GetCellForCoord(neighbors[i]);
                    cell.SetNeighbor((HexDirection)i, neighbor);
                }
            }  
        }
        
        private void ReplaceBehaviour(TileBehaviour behaviour, HexCoordinates coords)
        {
            Transform tfm = behaviour.transform;
            
            tfm.SetParent(ObjectCache.Current.DynamicCardBehaviours, false);
            tfm.position = HexCoordinates.ToPosition(coords);
            
            // todo provisional aquí
            if(Lists.CoordinatesBehaviours.ContainsKey(coords))
                new OnDeleteVisitor().GetBehaviourCommands(Lists.CoordinatesBehaviours[coords]);
            
            Lists.ReplaceBehaviour(coords, behaviour);
            
            behaviour.SetCurrentCoordinates(coords);
        }
        
        // -------------------------------------------------------------------------------------------------------------

        public void Save(GameDataWriter writer)
        {
           List<(HexCoordinates, TileType)> l = Lists.ListCurrentTiles();
           
           writer.Write(l.Count);

           for (int i = 0; i < l.Count; i++)
           {
               writer.Write(l[i].Item1);
               writer.Write((int)l[i].Item2);
           }
        }

        public void Load(GameDataReader reader)
        {
            int tileNum = reader.ReadInt();

            for (int i = 0; i < tileNum; i++)
            {
                HexCoordinates coords = reader.ReadHexCoords();
                TileType type = (TileType) reader.ReadInt();

                if (type != TileType.None)
                    PlaceTypeAtCoords(type, coords);
            }
        }
    }
}