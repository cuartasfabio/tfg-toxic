using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.TileCreations.TerrainFunctions;
using Gameplay.TileCreations.TileScriptObjs;
using UnityEngine;
using Utils;

namespace Gameplay.TileCreations
{
    public class Tile: MonoBehaviour
    {
        private TileData _tileData;
        private TileContentBuilder _contentBuilder;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _tileBaseTransform;
        // [SerializeField] private SpriteRenderer _spriteRenderer;

        private List<(PropPlacer.PlacedProp, Renderer)> _props;
        
        [Serializable]
        public class Prop
        {
            public PropData propData;
            public int Min = 0;
            public int Max = 0;
        }

        public void Init(TileData tileData)
        {
            _tileData = tileData;
            _props = new List<(PropPlacer.PlacedProp, Renderer)>();
            BuildContent();
        }

        /// <summary>
        /// Builds the content of the tile, sets the base and places the props.
        /// </summary>
        private void BuildContent()
        {
            _meshRenderer.material = _tileData.TileBaseMaterial;
            
            PropPlacer propPlacer = new PropPlacer(_tileData);
           
            List<PropPlacer.PlacedProp> stuffToPlace = propPlacer.PlaceWithPoisson();
            
            for (int i = 0; i < stuffToPlace.Count; i++)
            {
                PropPlacer.PlacedProp prop = stuffToPlace[i];
                
                GameObject newSpriteObj = new GameObject($"[Sprite Obj] {prop.Sprite.name}");
                
                if (prop.Material != null)
                {
                    MeshFilter meshFilter = newSpriteObj.AddComponent<MeshFilter>();
                    MeshRenderer meshRenderer = newSpriteObj.AddComponent<MeshRenderer>();

                    meshFilter.sharedMesh = TileContentBuilder.Get().GetMesh(prop.Sprite);
                    meshRenderer.sharedMaterial = prop.Material;
                    
                    _props.Add((prop,meshRenderer));
                }
                else
                {
                    SpriteRenderer spriteRenderer = newSpriteObj.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = prop.Sprite;
                    spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;
                    
                    _props.Add((prop,spriteRenderer));
                }
                
                newSpriteObj.transform.SetParent(transform);
                
                newSpriteObj.transform.localPosition = prop.Position;
                
                
            }
        }

        public void ShowPreview(Color tint)
        {
            _meshRenderer.material.SetFloat("_GreyScale", 1f);
            _meshRenderer.material.SetColor("_Tint", tint);
            for (int i = 0; i < _props.Count; i++)
            {
                _props[i].Item2.material = _props[i].Item1.MatPreview;
                _props[i].Item2.material.SetColor("_Tint", tint);
            }  
        }
        
        public void ShowTile()
        {
            _meshRenderer.material.SetColor("_Tint", Color.white);
            _meshRenderer.material.SetFloat("_GreyScale", -1f);
            for (int i = 0; i < _props.Count; i++)
            {
                _props[i].Item2.material = _props[i].Item1.Material;
            }
        }

        public IEnumerator TilePlaceAnimation()
        {
            // scale down base
            _tileBaseTransform.localScale = Vector3.zero;
            // scale down props
            for (int i = 0; i < _props.Count; i++)
            {
                _props[i].Item2.transform.localScale = Vector3.zero;
            }


            // base scales up
            StartCoroutine(AnimationsController.ScaleUiElement(_tileBaseTransform, Vector3.one, TfMath.EaseInOutCubic, 0.2f));
            yield return new WaitForSeconds(0.2f);
            
            // props scale up
            for (int i = 0; i < _props.Count; i++)
            {
                StartCoroutine(AnimationsController.ScaleUiElement(_props[i].Item2.transform, Vector3.one, TfMath.EaseOutElastic, 0.15f));
                yield return new WaitForSeconds(0.025f);
            }
        }

        public IEnumerator TileHoverEnterAnimation()
        {
            _meshRenderer.transform.localScale = new Vector3(1.45f,1.45f,1.45f);
            // scale down props
            for (int i = 0; i < _props.Count; i++)
            {
                _props[i].Item2.transform.localScale = Vector3.one;
            }
            
            StartCoroutine(AnimationsController.ScaleUiElement(_meshRenderer.transform, new Vector3(1.47f,1.47f,1.47f), TfMath.BellEase, 0.15f));
            // props scale up
            for (int i = 0; i < _props.Count; i++)
            {
                StartCoroutine(AnimationsController.ScaleUiElement(_props[i].Item2.transform, new Vector3(1.05f,1.05f,1.05f), TfMath.BellEase, 0.1f));
                
            }

            yield return null;
        }
        
        public void TileHoverExit()
        {
            _meshRenderer.transform.localScale = new Vector3(1.45f,1.45f,1.45f);
            // scale down props
            for (int i = 0; i < _props.Count; i++)
            {
                _props[i].Item2.transform.localScale = Vector3.one;
            }
        }
        
       
        
    }
}