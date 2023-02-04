using System.Collections.Generic;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Visitors.VECommands;

namespace Gameplay.Commands.TileBehaviourCommands
{
	public class UpdateScoreActionCommand : ITileActionCommand
	{
		private readonly float _delay;
		public int TotalAmount { get; private set;  }
		public HexCoordinates OriginCoords { get; }
		public List<PartialScore> PartialScores { get; }
		public struct PartialScore
		{
			public int PartialAmount;
			public HexCoordinates ForCoords;
		}
		public bool IsFromTurnEnd { get; }

		public UpdateScoreActionCommand(HexCoordinates originCoords, float delay, bool isFromTurnEnd = false)
		{
			PartialScores = new List<PartialScore>();
			
			_delay = delay;
			OriginCoords = originCoords;

			IsFromTurnEnd = isFromTurnEnd;
		}

		public void AddPartialScore(int amount, HexCoordinates coords)
		{
			if (!coords.Equals(OriginCoords))
			{
				if (!AddToExistingPartialScore(amount, coords))
				{
					PartialScores.Add(new PartialScore() { PartialAmount = amount, ForCoords = coords });
					TotalAmount += amount;
				}

			} else
			{
				TotalAmount += amount;
			}
			
		}

		private bool AddToExistingPartialScore(int amount, HexCoordinates coords)
		{
			// si la coord ya tenía una score parcial, sustituirla por otra score parcial con la suma de amounts
			for (int i = 0; i < PartialScores.Count; i++)
			{
				if (PartialScores[i].ForCoords.Equals(coords))
				{
					PartialScores.Add(new PartialScore()
						{PartialAmount = (PartialScores[i].PartialAmount + amount), ForCoords = coords});
					PartialScores.Remove(PartialScores[i]);
					TotalAmount += amount;
					return true;
				}
			}
			return false;
		}
		
		public void Execute()
		{
			if (TotalAmount != 0)
			{
				ObjectCache.Current.ScoreManager.ExtractCrystalPoints(TotalAmount, OriginCoords);
				GameManager.Get().RunManager.LevelManager.LevelStats.LastTurnScore = TotalAmount;
				ObjectCache.Current.ScoreManager.UpdateScore(TotalAmount);
			}
		}

		public float GetDelay()
		{
			return _delay;
		}

		public void Accept(AbstractActionVisitor commandVisitor)
		{
			commandVisitor.Visit(this);
		}

		// public void AddPartialScores(UpdateScoreCommand otherScoreCommand)
		// {
		// 	for (int i = 0; i < otherScoreCommand.PartialScores.Count; i++)
		// 	{
		// 		PartialScore partialScore = otherScoreCommand.PartialScores[i];
		// 		AddPartialScore(partialScore.PartialAmount, partialScore.ForCoords);
		// 	}
		// }

	}
}