using TMPro;
using UnityEngine;

namespace UI.InGames.DescriptionHelp
{
    public class TooltipName: MonoBehaviour
    {
        public static TooltipName Instance { get; private set; }

        private bool _showCardTt;

        [SerializeField] private TextMeshProUGUI _tmp;
        
        private void Awake()
        {
            Instance = this;
            _tmp.GetComponent<TextMeshProUGUI>();
        }

        public void ShowToolTip(string textToDisplay)
        {
            _tmp.text = textToDisplay;
            _tmp.enabled = true;
        }

        public void HideToolTip()
        {
            _tmp.enabled = false;
        }

        public bool GetShowCardTooltip()
        {
            return _showCardTt;
        }
        public void SetShowCardTooltip(bool show)
        {
            _showCardTt = show;
        }
    }
}