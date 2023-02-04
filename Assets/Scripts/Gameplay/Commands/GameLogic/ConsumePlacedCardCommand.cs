using Gameplay.Cards;

namespace Gameplay.Commands.GameLogic
{
	public class ConsumePlacedCardCommand : IGameCommand
	{
		private readonly float _delay;
		private readonly UICard _selectedCard;
		
		public ConsumePlacedCardCommand(UICard selectedCard, float delay = 0.0f)
		{
			_selectedCard = selectedCard;
			_delay = delay;
		}
		
		public void Execute()
		{
			ObjectCache.Current.UiCardHand.ConsumeAndRemoveCard(_selectedCard);
		}

		public float GetDelay()
		{
			return _delay;
		}
	}
}