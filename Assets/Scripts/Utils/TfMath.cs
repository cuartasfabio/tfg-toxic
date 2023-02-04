using System;
using UnityEngine;

namespace Utils
{
	public static class TfMath
	{
		public static float EaseOutExpo(float x)
		{
			return Math.Abs(x - 1.0f) < 0.01f ? 1.0f : 1.0f - Mathf.Pow(2, -10 * x);
		}
		
		public static float EaseOutQuad(float x)
		{
			return 1 - (1 - x) * (1 - x);
		}
		
		public static float EaseLinear(float x)
		{
			return x;
		}
		
		public static float EaseInQuad(float x)
		{
			return x * x;
		}
		
		public static float EaseInCubic(float x)
		{
			return x * x * x;
		}
		
		public static float EaseOutCubic(float x)
		{
			return (float) (1 - Math.Pow(1 - x, 3));
		}

		public static float EaseOutBack(float x)
		{
			float c1 = 1.70158f;
			float c3 = c1 + 1f;
			
			return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
		}
		
		public static float EaseOutCirc(float x)
		{
			return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
		}
		
		public static float EaseInBack(float x)
		{
			float c1 = 1.70158f;
			float c3 = c1 + 1f;
			
			return c3 * x * x * x - c1 * x * x;
		}
		
		public static float EaseInOutCubic(float x)
		{
			return x < 0.5f ? 4.0f * x * x * x : 1.0f - Mathf.Pow(-2.0f * x + 2, 3) / 2.0f;
		}
		
		public static float EaseInOutQuint(float x)
		{
			return x < 0.5f ? 16.0f * x * x * x * x * x : 1.0f - Mathf.Pow(-2.0f * x + 2, 5) / 2.0f;
		}

		public static float EaseOutQuint(float x)
		{
			return 1.0f - Mathf.Pow(1.0f - x, 5);
		}
		
		public static float EaseInQuint(float x)
		{
			return x * x * x * x * x;
		}
		
		public static float EaseOutElastic(float x)
		{
			float c4 = (2 * Mathf.PI) / 3;

			return x == 0.0f 
				? 0
				: Math.Abs(x - 1.0f) < 0.01f
				? 1
				: Mathf.Pow(2.0f, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
		}

		public static float BellEaseCentered(float x)
		{
			return ((Mathf.Sin(2 * Mathf.PI * x) + 1) / 2f) - 0.5f;
		}

		public static float BellEase(float x)
		{
			return (Mathf.Sin(2 * Mathf.PI * (x - 1/4f)) + 1) / 2f;
		}
		
		public static float EaseInOutSine(float x)
		{
			return -(Mathf.Cos(Mathf.PI * x) - 1) / 2f;
		}

		public static Vector3 Round(Vector3 v, int d)
		{
			return new Vector3((float) Math.Round(v.x, d), (float) Math.Round(v.y, d), (float) Math.Round(v.z, d));
		}
		
		/// <summary>
		/// Numeric springing: http://allenchou.net/2015/04/game-math-precise-control-over-numeric-springing/
		/// </summary>
		/// <param name="x">Value (input/output)</param>
		/// <param name="v">Velocity (input/output)</param>
		/// <param name="xt">Target value</param>
		/// <param name="zeta">Damping ratio</param>
		/// <param name="omega">Angular frequency</param>
		/// <param name="h">Time step</param>
		public static void Spring(ref float x, ref float v, float xt, float zeta, float omega, float h)
		{
			float f = 1.0f + 2.0f * h * zeta * omega;
			float oo = omega * omega;
			float hoo = h * oo;
			float hhoo = h * hoo;
			float detInv = 1.0f / (f + hhoo);
			float detX = f * x + h * v + hhoo * xt;
			float detV = v + hoo * (xt - x);
			x = detX * detInv;
			v = detV * detInv;
		}
	}
}