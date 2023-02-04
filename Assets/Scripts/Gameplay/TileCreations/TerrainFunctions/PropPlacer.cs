using System.Collections.Generic;
using Gameplay.TileCreations.TileScriptObjs;
using UnityEngine;
using Utils;

namespace Gameplay.TileCreations.TerrainFunctions
{
    public class PropPlacer
    {
        private readonly List<PropArea> _spriteAreas;
        private readonly TileData _tileData;

        private List<Vector2> _samples;

        public struct PlacedProp
        {
            public Vector3 Position;
            public Material Material;
            public Material MatPreview;
            public Sprite Sprite;
        }
        
        public struct PropArea
        {
            public float SampleX;
            public float SampleY;
            public float Radius;
        }
        
        
        public PropPlacer(TileData tileData)
        {
            _spriteAreas = new List<PropArea>();
            _tileData = tileData;
        }
        
        /// <summary>
        /// Uses Poisson Sampling to pick the position of the Props inside the Tile.
        /// </summary>
        /// <returns>A set of PlacedProps.</returns>
        public List<PlacedProp> PlaceWithPoisson() 
        {
            List<PlacedProp> stuffToPlace = new List<PlacedProp>();
            
            _samples = GetPoissonSamples();
            _samples.KFYShuffle();
        
            // empezar a colocar los sprites por orden de mayor anchura para optimizar espacio
            
            List<int> appearances = CalculateAppearances();
        
            for (int i = 0; i < _tileData.Props.Length; i++)
            {
                for (int j = 0; j < appearances[i]; j++)                                    // para cada aparicion
                {
                    if (_samples.Count < 1)
                        break;                                                              // no quedan samples en las que colocar

                    (Sprite, Material, Material) choice = PickRandomSpriteVariation(i);               // escoger un sprite de sus variations
                    
                    float exclusionRadius = GetSpriteWidth(i,choice.Item1) / 2f;            // calcular su radio
                    Vector2 selectedSample = _samples[Random.Range(0,_samples.Count)];      // escoger un punto random de samples

                    PropArea area = new PropArea()
                    {
                        SampleX = selectedSample.x,
                        SampleY = selectedSample.y,
                        Radius = exclusionRadius
                    };

                    area = SearchNoOverlappingSample(area);
                    
                    _samples.Remove(new Vector2(area.SampleX, area.SampleY));
                
                    _spriteAreas.Add(area);
                    
                    stuffToPlace.Add(new PlacedProp()
                    {
                        Position = new Vector3(area.SampleX, 0, area.SampleY),
                        Material = choice.Item2,
                        MatPreview = choice.Item3,
                        Sprite = choice.Item1
                    });

                    RemoveSamplesInsideArea(area);
                }
            }

            if (_tileData.HasCentralProp)
            {
                PropData propData = _tileData.CentralProp.propData;
                int randomVar = RandomTf.Rng.Next(propData.Variations.Length);
                stuffToPlace.Add(new PlacedProp()
                {
                    Position = new Vector3(0, 0, 0),
                    Material = propData.VariationsMaterials[randomVar],
                    MatPreview = propData.VarPreviewsMaterials[randomVar],
                    Sprite = propData.Variations[randomVar]
                });
            }
            
            return stuffToPlace;
        }
        
        private List<Vector2> GetPoissonSamples() 
        {
            PoissonDiscSampler pds = new PoissonDiscSampler
                (_tileData.Radius,_tileData.Radius, _tileData.MinSeparation);
            return pds.GetSamplesInList();
        }
        
        /// <summary>
        /// Returns possible spots for a Prop based on its width.
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        private PropArea SearchNoOverlappingSample(PropArea area)
        {
            PropArea clearArea = area;
            
            foreach (var sample in _samples)
            {
                if (SpriteAreasFunctions.IsAreaClear(clearArea, _spriteAreas))
                    break;
                clearArea = new PropArea()
                {
                    SampleX = sample.x,
                    SampleY = sample.y,
                    Radius = area.Radius
                };
            }

            return clearArea;
        }
        
        /// <summary>
        /// Excludes the samples inside a given PropArea
        /// </summary>
        /// <param name="area"></param>
        private void RemoveSamplesInsideArea(PropArea area)
        {
            Vector2 center = new Vector2(area.SampleX, area.SampleY);
            
            for (int i = 0; i < _samples.Count; i++)
            {
                float r = (_samples[i] - center).magnitude;
                if (r < area.Radius) 
                    _samples.Remove(_samples[i]);
            } 
        }

        /// <summary>
        /// Picks a number of Appearances for a Prop based on it's (min,max).
        /// </summary>
        /// <returns></returns>
        private List<int> CalculateAppearances()
        {
            List<int> appearances = new List<int>();
            for (int i = 0; i < _tileData.Props.Length; i++)
            {
                appearances.Add(Random.Range(_tileData.Props[i].Min,
                    _tileData.Props[i].Max + 1));
            }
            return appearances;
        }

        /// <summary>
        /// Picks a sprite and materials for a prop, from it's variations.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private (Sprite, Material, Material) PickRandomSpriteVariation(int index)
        {
            Sprite[] variations = _tileData.Props[index].propData.Variations;
            Material[] variationsMats = _tileData.Props[index].propData.VariationsMaterials;
            Material[] previewsMats = _tileData.Props[index].propData.VarPreviewsMaterials;
            
            int r = Random.Range(0,variations.Length);
            return (variations[r], variationsMats[r], previewsMats[r]);
        }

        private float GetSpriteWidth(int index, Sprite sprite)
        {
            return _tileData.Props[index].propData.Width / sprite.pixelsPerUnit;
        }
        
    }
}

