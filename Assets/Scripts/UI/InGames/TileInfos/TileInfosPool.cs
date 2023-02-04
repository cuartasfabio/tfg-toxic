using UnityEngine;

namespace UI.InGames.TileInfos
{
	public class TileInfosPool : MonoBehaviour
	{
		[SerializeField] private Transform _panel;
		[SerializeField] private GameObject _eatableLevelIconPrefab;
		private UIEatableLevelIcon _eatableLevelIcon;
		
		// private Queue<UIScoreText> _textsList;
		
		
		// private void Start()
		// {
		// 	_textsList = new Queue<UIScoreText>();
		// }
		
		public UIEatableLevelIcon RequestEatableLevelIcon(Vector3 tile, float yOffset, int level)
		{
			// if (_textsList.Count < 1)
			// {
			// 	UIScoreText textObject = Instantiate(_scoreTextPrefab, _panel);
			// 	_textsList.Enqueue(textObject);
			// }
			//
			// _textsList.Peek().Init(tile, fontSize);
			//
			// return _textsList.Dequeue();

			if (_eatableLevelIcon == null)
			{
				_eatableLevelIcon = Instantiate(_eatableLevelIconPrefab, _panel).GetComponent<UIEatableLevelIcon>();
			}
			
			_eatableLevelIcon.Init(tile, yOffset, level);
			_eatableLevelIcon.gameObject.SetActive(true);
			return _eatableLevelIcon;
		}

		// public void GiveBackScoreText(List<UIScoreText> canvases)
		// {
		// 	for (int i = 0; i < canvases.Count; i++)
		// 	{
		// 		GiveBackText(canvases[i]);
		// 	}
		// }
		
		public void GiveBackEatableLevelIcon()
		{

			if (_eatableLevelIcon != null)
			{
				_eatableLevelIcon.HideIcons();
				_eatableLevelIcon.gameObject.SetActive(false);
			}
		}
	}
}