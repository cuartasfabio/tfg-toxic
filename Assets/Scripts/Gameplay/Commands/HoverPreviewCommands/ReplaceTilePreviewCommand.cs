using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;
using UnityEngine;

namespace Gameplay.Commands.HoverPreviewCommands
{
	public class ReplaceTilePreviewCommand : IActionPreviewCommand
	{
		private readonly HexCoordinates _tileToReplace;
		private readonly TileType _newType;
		private readonly bool _sameAsHover;
		private readonly float _delay;

		private TileBehaviour _toBeReplaced;
		private Vector3 _localPos;
		private TileBehaviour _newPreview;

		public ReplaceTilePreviewCommand(HexCoordinates tileToReplace, TileType newType, bool sameAsHover, float delay)
		{
			_delay = delay;
			_tileToReplace = tileToReplace;
			_newType = newType;
			_sameAsHover = sameAsHover;
		}
		
		public void Execute()
		{
			if (_shouldSkip) return;
			
			// todo minimizar accesos a Transforms

			_toBeReplaced =
				ObjectCache.Current.HexGrid.Lists.CoordinatesBehaviours[_tileToReplace];
			_localPos = _toBeReplaced.gameObject.transform.localPosition;
			_toBeReplaced.gameObject.SetActive(false);
			_newPreview = ObjectCache.Current.TileBehaviourPool.GetNewTile(_newType, _localPos);
			
			_newPreview.ShowPreview(Color.grey);
			// bajar alfa de toda la tile

			// if (_sameAsHover)
			// {
			// 	// _toReplaceBorder.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			// 	// _toReplaceBorder.gameObject.transform.position = new Vector3(_localPos.x, 1.2f, _localPos.z);
			// 	_newPreview.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			// 	_newPreview.gameObject.transform.position = new Vector3(_localPos.x, 1.2f, _localPos.z);
			// }

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
			
			// Object.Destroy(_toReplaceBorder.gameObject);
			_newPreview.DeleteSelf();
			_toBeReplaced.gameObject.SetActive(true);
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