using System;
using Gameplay.Grids.Hexes.HexHelpers;
using UI.InGames.ScoreTexts;

namespace Gameplay.Commands.HoverPreviewCommands
{
	public class ScorePreviewCommand : IActionPreviewCommand
	{
		private readonly HexCoordinates _coords;
		private readonly string _amount;
		private readonly int _fontSize;
		private readonly float _delay;

		private UIScoreText _scoreText;

		private readonly bool _isFromTurnEnd;
		
		public ScorePreviewCommand(HexCoordinates coords, string amount, int fontSize, float delay, bool isFromTurnEnd = false)
		{
			_delay = delay;
			_coords = coords;
			_amount = amount;
			_fontSize = fontSize;
			_isFromTurnEnd = isFromTurnEnd;
		}
		
		public void Execute()
		{
			if (_shouldSkip) return;
			
			_scoreText =
				ObjectCache.Current.ScoreTextPool.RequestScoreText(
					HexCoordinates.ToPosition(_coords), _fontSize, _isFromTurnEnd);
			_scoreText.SetText(_amount);
			
			if (Int32.Parse(_amount) > 0)
				ObjectCache.Current.ScoreManager.UpdateScorePreview(Int32.Parse(_amount));
			
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
			
			if (Int32.Parse(_amount) > 0)
				ObjectCache.Current.ScoreManager.ResetScorePreview(Int32.Parse(_amount));
			ObjectCache.Current.ScoreTextPool.GiveBackText(_scoreText);
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