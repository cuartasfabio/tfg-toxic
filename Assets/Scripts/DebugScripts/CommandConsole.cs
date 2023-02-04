using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Levels;
using Gameplay.Tiles;
using TMPro;
using UnityEngine;
using Utils;

namespace DebugScripts
{
	public class CommandConsole : MonoBehaviour
	{
		[SerializeField] private TMP_InputField _inputText;
		[SerializeField] private TMP_Text _helpText;
		private string _commandList = "- ground:\tGROUND card to clear any tile\n" +
		                              "- carns:\tCARNIVORES card to hand\n" +
		                              "- explorer:\tEXPLORER card to hand\n" +
		                              "- forest:\tFOREST card to hand\n" +
		                              "- herbs:\tHERBIVORES card to hand\n" +
		                              "- lake:\tLAKE card to hand\n" +
		                              "- campsite:\tCAMPSITE card to hand\n" +
		                              "- wastes:\tWASTES card to hand\n" + 
		                              "- purifier:\tPURIFIER card to hand\n" +
		                              "- radio:\tRADIO card to hand\n" +
		                              "- farm:\tFARM card to hand\n" +
		                              "- meadow:\tMEADOW card to hand\n" +
		                              "- lumber:\tLUMBERJACK card to hand\n" +
		                              "- swamp:\tSWAMP card to hand\n" +
		                              "- dunes:\tDUNES card to hand\n" +
		                              "- mountain:\tMOUNTAIN card to hand\n";

		public void Init()
		{
			_helpText.color = Color.clear;
		}

		public void ExecuteCommandInput()
		{
			string input = _inputText.text;
			if (input.Contains("help"))
			{
				_helpText.color = Color.white;
				_helpText.text = _commandList;
				StartCoroutine(FadeHelpText());
			}
			else if (input.Contains("complete quests"))
			{
				CompleteQuests();
			}
			else if (input.Contains("lake"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Lake, true);
			}
			else if (input.Contains("forest"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Forest, true);
			}
			else if (input.Contains("herbs"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Herbivores, true);
			}
			else if (input.Contains("carns"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Carnivores, true);
			}
			else if (input.Contains("campsite"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Campsite, true);
			}
			else if (input.Contains("explorer"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Explorer, true);
			}
			else if (input.Contains("wastes"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Wastes, true);
			}
			else if (input.Contains("ground"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Ground, true);
			}
			else if (input.Contains("mountain"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Mountain, true);
			}
			else if (input.Contains("purifier"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Purifier, true);
			}
			else if (input.Contains("radio"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.RadioTower, true);
			}
			else if (input.Contains("farm"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Farm, true);
			}
			else if (input.Contains("meadow"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Meadow, true);
			}
			else if (input.Contains("lumber"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Lumberjack, true);
			}
			else if (input.Contains("swamp"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Swamp, true);
			}
			else if (input.Contains("dunes"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Dunes, true);
			}
			else if (input.Contains("cabin"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.OldCabin, true);
			}
			else if (input.Contains("swamp"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Swamp, true);
			}
			else if (input.Contains("factory"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Factory, true);
			}
			else if (input.Contains("farmland"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Farmland, true);
			}
			else if (input.Contains("mutants"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Mutants, true);
			}
			else if (input.Contains("warped"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.WarpedWoods, true);
			}
			else if (input.Contains("monolith"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Monolith, true);
			}
			else if (input.Contains("village"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.Village, true);
			}
			else if (input.Contains("city"))
			{
				ObjectCache.Current.UiCardHand.CardToHand(TileType.City, true);
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				ExecuteCommandInput();
				CleanCommandInput();
			}
		}
		
		private void CleanCommandInput()
		{
			_inputText.text = "";
			_inputText.Select();
			_inputText.ActivateInputField();
		}

		private IEnumerator FadeHelpText()
		{
			yield return StartCoroutine(AnimationsController.TextFadingOut(_helpText, TfMath.EaseLinear, 5f));
			_helpText.color = Color.clear;
		}

		private void CompleteQuests()
		{
			GameManager.Get().RunManager.LevelManager.QuestsManager.CompleteQuestsCommand();
		}
		
	}
}