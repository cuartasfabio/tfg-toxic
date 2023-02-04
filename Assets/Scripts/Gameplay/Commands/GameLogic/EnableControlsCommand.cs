using Controls;

namespace Gameplay.Commands.GameLogic
{
	public class EnableControlsCommand : IGameCommand
	{
		private bool _enable;
		
		public EnableControlsCommand(bool enable)
		{
			_enable = enable;
		} 
		
		public void Execute()
		{
			GameControls.EnableControls(_enable);
			// ObjectCache.Current.UiCardHand.EnableCards();
		}

		public float GetDelay()
		{
			return 0.0f;
		}
	}
}