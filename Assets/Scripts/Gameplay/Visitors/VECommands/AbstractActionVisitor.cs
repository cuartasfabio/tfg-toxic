using System.Collections.Generic;
using Gameplay.Commands.HoverPreviewCommands;
using Gameplay.Commands.TileBehaviourCommands;
using Gameplay.Commands.TileBehaviourCommands.Formations;

namespace Gameplay.Visitors.VECommands
{
	/// <summary>
	/// Visitor interface for the TileActionCommand object structure.
	/// </summary>
	public class AbstractActionVisitor
	{
		protected List<IActionPreviewCommand> _commands;

		/// <summary>
		/// Runs the visitor for every TileActionCommand resultant of an TileVisitor.
		/// </summary>
		/// <param name="tileActions">The list of TileActionCommands to evaluate.</param>
		/// <returns>The ActionPreviewCommands for all the TileActionCommands.</returns>
		public List<IActionPreviewCommand> GetPreviewCommands(List<ITileActionCommand> tileActions)
		{
			_commands = new List<IActionPreviewCommand>();
			for (int i = 0; i < tileActions.Count; i++)
			{
				tileActions[i].Accept(this);
			}
			return _commands;
		} 
		
		public virtual void Visit(DiscoverTileActionCommand tileAction) {  }
		public virtual void Visit(CreateAndPlaceTileActionCommand tileAction) {  }
		public virtual void Visit(AddToFormationActionCommand tileAction) {  }
		public virtual void Visit(RemoveFromFormationActionCommand tileAction) {  }
		public virtual void Visit(UpdateScoreActionCommand tileAction) {  }
		public virtual void Visit(CardToHandActionCommand tileAction) {  }
		public virtual void Visit(PlaceSelectedActionCommand tileAction) {  }
		public virtual void Visit(ShuffleCardIntoDeckActionCommand tileAction) {  }
	}
}