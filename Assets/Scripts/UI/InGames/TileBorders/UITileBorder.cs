using Gameplay;
using Gameplay.Grids.Hexes.HexHelpers;
using UnityEngine;

namespace UI.InGames.TileBorders
{
	public class UITileBorder : MonoBehaviour
	{
		[SerializeField] private MeshRenderer _mesh;
		[SerializeField] private Transform _transform;

		public void Move(HexCoordinates coords)
		{
			_transform.position = HexFunctions.SnapToGrid(coords);
		}
		
		public void SetMat(TileBorderPool.HoverBorderType type)
		{
			_mesh.material = ObjectCache.Current.TileBorderPool.GetMatForBorderType(type);
		}
	}
}