using Gameplay.Cards;
using Gameplay.Tiles;
using UnityEngine;

namespace Gameplay.Commands.HoverPreviewCommands
{
	public class CardPreviewCommand : IActionPreviewCommand
	{
		private readonly Vector3 _origin;
		private readonly TileType _tileType;
		private readonly float _delay;

		private UICardPreview _preview;

		public CardPreviewCommand(Vector3 origin, TileType tileType, float delay)
		{
			_delay = delay;
			_origin = origin;
			_tileType = tileType;
		}
		
		public void Execute()
		{
			if (_shouldSkip) return;
			
			_preview = ObjectCache.Current.CardPool.GetCardPreview(_tileType, _origin);
			Vector3 pos = _preview.transform.localPosition;
			_preview.transform.localPosition = new Vector3(pos.x,pos.y + 1f,pos.z);
			
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
			
			// eliminar preview de la carta
			_preview.DestroySelf();
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