using System.Collections.Generic;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;
using UnityEngine;

namespace Gameplay.TileCreations.Formations
{
    public static class AdjacencySpritePicker
    {
        private static TileType _type;
        private static AdjacencySpritesData _variants;
        // private static HexGrid _grid;
        // private static HexCell _cell;
        private static TileBehaviour _behaviour; 

        public static void PickSpriteForTile(TileBehaviour behaviour, TileType type, AdjacencySpritesData variants)
        {
            _behaviour = behaviour;
            _type = type;
            _variants = variants;
            // _grid = ObjectCache.Current.HexGrid;
            
            HexCoordinates coords = HexCoordinates.ToCoordinates(behaviour.transform.position);
            
            // _cell = _grid.Lists.CoordinatesCells[coords].Cell;

            // Recalcula en tile colocada
            UpdateVariationAndRotation(coords);
            // Recalcula en sus vecinos
            RecalculateNeighbourTilesSprites(coords, _type, variants);
        }
        
        public static void RecalculateNeighbourTilesSprites(HexCoordinates coords, TileType type, AdjacencySpritesData variants)
        {
            _variants = variants;
            _type = type;
            // _cell = _grid.Lists.CoordinatesCells[coords].Cell;
            // _grid = ObjectCache.Current.HexGrid;

            // Recalcula en vecinos de la tile
            List<HexCoordinates> neighbors = HexFunctions.GetNeighborsOfType(coords, new []{_type});
            for (int i = 0; i < neighbors.Count; i++)
            {
                // _cell = _grid.Lists.CoordinatesCells[neighbors[i]].Cell;
                _behaviour = ObjectCache.Current.HexGrid.Lists.CoordinatesBehaviours[neighbors[i]];
            
                UpdateVariationAndRotation(neighbors[i]);
            }
        }

        private static void UpdateVariationAndRotation(HexCoordinates coords)
        {
            int[] variant = GetVariantAndRotation(coords); 
            
            // Sprite sprite = _variants.Variations[variant[0]];
            Sprite sprite = _variants.Variations[variant[0] * HexMetrics.AdjacencyMatrix[1].Length + variant[1]];
            /*
             * Para poder ahorrarme los 2 getChild() tendría que tener: CardBehaviour --> Tile
             * Sería un GetComponentInChild<Tile>() en Init
             * ¿Quiero tener ese acoplamiento?
             */
            Transform tileBase = _behaviour.transform.GetChild(0).GetChild(0);
            MeshRenderer mr = tileBase.GetComponent<MeshRenderer>();
            mr.material.mainTexture = sprite.texture;
            // sr.material = _variants.Material;
            // tileBase.rotation = Quaternion.Euler(90,0,0);
            //tileBase.RotateAround(tileBase.position ,Vector3.up, variant[1]);
        }
        
        /*
         * Analiza cell, codificando sus vecinos de tipo type y escoge un sprite de la lista variants
         */
        private static int[] GetVariantAndRotation(HexCoordinates coords)
        {
            int codedValue = CodifyCellNeighbours(coords);
            
            // todo muy mejorable
            for (int i = 0; i < HexMetrics.AdjacencyMatrix.Length; i++)
            {
                for (int j = 0; j < HexMetrics.AdjacencyMatrix[i].Length; j++)
                {
                    if (HexMetrics.AdjacencyMatrix[i][j] == codedValue)
                        return new[] {i, j}; 
                }
            }
            
            return new[] {0, 0};
        }

        /*
         * Codifica el array de vecinos de la celda según el type (0 != type, 1 == type) en un nº binario
         * y lo devuelve en decimal
         */
        private static int CodifyCellNeighbours(HexCoordinates coords)
        {
            List<HexCoordinates> neighbors = HexFunctions.GetNeighborCoords(coords);
            
            string binCode = "";
            for (int i = 0; i < neighbors.Count; i++)
            {
                // HexCell neighbor = _cell.Neighbors[i];
                
                // if (neighbor != null)
                // {
                if (ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(neighbors[i]) == _type)
                {
                    binCode += "1";
                }
                else
                {
                    binCode += "0";
                }
                     
                // }
                // else binCode += "0";
            }
            return System.Convert.ToInt32(binCode, 2);
        }
    }
}