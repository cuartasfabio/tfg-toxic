using System;
using System.Collections.Generic;

namespace Utils
{
	public static class RandomTf // todo change for noise value?
	{
		public static readonly Random Rng = new Random();

		public static void KFYShuffle<T>(this IList<T> list)
		{
			for (int i = list.Count - 1; i > 0; i--)
			{
				int n = Rng.Next(i + 1);
				
				//swap
				(list[i], list[n]) = (list[n], list[i]);
			}
		}
	}
}