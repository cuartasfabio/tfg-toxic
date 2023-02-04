using System;
using Gameplay.Grids.Hexes.HexHelpers;
using UnityEngine;

namespace Gameplay.Grids.GridHelpers
{
	public class GridBounds
	{
		// The max and min HexCoordinates on the current grid.
		private int _minX { get; set; }
		private int _maxX { get; set; }
		private int _minZ { get; set; }
		private int _maxZ { get; set; }

		private Rect _bounds { get; set; }

		public GridBounds()
		{
			_maxX = Int32.MinValue;
			_maxZ = Int32.MinValue;
			
			_minX = Int32.MaxValue;
			_minZ = Int32.MaxValue;
		}

		/// <summary>
		/// Recalculates the new bounds and updates CameraControl.
		/// </summary>
		/// <param name="coords">The last created HexCoordinates</param>
		public void UpdateBounds(HexCoordinates coords)
		{
			_minX = Mathf.Min(_minX, coords.X);
			_minZ = Mathf.Min(_minZ, coords.Z);
			
			_maxX = Mathf.Max(_maxX, coords.X);
			_maxZ = Mathf.Max(_maxZ, coords.Z);
			
			// pasar de coords a world space
			float minX = _minX * HexMetrics.InnerRadius * 2;
			float maxX = _maxX * HexMetrics.InnerRadius * 2;
			float minZ = _minZ * HexMetrics.OuterRadius * 2;
			float maxZ = _maxZ * HexMetrics.OuterRadius * 2;

			_bounds = new Rect
			{
				xMin = minX,
				xMax = maxX,
				yMin = minZ,
				yMax = maxZ
			};
			
			ObjectCache.Current.CameraControl.UpdateGridBounds(_bounds);
		}
		
		public HexCoordinates GetOutOfCameraCoordinate()
		{
			return new HexCoordinates(_maxX + 20, 0);
		}
		
		
	}
}