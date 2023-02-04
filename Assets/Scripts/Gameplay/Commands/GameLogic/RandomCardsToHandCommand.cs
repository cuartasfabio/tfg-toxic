namespace Gameplay.Commands.GameLogic
{
	public class RandomCardsToHandCommand : IGameCommand
	{
		private readonly float _delay;

		public RandomCardsToHandCommand(float delay = 0.0f)
		{
			_delay = delay;
		}
		
		public void Execute()
		{
			ObjectCache.Current.UiCardHand.DrawCards();
		}

		public float GetDelay()
		{
			return _delay;
		}
		
	}
}