using System.Collections.Generic;
using UnityEngine;

namespace UI.InGames.TileBorders
{
	public class TileBorderPool : MonoBehaviour
	{
		[SerializeField] private GameObject _borderPrefab;
		[SerializeField] private GameObject _toDiscoverPrefab;
		
		public enum HoverBorderType
		{
			Idle,
			CannotPlace,
			CanPlace
		}

		[SerializeField] private Material _hoveringMat;

		[SerializeField] private List<Color> _hoveringColors;

		public Material GetMatForBorderType(HoverBorderType type)
		{
			_hoveringMat.SetColor("_Tint", _hoveringColors[(int) type]);
			return _hoveringMat;
		}

		public UITileBorder GetTileBorderObject()
		{
			return Instantiate(_borderPrefab, Vector3.zero, Quaternion.identity).GetComponent<UITileBorder>();
		}
        
		public GameObject GetToDiscoverFogObject(Vector3 pos)
		{
			return Instantiate(_toDiscoverPrefab, pos, Quaternion.identity);
		}
	}
}