using UnityEngine;

namespace Gameplay.Grids.Hexes.HexHelpers
{
    public static class HexMetrics
    {
        public const float OuterRadius = 1.5f;
    
        public const float InnerRadius = OuterRadius * 0.866025404f; // √3 / 2

        public static readonly Vector3[] Corners =
        {
            new Vector3(0f,0f,OuterRadius),
            new Vector3(InnerRadius,0f,0.5f * OuterRadius),
            new Vector3(InnerRadius,0f,-0.5f * OuterRadius),
            new Vector3(0f,0f,-OuterRadius),
            new Vector3(-InnerRadius,0f,-0.5f * OuterRadius),
            new Vector3(-InnerRadius,0f,0.5f * OuterRadius),
            new Vector3(0f,0f,OuterRadius) // vertice extra para evitar indexoutofbounds al crear la mesh
        };
        
        public static readonly HexCoordinates[] CellNeighbors =
        {
            new HexCoordinates(0,1), // clockwise starting NE
            new HexCoordinates(1,0), 
            new HexCoordinates(1,-1), 
            new HexCoordinates(0,-1), 
            new HexCoordinates(-1,0), 
            new HexCoordinates(-1,1)  
        };

        public static readonly int[][] AdjacencyMatrix =
        {
            new[] { 0},
            new[] { 32, 16, 8, 4 ,2, 1},
            new[] { 48, 24, 12, 6, 3, 33},
            new[] { 56, 28, 14, 7, 35, 49},
            new[] { 60, 30, 15, 39, 51, 57},
            new[] { 62, 31, 47, 55, 59, 61},
            new[] { 63},
            new[] { 40, 20, 10, 5, 34, 17},
            new[] { 36, 18, 9},
            new[] { 42, 21},
            new[] { 52, 26, 13, 38, 19, 41},
            new[] { 50, 25, 44, 22, 11, 37},
            new[] { 58, 29, 46, 23, 43, 53},
            new[] { 54, 27, 45}
        };
    }
}

