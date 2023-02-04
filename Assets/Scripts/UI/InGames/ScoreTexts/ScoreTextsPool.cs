using System.Collections.Generic;
using UnityEngine;

namespace UI.InGames.ScoreTexts
{
	public class ScoreTextsPool : MonoBehaviour
	{
		[SerializeField] private Transform _panel;
		[SerializeField] private UIScoreText _scoreTextPrefab;
		private Queue<UIScoreText> _textsList;
		[Space]
		[SerializeField] private Color _colorUp_onPlace;
		[SerializeField] private Color _colorDown_onPlace;
		private (Color, Color) _onPlaceColors;
		[Space]
		[SerializeField] private Color _colorUp_onTurnEnd;
		[SerializeField] private Color _colorDowp_onTurnEnd;
		private (Color, Color) _onTurnEndColors;
		
		
		private void Start()
		{
			_textsList = new Queue<UIScoreText>();
			
			_onPlaceColors = (_colorUp_onPlace, _colorDown_onPlace);
			_onTurnEndColors = (_colorUp_onTurnEnd, _colorDowp_onTurnEnd);
		}
		
		public UIScoreText RequestScoreText(Vector3 tile, int fontSize, bool isForTurnEnd = false)
		{
			if (_textsList.Count < 1)
			{
				UIScoreText textObject = Instantiate(_scoreTextPrefab, _panel);
				_textsList.Enqueue(textObject);
			}

			(Color, Color) colors = _onPlaceColors;
			
			if (isForTurnEnd) colors = _onTurnEndColors;
				
			
			_textsList.Peek().Init(tile, fontSize, colors);

			return _textsList.Dequeue();
		}

		public void GiveBackTextList(List<UIScoreText> canvases)
		{
			for (int i = 0; i < canvases.Count; i++)
			{
				GiveBackText(canvases[i]);
			}
		}
		
		public void GiveBackText(UIScoreText text)
		{
			text.DeactivateSelf();
			_textsList.Enqueue(text);
		}
		
	}
}