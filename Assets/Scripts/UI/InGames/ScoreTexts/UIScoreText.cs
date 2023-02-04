using System;
using Gameplay;
using TMPro;
using UnityEngine;

namespace UI.InGames.ScoreTexts
{
	public class UIScoreText : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _TMPText;
		[SerializeField] private RectTransform _textRectT;

		private Color _positive;
		private Color _negative;

		private Vector3 _tilePosition;

		private void Update()
		{
			_textRectT.position = ObjectCache.Current.MainCamera.WorldToScreenPoint(_tilePosition);
		}

		public void Init(Vector3 tile, int fontSize, (Color,Color) colors)
		{
			_tilePosition = tile;
			_textRectT.position = ObjectCache.Current.MainCamera.WorldToScreenPoint(_tilePosition);
			
			_TMPText.fontSize = fontSize;
			
			_TMPText.enabled = true;

			_positive = colors.Item1;
			_negative = colors.Item2;
		}
		
		public void DeactivateSelf()
		{
			_TMPText.enabled = false;
		}
		
		public void SetText(String text)
		{
			_TMPText.text = text;
			_TMPText.color = Int32.Parse(text) < 0 ? _negative : _positive;
		}
		
		
	}
}