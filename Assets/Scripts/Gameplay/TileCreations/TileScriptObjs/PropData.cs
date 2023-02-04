using System;
using UnityEngine;

namespace Gameplay.TileCreations.TileScriptObjs
{
    [Serializable, CreateAssetMenu(fileName = "New PropData", menuName = "Props/PropData")]
    public class PropData : ScriptableObject
    {
        [SerializeField] public Sprite[] Variations;
        
        [SerializeField] public Material[] VariationsMaterials;
        
        [SerializeField] public Material[] VarPreviewsMaterials;
        /// <summary>
        /// Width of the prop in pixels.
        /// Used to calculate the space between props.
        /// </summary>
        [SerializeField] public float Width;
    }
}