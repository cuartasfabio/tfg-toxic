using System.Collections.Generic;
using Gameplay.Commands.GameLogic;

namespace Gameplay.Commands.HoverPreviewCommands
{
	public class UndoHoverPreviewsCommand : IGameCommand
	{
		private readonly List<IActionPreviewCommand> _commands;
		
		
		public UndoHoverPreviewsCommand(List<IActionPreviewCommand> commands)
		{
			_commands = commands;
		}
		
		public void Execute()
		{
			for (int i = 0; i < _commands.Count; i++)
				_commands[i].Undo();
		}

		public float GetDelay()
		{
			return 0.0f;
		}
	}
}