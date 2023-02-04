using Gameplay.Commands.GameLogic;

namespace Gameplay.Commands.HoverPreviewCommands
{
	/// <summary>
	/// These commands show the previews or visual representations of the different TileActionCommands.
	/// They are returned by ActionVisitors.
	/// </summary>
	public interface IActionPreviewCommand : IGameCommand
	{
		void Undo();
		void SkipExecution(bool skip);
		
		// void UndoWhenFinished(bool undo);
	}
}