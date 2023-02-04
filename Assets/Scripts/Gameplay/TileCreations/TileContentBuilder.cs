using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.TileCreations
{
    public class TileContentBuilder
    {
        // Yes, this is a singleton. We'll change it once we revamp the asset loading
        // process to make everything load and generate asynchronously.
        public static TileContentBuilder Get()
        {
            if (_instance == null)
                _instance = new TileContentBuilder();
            return _instance;
        }
        private static TileContentBuilder _instance;


        /// <summary>
        /// A mesh cache, with a generated mesh for every
        /// given sprite size.
        /// </summary>
        private readonly Dictionary<Vector2Int, Mesh> _meshCache;

        private TileContentBuilder()
        {
            _meshCache = new Dictionary<Vector2Int, Mesh>();
        }

        
        public Mesh GetMesh(Sprite sprite)
        {
            Vector2Int size = Vector2Int.zero;
            size.x = sprite.texture.width;
            size.y = sprite.texture.height;
            
            if (_meshCache.TryGetValue(size, out Mesh mesh))
                return mesh;
            
            Vector2 bounds = new Vector2(size.x / sprite.pixelsPerUnit, size.y / sprite.pixelsPerUnit);

            Vector3[] vertices = new[]
            {
                new Vector3(-bounds.x / 2f, 0, 0),
                new Vector3(-bounds.x / 2f, bounds.y, 0),
                new Vector3(bounds.x / 2f, bounds.y, 0),
                new Vector3(bounds.x / 2f, 0, 0)
            };
            int[] indices = new int[]
            {
                0,
                1,
                2,
                2,
                3,
                0
            };
            Vector2[] uvs = new[]
            {
                Vector2.zero, 
                Vector2.up, 
                Vector2.one,
                Vector2.right
            };

            mesh = new Mesh();
            mesh.name = $"Proc Sprite Mesh [{size.x}x{size.y}]";
            
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.SetUVs(0, uvs);
            
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            _meshCache.Add(size, mesh);
            return mesh;
        }

    }
}