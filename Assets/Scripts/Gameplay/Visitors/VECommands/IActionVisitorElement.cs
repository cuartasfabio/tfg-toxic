namespace Gameplay.Visitors.VECommands
{
	/// <summary>
	/// Visitor Element interface for the TileActionCommand hierarchy.
	/// </summary>
	public interface IActionVisitorElement
	{
		void Accept(AbstractActionVisitor commandVisitor);
	}
}