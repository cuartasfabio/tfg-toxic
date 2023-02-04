using System;
using System.Collections;
using Audio;
using Backend.Persistence;
using Cysharp.Text;
using Gameplay.Commands.GameLogic;
using Gameplay.Grids.Hexes.HexHelpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Gameplay.Scores
{
	public class ScoreManager : MonoBehaviour, IPersistable
	{
		[SerializeField] private TextMeshProUGUI _tmpCurrentScore;
		[SerializeField] private TextMeshProUGUI _tmpObjectiveScore;
		[SerializeField] private RectTransform _hexRect;
		
		[SerializeField] private Image _scoreCircle;
		[SerializeField] private Image _scoreCirclePreview;

		[SerializeField] private Material _fontStdMat;
		[SerializeField] private Material _fontUpGlowMat;
		[SerializeField] private Material _fontDownGlowMat;
		

		[Serializable]
		public class CurrencyAssets
		{
			public Sprite _spr;
			public Material _mat;
		}
		
		[Space]
		[SerializeField] private CurrencyAssets[] _currencyVariations;

		private DynamicScoreObjectives _dso;

		private int _currentScore;

		private Transform _pointsAnimationEnd;

		private AudioId _scoreSfx;

		private int _currentObjective;

		public void Init(float difficultyRatio)
		{
			_currentScore = 0;
			_dso = new DynamicScoreObjectives(difficultyRatio);
			_currentObjective = _dso.GetCurrentObjective();
			// RefreshScoreUI(0);
			StartCoroutine(RefreshScoreUI(0));
			
			_pointsAnimationEnd = new GameObject("PointsAnchor").transform;
			_pointsAnimationEnd.SetParent(ObjectCache.Current.MainCamera.gameObject.transform);
			_pointsAnimationEnd.localPosition = new Vector3(-1.1f, -0.55f, 1.67f);
		}
		
		
		public void UpdateScore(int amount)
		{
			GameManager.Get().RunManager.SetGlobalScore(amount);
			
			if (_currentScore + amount >= _currentObjective) // supera el objetivo
			{
				ObjectCache.Current.CommandBuffer.EnqueueCommand(new RandomCardsToHandCommand(3));
				
				int reminder = _currentScore + amount - _currentObjective;
				_dso.UpdateObjective();
				// primer objetivo actualizado
				while (reminder >= _dso.GetCurrentObjective()) // mientras siga siendo mayor que el siguiente objetivo
				{
					reminder = amount - _dso.GetCurrentObjective();
					_dso.UpdateObjective();
				}
				// tenemos el reminder y el objetivo actualizados

				StartCoroutine(GoBeyondObjective(_currentObjective, reminder));
				
			} else // no supera el objetivo
			{
				if (_currentScore + amount < 0)
					_currentScore = 0;
				else
					_currentScore += amount;
				
				StartCoroutine(RefreshScoreUI(amount));
			}
		}
		
		private IEnumerator GoBeyondObjective(int obj, int reminder)
		{
			// score up to objective
			int toObj = obj - _currentScore;
			_currentScore = obj;
			yield return StartCoroutine(RefreshScoreUI(toObj));
				
			// objective reached
			ObjectiveReached();
				
			// score and ui reset to 0
			_currentScore = 0;
			yield return StartCoroutine(RefreshScoreUI(0));
				
			// score up reminder
			_currentScore = reminder;
			yield return StartCoroutine(RefreshScoreUI(reminder));
		}

		
		private void ObjectiveReached()
		{
			_currentObjective = _dso.GetCurrentObjective();
			AudioController.Get().PlaySfx(AudioId.SFX_ScoreObjective);
		}


		private IEnumerator RefreshScoreUI(int amount)
		{
			// updating circle fill and text
			
			float scale = (float)_currentScore / _currentObjective;

			if (amount != 0)
			{
				_scoreSfx = AudioId.SFX_ScoreUp;
				Material textMat = _fontUpGlowMat;
				if (amount < 0)
				{
					textMat = _fontDownGlowMat;
					_scoreSfx = AudioId.SFX_ScoreDown;
				}
				yield return StartCoroutine(ScoreUpDownAnimation(scale, amount, textMat));
			}
			else
			{
				// _tmpObjectiveScore.text = ZString.Concat(_currentScore, "/", _currentObjective);
				_tmpCurrentScore.text = _currentScore.ToString();
				_tmpObjectiveScore.text = _currentObjective.ToString();
				_scoreCircle.fillAmount = scale;
				_scoreCirclePreview.fillAmount = scale;
				yield return null;
			}
		}

		private IEnumerator ScoreUpDownAnimation(float newScale, int amount, Material glowMat)
		{
			int start = _currentScore - amount;
			int end = _currentScore;
			
			yield return new WaitForSeconds(1f);
			
			// start shake coroutine
			_tmpCurrentScore.fontSharedMaterial = glowMat;
			IEnumerator shakeCoroutine = ScoreShakeAnimation();
			StartCoroutine(shakeCoroutine);

			StartCoroutine(AnimationsController.EaseScoreText(_tmpCurrentScore,start, end,  _currentObjective, TfMath.EaseLinear, 0.5f));
			StartCoroutine(AnimationsController.EaseImageFillAmount(_scoreCircle, _scoreCircle.fillAmount, newScale, TfMath.EaseLinear, 0.5f));
			yield return StartCoroutine(AnimationsController.EaseImageFillAmount(_scoreCirclePreview, _scoreCirclePreview.fillAmount, newScale, TfMath.EaseLinear, 0.5f));
			
			// stop shake coroutine when circle fully updated
			StopCoroutine(shakeCoroutine);
			yield return new WaitForSeconds(0.1f);
			_tmpCurrentScore.fontSharedMaterial = _fontStdMat;
			_hexRect.localRotation = Quaternion.identity;
			_hexRect.localScale = Vector3.one;
		}

		private IEnumerator ScoreShakeAnimation()
		{
			_hexRect.localScale = Vector3.one * 1.1f;
			while (true)
			{
				// StartCoroutine(AnimationsController.ScaleUiElement(_textRect, 1.15f, TfMath.BellEaseCentered, 0.05f));
				yield return StartCoroutine(AnimationsController.RotateUIElement(_hexRect, 0.0f, 0.05f, TfMath.BellEaseCentered, 0.035f));
			}
		}

		
		public void UpdateScorePreview(int amount)
		{
			// _tmpObjectiveScore.text = ZString.Concat(_currentScore + amount, "/", _currentObjective);
			_tmpCurrentScore.text = _currentScore.ToString();
			_tmpObjectiveScore.text = _currentObjective.ToString();
			
			// updateing preview circle
			float scale = (float)(_currentScore + amount) / _currentObjective;
			_scoreCirclePreview.fillAmount = scale;
		}
		
		public void ResetScorePreview(int amount)
		{
			// _tmpObjectiveScore.text = ZString.Concat(_currentScore, "/", _currentObjective);
			_tmpCurrentScore.text = _currentScore.ToString();
			_tmpObjectiveScore.text = _currentObjective.ToString();
			
			// updateing preview circle
			float scale = (float)(_currentScore - amount) / _currentObjective;
			_scoreCirclePreview.fillAmount = scale;
		}


		public void ExtractCrystalPoints(int points, HexCoordinates coords)
		{
			StartCoroutine(ExtractCrystalPointsAnimation(points, HexCoordinates.ToPosition(coords)));
		}
		
		private IEnumerator ExtractCrystalPointsAnimation(int points, Vector3 origin)
		{
			for (int i = 0; i < points; i++)
			{
				StartCoroutine(MoveSingleCrystal(origin));
				
				yield return new WaitForSeconds(0.08f);
			}
		}

		private IEnumerator MoveSingleCrystal(Vector3 origin)
		{
			SpriteRenderer shard = new GameObject("Score Point").AddComponent<SpriteRenderer>();
			Transform shardTrf = shard.transform;
			shardTrf.position = origin;
			CurrencyAssets cs = _currencyVariations[RandomTf.Rng.Next(_currencyVariations.Length)];
			shard.sprite = cs._spr;
			shard.material = cs._mat;

			shardTrf.localScale = new Vector3(0.75f, 0.75f, 0.75f);
			StartCoroutine(AnimationsController.ScaleUiElement(shardTrf, new Vector3(0.2f, 0.2f, 0.2f), TfMath.EaseLinear, 0.5f));

			yield return StartCoroutine(
				AnimationsController.MoveTransformParabolic(shardTrf, origin, _pointsAnimationEnd, TfMath.EaseOutQuad, 0.5f));
			
			AudioController.Get().PlaySfx(_scoreSfx);
				
			Destroy(shardTrf.gameObject);
		}

		// -------------------------------------------------------------------------------------------------------------

		public void Save(GameDataWriter writer)
		{
			writer.Write(_currentScore);
			
			_dso.Save(writer);
		}

		public void Load(GameDataReader reader)
		{
			_currentScore = reader.ReadInt();
			
			_dso.Load(reader);
			_currentObjective = _dso.GetCurrentObjective();
			
			LoadScoreUI();
		}

		private void LoadScoreUI()
		{
			float scale = (float)_currentScore / _currentObjective;
			// _tmpObjectiveScore.text = ZString.Concat(_currentScore, "/", _currentObjective);
			_tmpCurrentScore.text = _currentScore.ToString();
			_tmpObjectiveScore.text = _currentObjective.ToString();
			_scoreCircle.fillAmount = scale;
			_scoreCirclePreview.fillAmount = scale;
		}
	}
}