using Backend.Persistence;
using UnityEngine;

namespace Gameplay.Scores
{
	public class DynamicScoreObjectives : IPersistable
	{
		private readonly float _period;
		
		private readonly float[] _steps;
		
		private int _currentOffset;
		private int _currentStep;
		private int _currentObjective;
		
		private readonly float _functionSlopeFactor;	// 1.5 easiest, 1.25 moderate, 1.0 hard

		public DynamicScoreObjectives(float functionSlopeFactorFactor)
		{
			_functionSlopeFactor = functionSlopeFactorFactor;
			
			_period = 2 * Mathf.PI / 3f;
			
			// the 3 points sampled every period of the function
			_steps = new[]
			{
				0f, 0.9339f, 1.55f
			};
			
			_currentOffset = 2;
			_currentStep = 1;
			
			UpdateObjective();
		}

		public int GetCurrentObjective()
		{
			return _currentObjective;
		}
		
		public void UpdateObjective()
		{
			_currentObjective = ComputeNextObjective();
		}

		private int ComputeNextObjective()
		{
			// primero, curva coseno dificultad
			// luego, otros factores que afecten

			float xStep = _steps[_currentStep] + _currentOffset * _period;
			float difficultyMult = EvaluateDifficulty(xStep) * 10f;
			IncrementStep();
			// IncrementOffset();
			return RoundObjective(difficultyMult);
		}
		
		private float EvaluateDifficulty(float x)
		{
			return Mathf.Cos(3 * x) + x/_functionSlopeFactor; // difficulty formula
		}

		private int RoundObjective(float mult)
		{
			// REDONDEAR A DECENAS
			mult = mult / 10f;
			mult = Mathf.Round(mult);
			mult = mult * 10f;
			return (int) mult;
		}

		private void IncrementStep()
		{
			if (_currentStep < _steps.Length - 1)
				_currentStep++;
			else
			{
				_currentStep = 0;
				IncrementOffset();
			}
				
		}
		
		private void IncrementOffset()
		{
			// float xStep = _steps[_currentStep] + _currentOffset * _period;
			// _currentOffset = (int) Math.Floor(xStep / _period);
			_currentOffset++;
		}
		
		// -------------------------------------------------------------------------------------------------------------

		public void Save(GameDataWriter writer)
		{
			writer.Write(_currentOffset);
			writer.Write(_currentStep);
			writer.Write(_currentObjective);
		}

		public void Load(GameDataReader reader)
		{
			_currentOffset = reader.ReadInt();
			_currentStep = reader.ReadInt();
			_currentObjective = reader.ReadInt();
		}
	}
}