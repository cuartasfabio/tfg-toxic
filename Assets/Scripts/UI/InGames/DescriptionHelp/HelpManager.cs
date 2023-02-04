using Backend.Localization;
using Gameplay;
using Gameplay.Cards;
using Gameplay.Tiles;
using TMPro;
using UnityEngine;

namespace UI.InGames.DescriptionHelp
{
	public class HelpManager : MonoBehaviour
	{
		[SerializeField] private TMP_Text _detailTMP;

		private void SetTypeDescription(string type)
		{
			_detailTMP.text = type;
		}

		public void UpdateHelpPanel(TileType type)
		{
			UICardData uiCardData = ObjectCache.Current.CardPool.GetCardDataOfType(type);
			UpdateHelpPanel(uiCardData);
		}
		
		public void UpdateHelpPanel(UICardData uiCardData)
		{
			TooltipName.Instance.ShowToolTip(StringBank.GetStringRaw(uiCardData.CardName));
			SetTypeDescription(StringBank.GetStringRaw(uiCardData.Synergies));
		}
	}
}