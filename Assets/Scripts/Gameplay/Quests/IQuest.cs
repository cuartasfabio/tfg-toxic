namespace Gameplay.Quests
{
	public interface IQuest
	{
		bool CheckCompletion();

		QuestId GetQuestId();

		string GetProgress();

		void SetCompleted();
	}
}