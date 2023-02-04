using System.Collections.Generic;
using Cysharp.Text;
using Gameplay.Commands.HoverPreviewCommands;
using Gameplay.Commands.TileBehaviourCommands;
using Gameplay.Grids.Hexes.HexHelpers;

namespace Gameplay.Visitors.VECommands
{
	public class OnHoverVisualEffectsVisitor : AbstractActionVisitor
	{
		//private List<UIScoreText> _scoreTextsList;
		// private List<GameObject> _toDiscoverConsumeList;	// pool de objetos también para estos dos prefabs
		// private List<CardBehaviour> _toDiscoverBehs;

		public OnHoverVisualEffectsVisitor()
		{
			_commands = new List<IActionPreviewCommand>();
			// _scoreTextsList = new List<UIScoreText>();
			// _toDiscoverConsumeList = new List<GameObject>();
			// _toDiscoverBehs = new List<CardBehaviour>();
		}

		// public void UndoCommands()
		// {
		// 	for (int i = 0; i < _commands.Count; i++)
		// 	{
		// 		_commands[i].Undo();
		// 	}
		// }

		// public void HoverChanges()
		// {
		// 	// Borra toda la visualización del onhover previo
		// 	// crear nuevos commands con execute y undo (deshace la previo onhover)
		// 	
		// 	// devuelve los texts al score pool
		// 	// ObjectCache.Current.ScoreTextPool.GiveBackTextList(_scoreTextsList);
		// 	// _scoreTextsList = new List<UIScoreText>();
		// 	
		// 	// borra las tiles a consumir / descubrir
		// 	// for (int i = 0; i < _toDiscoverConsumeList.Count; i++)
		// 	// {
		// 	// 	Object.Destroy(_toDiscoverConsumeList[i]);
		// 	// }
		// 	// _toDiscoverConsumeList = new List<GameObject>();
		// 	
		// 	// borra los behaviours to discover
		// 	// for (int i = 0; i < _toDiscoverBehs.Count; i++)
		// 	// {
		// 	// 	_toDiscoverBehs[i].DeleteSelf();
		// 	// }
		// 	// _toDiscoverBehs = new List<CardBehaviour>();
		// 	
		// }

		public override void Visit(DiscoverTileActionCommand cardActionCommand)
		{
			if (cardActionCommand.ShowCellPreview)
				_commands.Add(new DiscoverPreviewCommand(cardActionCommand.TileToDiscover, 0f));
		}

		public override void Visit(CreateAndPlaceTileActionCommand cardActionCommand)
		{
			_commands.Add(new ReplaceTilePreviewCommand(cardActionCommand.TileToReplace, cardActionCommand.NewType, cardActionCommand.SameAsHover, 0f));
		}
		
		public override void Visit(CardToHandActionCommand cardActionCommand)
		{
			_commands.Add(new CardPreviewCommand(HexCoordinates.ToPosition(cardActionCommand.OriginCoordinates),
				cardActionCommand.TypeToCreate, 0f));
		}
		
		public override void Visit(UpdateScoreActionCommand cardActionCommand)
		{
			for (int i = 0; i < cardActionCommand.PartialScores.Count; i++)
				_commands.Add(new ScorePreviewCommand(cardActionCommand.PartialScores[i].ForCoords,
					cardActionCommand.PartialScores[i].PartialAmount.ToString(), 70, 0.0f, cardActionCommand.IsFromTurnEnd));
			
			if (cardActionCommand.TotalAmount > 0 || cardActionCommand.PartialScores.Count > 0 && !cardActionCommand.IsFromTurnEnd)
				_commands.Add(new ScorePreviewCommand(cardActionCommand.OriginCoords,
					ZString.Concat((cardActionCommand.TotalAmount < 0 ? "" : "+"), cardActionCommand.TotalAmount), 110, 0.0f, cardActionCommand.IsFromTurnEnd));
		}

		public override void Visit(ShuffleCardIntoDeckActionCommand cardActionCommand)
		{
			_commands.Add(new CardToDeckPreviewCommand(HexCoordinates.ToPosition(cardActionCommand.OriginCoordinates),
				cardActionCommand.TypeToShuffle, 0f));
		}

	}
}