using System.Collections.Generic;
using Gameplay.Commands.TileBehaviourCommands;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;
using Utils;

namespace Gameplay.Visitors.Tiles
{
	public class OnTurnEndVisitor : AbstractTileVisitor
	{
		// The turn ends right after all effects of placing a Card are finished

		private UpdateScoreActionCommand _turnEndScoreActionCommand;
		
		public new List<ITileActionCommand> GetBehaviourCommands(List<TileBehaviour> tiles)
		{
			_commands = new List<ITileActionCommand>();
			
			_turnEndScoreActionCommand = new UpdateScoreActionCommand(new HexCoordinates(), 0.0f, true);
			// _commands.Add(_turnEndScoreCommand); todo UNCOMMENT TO MAKE SCORE WORK
			
			for (int i = 0; i < tiles.Count; i++)
			{
				tiles[i].Accept(this);
			}
			
			return _commands;
		}

		public UpdateScoreActionCommand GetTurnEndScoreCommand()
		{
			return _turnEndScoreActionCommand;
		}
		

        public override void Visit(MutantsBehaviour behaviour)
        {
	        HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);
	        
	        
	        List<HexCoordinates> eatableWastes = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
		        new[] {TileType.Wastes}, 1);
	        if (eatableWastes.Count > 0)
	        {
		        _commands.Add(new CreateAndPlaceTileActionCommand(behaviourCoords, TileType.Ground, 0.1f));
		        _commands.Add(new CreateAndPlaceTileActionCommand(eatableWastes[0], TileType.Mutants, 0.1f));
		        _turnEndScoreActionCommand.AddPartialScore(20, eatableWastes[0]);
		        return;
	        }
	        
	        // check for neighbour campsite / village / purifier / radioTower
	        List<HexCoordinates> canAttack = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
		        new[] {TileType.Campsite, TileType.Lumberjack, TileType.Farm, TileType.Purifier , TileType.Village, TileType.RadioTower}, 1);
	        if (canAttack.Count > 0)
	        {
		        HexCoordinates eatenTile = canAttack[RandomTf.Rng.Next(canAttack.Count)];
		        _commands.Add(new CreateAndPlaceTileActionCommand(behaviourCoords, TileType.Ground, 0.1f));
		        _commands.Add(new CreateAndPlaceTileActionCommand(eatenTile, TileType.Ground, 0.1f));
		        _turnEndScoreActionCommand.AddPartialScore(-10, eatenTile);
		        return;
	        }
	        
	        // moves randomly
	        List<HexCoordinates> walkable = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
		        new[] {TileType.Ground}, 1);

	        if (walkable.Count > 0)
	        {
		        int randomNeighbour = RandomTf.Rng.Next(walkable.Count);

		        _commands.Add(new CreateAndPlaceTileActionCommand(behaviourCoords, TileType.Ground, 0.1f));
		        _commands.Add(new CreateAndPlaceTileActionCommand(walkable[randomNeighbour], TileType.Mutants, 0.1f));
		       
	        }
        }
        
        public override void Visit(MonolithBehaviour behaviour)
        {
	        HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);
	        
	        
	        // extra score per natural neighbour
	        List<HexCoordinates> naturals = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
		        new[] {TileType.Dunes, TileType.Forest, TileType.Lake, TileType.Meadow , TileType.Mountain, TileType.Oasis, TileType.Swamp}, 2);
	        for (int i = 0; i < naturals.Count; i++)
	        {
		        _turnEndScoreActionCommand.AddPartialScore(2, naturals[i]);
	        }
        }

	}
}