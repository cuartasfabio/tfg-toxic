using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Quests
{
	public class UIQuestList : MonoBehaviour
	{
		[SerializeField] private GameObject _questItemPrefab;
		[SerializeField] private GameObject _nextWorldBtnPrefab;

		[SerializeField] public Sprite Unchecked;
		[SerializeField] public Sprite Checked;
		
		private List<UIQuestItem> _quests;
		private UINextWorldButton _nwb;
		
		public void Init()
		{
			_quests = new List<UIQuestItem>();
		}

		public void FillQuestList(List<IQuest> chosenQuests)
		{
			for (int i = 0; i < chosenQuests.Count; i++)
			{
				UIQuestItem qItem = Instantiate(_questItemPrefab, transform).GetComponent<UIQuestItem>();
				qItem.Init(chosenQuests[i]);
				_quests.Add(qItem);
			}
		}

		public void MarkQuestAsCompleted(int index)
		{
			_quests[index].MarkAsCompleted();
		}

		public void UpdateProgressOfQuest(int index)
		{
			_quests[index].UpdateProgress();
		}

		public void EnableNextWorldButton()
		{
			StartCoroutine(SpanwNextWorldButton());
		}

		private IEnumerator SpanwNextWorldButton()
		{
			yield return new WaitForSeconds(3f);
			_nwb = Instantiate(_nextWorldBtnPrefab, transform).GetComponent<UINextWorldButton>();
		}
	}
}