using System;
using UnityEngine;

namespace Gameplay.Quests
{
	[Serializable, CreateAssetMenu(fileName = "New QuestPoolsData", menuName = "Quests/QuestPoolsData")]
	public class QuestPoolsData : ScriptableObject
	{
		/// <summary>
		/// List of quests from the default quest list.
		/// A default quest is one that can appear in every level.
		/// </summary>
		[SerializeField] public QuestId[] DefaultQuests;
		[SerializeField] public int NumberOfDefaultQuests;
		/// <summary>
		/// List of specific quests for a certain level.
		/// </summary>
		[Space]
		[SerializeField] public QuestId[] SpecificQuests;
		[SerializeField] public int NumberOfSpecificQuests;
	}
}