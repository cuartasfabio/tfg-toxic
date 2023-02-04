using Audio;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Visitors.VECommands;

namespace Gameplay.Commands.TileBehaviourCommands
{
    public class DiscoverTileActionCommand: ITileActionCommand
    {
        private readonly float _delay;
        public HexCoordinates TileToDiscover { get;  }

        public readonly bool ShowCellPreview;

        public DiscoverTileActionCommand(HexCoordinates coords, bool showPreview, float delay)
        {
            _delay = delay;
            TileToDiscover = coords;
            ShowCellPreview = showPreview;
        }
        
        public void Execute()
        {
            AudioController.Get().PlaySfx(AudioId.SFX_TileDiscover);
            ObjectCache.Current.GridSizeManager.DiscoverTile(TileToDiscover);
            
            if(ShowCellPreview) GameManager.Get().RunManager.LevelManager.LevelStats.ExploredTiles += 1;
            
        }

        public float GetDelay()
        {
            return _delay;
        }

        public void Accept(AbstractActionVisitor commandVisitor)
        {
            commandVisitor.Visit(this);
        }
        
    }
}