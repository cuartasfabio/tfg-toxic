using UnityEngine;

namespace Gameplay.Grids.GridHelpers
{
	/// <summary>
	/// Represents the chances of discovering a neighbour cell when a tile is placed next to the grid limits.
	/// </summary>
	public class GridDiscoveryRate
	{
		private float _discoveryRate;
		
		public GridDiscoveryRate(float discoveryRate)
		{
			SetRate(discoveryRate);
		}

		public void SetRate(float amount)
		{
			_discoveryRate = Mathf.Clamp01(_discoveryRate + amount);
		}

		public float GetRate()
		{
			return _discoveryRate;
		}
	}
}