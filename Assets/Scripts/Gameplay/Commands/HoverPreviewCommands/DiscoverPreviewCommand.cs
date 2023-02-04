using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;
using UnityEngine;

namespace Gameplay.Commands.HoverPreviewCommands
{
	public class DiscoverPreviewCommand : IActionPreviewCommand
	{
		private readonly HexCoordinates _coords;
		private readonly float _delay;
		
		private TileBehaviour _behToDiscover;
		private GameObject _toDiscoverFog;
		
		public DiscoverPreviewCommand(HexCoordinates coords, float delay)
		{
			_delay = delay;
			_coords = coords;
		}
		
		public void Execute()
		{
			if (_shouldSkip) return;
			
			Vector3 position = HexCoordinates.ToPosition(_coords);
			
			// mostrar una PREVIEW de la tile a descubrir
			
			TileType type = ObjectCache.Current.TerrainBuilder.GetTerrainTileForCoords(_coords);
			_behToDiscover = ObjectCache.Current.TileBehaviourPool.GetNewTile(type, HexCoordinates.ToPosition(_coords));
			
			_behToDiscover.SetMeshRaycast(false);
			_behToDiscover.ShowPreview(new Color(0.78f,0.39f,0.78f,1));
			
			_toDiscoverFog = ObjectCache.Current.TileBorderPool.GetToDiscoverFogObject(position);
			
			_executed = true;
			
			if (_undoWhenFinished) Undo();
		}

		public float GetDelay()
		{
			return _delay;
		}

		public void Undo()
		{
			if (!_executed) return;
			
			Object.Destroy(_toDiscoverFog);
			_behToDiscover.DeleteSelf();
		}
		
		private bool _shouldSkip;
		public void SkipExecution(bool skip)
		{
			_shouldSkip = skip;
		}

		private bool _undoWhenFinished;
		public void UndoWhenFinished(bool undo)
		{
			_undoWhenFinished = undo;
		}

		private bool _executed;
	}
}