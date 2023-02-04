namespace Gameplay.Commands.GameLogic
{
	/// <summary>
	/// Base command interface for TileActions and ActionPreview commands.
	/// </summary>
	public interface IGameCommand
	{
		void Execute();
		float GetDelay();
	}
}