using Gameplay.Commands.GameLogic;
using Gameplay.Visitors.VECommands;

namespace Gameplay.Commands.TileBehaviourCommands
{
    /// <summary>
    /// These commands represent the actions and effects of the different TileBehaviours.
    /// They are returned by TileVisitors.
    /// </summary>
    public interface ITileActionCommand : IActionVisitorElement, IGameCommand
    {
        
    }
}