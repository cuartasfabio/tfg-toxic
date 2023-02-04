using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Gameplay.Quests
{
	public class UIQuestItem : MonoBehaviour
	{
		[SerializeField] private RectTransform _rect;
		[SerializeField] private Image _background;
		
		[SerializeField] private Image _questClassIcon;
		[SerializeField] private TMP_Text _text;
		[SerializeField] private TMP_Text _progress;
		[SerializeField] private Image _checkBoxIcon;

		private RectTransform _layout;
		private IQuest _quest;

		public void Init(IQuest quest)
		{
			_quest = quest;
			
			_layout = GetComponentInParent<RectTransform>();
			
			_text.text = quest.ToString();
			_progress.text = quest.GetProgress();
			_checkBoxIcon.sprite = ObjectCache.Current.UIQuestList.Unchecked;
			_checkBoxIcon.color = Color.gray;
		}

		public void MarkAsCompleted()
		{
			_questClassIcon.color = Color.gray;
			_progress.fontSharedMaterial = _progress.fontSharedMaterials[0];
			_checkBoxIcon.sprite = ObjectCache.Current.UIQuestList.Checked;
			_checkBoxIcon.color = new Color(0.7283847f, 1f, 0.2783019f, 1f);
			
			StartCoroutine(VanishQuestItem());
		}

		public void UpdateProgress()
		{
			_progress.text = _quest.GetProgress();
		}

		private IEnumerator VanishQuestItem()
		{
			yield return new WaitForSeconds(1f);
			StartCoroutine(AnimationsController.ImageAlphaFadeOut(_questClassIcon, TfMath.EaseInOutQuint, 1f));
			StartCoroutine(AnimationsController.TextFadingOut(_text, TfMath.EaseInOutQuint, 1f));
			StartCoroutine(AnimationsController.TextFadingOut(_progress, TfMath.EaseInOutQuint, 1f));
			StartCoroutine(AnimationsController.ImageAlphaFadeOut(_checkBoxIcon, TfMath.EaseInOutQuint, 1f));
			StartCoroutine(AnimationsController.ImageAlphaFadeOut(_background, TfMath.EaseInOutQuint, 1f));
			yield return new WaitForSeconds(1.2f);
			
			IEnumerator updateLayoutCoroutine = UpdateCardLayoutGroup();
			StartCoroutine(updateLayoutCoroutine);
			
			yield return StartCoroutine(AnimationsController.ScaleUiElementHeight(_rect,60, -10, TfMath.EaseInOutQuint, 0.4f));
			yield return new WaitForSeconds(0.75f);

			StopCoroutine(updateLayoutCoroutine);
			
			Destroy(gameObject);
		}
		
		private IEnumerator UpdateCardLayoutGroup()
		{
			while (true)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(_layout);
				yield return new WaitForSeconds(0.001f);
			}
		}
	}
}