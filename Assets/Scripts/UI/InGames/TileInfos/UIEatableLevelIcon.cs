using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGames.TileInfos
{
	public class UIEatableLevelIcon : MonoBehaviour
	{
		[SerializeField] private RectTransform _IconsRectT;
		[SerializeField] private List<Image> _levels;

		private Vector3 _tile;
		private float _yOffset;

		public void Init(Vector3 tile, float yOffset, int level)
		{
			_tile = tile;
			_yOffset = yOffset;
			Vector3 screenPoint = ObjectCache.Current.MainCamera.WorldToScreenPoint(_tile);
			_IconsRectT.position = new Vector2(screenPoint.x, screenPoint.y + _yOffset);

			for (int i = 0; i < level; i++)
			{
				_levels[i].gameObject.SetActive(true);
			}
		}
		
		private void Update()
		{
			Vector3 screenPoint = ObjectCache.Current.MainCamera.WorldToScreenPoint(_tile);
			_IconsRectT.position = new Vector2(screenPoint.x, screenPoint.y + _yOffset);
		}

		public void HideIcons()
		{
			for (int i = 0; i < _levels.Count; i++)
			{
				_levels[i].gameObject.SetActive(false);
			}
		}
	}
}