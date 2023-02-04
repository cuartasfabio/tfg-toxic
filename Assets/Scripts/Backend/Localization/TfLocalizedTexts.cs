using System.Collections.Generic;
using TMPro;

namespace Backend.Localization
{
	public static class TfLocalizedTexts
	{
		/// <summary>
		/// Texts on screen to be updated when loc changes.
		/// </summary>
		private static List<(TMP_Text, string)> _toUpdateLocTexts = new List<(TMP_Text, string)>();

		/// <summary>
		/// Gets the raw localized string for a given stringId and stores
		/// it in the text list.
		/// </summary>
		/// <param name="tmpText">TextMeshPro object to update.</param>
		/// <param name="stringId">StringId for the text.</param>
		/// <returns>The localized string.</returns>
		public static string GetStringRaw(TMP_Text tmpText, string stringId)
		{
			_toUpdateLocTexts.Add((tmpText, stringId));
			return StringBank.GetStringRaw(stringId);
		}

		/// <summary>
		/// Updates every stored localized text to the current loc.
		/// </summary>
		public static void RefreshTexts()
		{
			for (int i = _toUpdateLocTexts.Count - 1; i >= 0; i--)
			{
				if (_toUpdateLocTexts[i].Item1 != null)
				{
					_toUpdateLocTexts[i].Item1.text = StringBank.GetStringRaw(_toUpdateLocTexts[i].Item2);
				}
				else
				{
					_toUpdateLocTexts.RemoveAt(i);
				}
			}
		}
	}
}