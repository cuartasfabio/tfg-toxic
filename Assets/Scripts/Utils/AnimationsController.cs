using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Text;
using MbsUnity.Runtime.Common;
using MbsUnity.Runtime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Utils
{
	public class AnimationsController : SingletonBehaviour<AnimationsController>
	{
		// public static AnimationsController Instance { get; private set; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void Initialize()
		{
			new GameObject($"#{nameof(AnimationsController)}").AddComponent<AnimationsController>();
			// DontDestroyOnLoad(Instance);
		}
        
		private const float _timeStep = 0.01f;


        public static IEnumerator EaseDoF(DepthOfField dof, float start, float end, Func<float, float> easeFunction,
            float duration = 1f)
        {
            float t = 0f;

            while (t < duration)
            {                                                        
                dof.aperture.value = MbsMath.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator EaseImageFillAmount(Image image,float start, float end, Func<float, float> easeFunction,
            float duration = 1f)
        {
            float t = 0f;

            while (t < duration)
            {                                                        
                image.fillAmount = MbsMath.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator EaseScoreText(TextMeshProUGUI scoreText, int startScore, int endScore, int obj, Func<float, float> easeFunction,
            float duration = 1f)
        {
            float t = 0f;

            while (t < duration)
            {       
                scoreText.text = Math.Round(MbsMath.Lerp(startScore, endScore, easeFunction(t / duration))).ToString(CultureInfo.InvariantCulture);
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        

        // TMP_Text effects -------------------------------------------------
        
        public static IEnumerator FadeInTexts(List<TMP_Text> texts, Func<float,float> easeFunction, float duration = 0.1f, float offset = 0.02f)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                Get().StartCoroutine(TextFadingIn(texts[i], easeFunction, duration));
                yield return new WaitForSeconds(offset);
            }
            yield return null;
        }
        
        public static IEnumerator FadeOutTexts(List<TMP_Text> texts, Func<float,float> easeFunction, float duration = 0.1f, float offset = 0.02f)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                Get().StartCoroutine(TextFadingOut(texts[i], easeFunction, duration));
                yield return new WaitForSeconds(offset);
            }
            yield return null;
        }

        public static IEnumerator TextFadingIn(TMP_Text txt, Func<float,float> easeFunction, float duration = 1f)
        {
            float t = 0f;

            Color btn = txt.color;
            Color start = new Color(btn.r, btn.g, btn.b, 0);
            Color end = new Color(btn.r, btn.g, btn.b, 1);
            
            while (t < duration)
            {
                txt.color = Color.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator TextFadingOut(TMP_Text txt, Func<float,float> easeFunction, float duration = 1f)
        {
            float t = 0f;

            Color btn = txt.color;
            Color start = new Color(btn.r, btn.g, btn.b, 1);
            Color end = new Color(btn.r, btn.g, btn.b, 0);
            
            while (t < duration)
            {
                txt.color = Color.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }

        public static IEnumerator ImageAlphaFadeOut(Image img, Func<float,float> easeFunction, float duration = 1f, float initialDelay = 0.0f)
        {
            yield return new WaitForSeconds(initialDelay);
            
            float t = 0f;

            Color color = img.color;
            Color start = new Color(color.r, color.g, color.b, 1);
            Color end = new Color(color.r, color.g, color.b, 0);
            
            while (t < duration)
            {
                img.color = Color.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator ImageAlphaFadeIn(Image img, Func<float,float> easeFunction, float duration = 1f, float initialDelay = 0.0f)
        {
            yield return new WaitForSeconds(initialDelay);
            
            float t = 0f;

            Color color = img.color;
            Color start = new Color(color.r, color.g, color.b, 0);
            Color end = new Color(color.r, color.g, color.b, 1);
            
            while (t < duration)
            {
                img.color = Color.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator ImageColorFadeOut(Image img, Func<float,float> easeFunction, float duration = 1f, float initialDelay = 0.0f)
        {
            yield return new WaitForSeconds(initialDelay);
            
            float t = 0f;
            
            Color start = new Color(1, 1, 1, 1);
            Color end = new Color(0, 0, 0, 1);
            
            while (t < duration)
            {
                img.color = Color.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator ImageColorFadeIn(Image img, Func<float,float> easeFunction, float duration = 1f, float initialDelay = 0.0f)
        {
            yield return new WaitForSeconds(initialDelay);
            
            float t = 0f;

            Color start = new Color(0, 0, 0, 1);
            Color end = new Color(1, 1, 1, 1);
            
            while (t < duration)
            {
                img.color = Color.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator MoveTransform(Transform transform, Vector3 start, Transform end, Func<float,float> easeFunction, float duration = 1f )
        {
            float t = 0f;
            
            while (t < duration)
            {                                                        
                transform.position = MbsMath.Lerp(start, end.position, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator MoveTransformParabolic(Transform transform, Vector3 start, Transform end, Func<float,float> easeFunction, float duration = 1f )
        {
            float t = 0f;
            Vector3 startEndPos = new Vector3(start.x,start.y + 3f,start.z);
            
            while (t < duration)
            {                                                         
                transform.position = MbsMath.Lerp(start, MbsMath.Lerp(startEndPos,end.position, TfMath.EaseLinear(t/duration)), easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        // rect transform effects -------------------------------------------------
        
        public static IEnumerator MoveUIElement(RectTransform rect, Vector3 direction, float magnitude, Func<float,float> easeFunction, float duration = 1f )
        {
            float t = 0f;

            Vector3 start = rect.anchoredPosition;
            Vector3 end = start;
            MbsMath.Vector.VectorAdd(ref end, direction * magnitude);
            
            while (t < duration)
            {                                                        
                rect.anchoredPosition = MbsMath.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator MoveUIElementY(RectTransform rect, float start, float end, Func<float,float> easeFunction, float duration = 1f )
        {
            float t = 0f;
            
            while (t < duration)
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x,
                    MbsMath.Lerp(start, end, easeFunction(t / duration)));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator MoveUIElementX(RectTransform rect, float start, float end, Func<float,float> easeFunction, float duration = 1f )
        {
            float t = 0f;
            
            while (t < duration)
            {
                rect.anchoredPosition = new Vector2(MbsMath.Lerp(start, end, easeFunction(t / duration)),
                    rect.anchoredPosition.y);
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator MoveUIElementToTransform(RectTransform rect, RectTransform objective, Func<float,float> easeFunction, float duration = 1f )
        {
            float t = 0f;

            Vector3 start = rect.position;
            
            while (t < duration)
            {
                rect.position = MbsMath.Lerp(start, objective.position, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator RotateUIElement(RectTransform rect, float start, float end, Func<float,float> easeFunction, float duration = 1f )
        {
            float t = 0f;
            Quaternion initRot = rect.localRotation;
            
            while (t < duration)
            {
                rect.localRotation = new Quaternion(initRot.x,initRot.y,MbsMath.Lerp(start, end, easeFunction(t / duration)),1);
                // rect.Rotate( new Vector3(0,0,MbsMath.Lerp(start, end, easeFunction(t / duration))));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator MoveUiElements(List<RectTransform> rects, Vector3 direction, float magnitude, Func<float,float> easeFunction,  float duration = 0.1f, float offset = 0.02f)
        {
            for (int i = 0; i < rects.Count; i++)
            {
                Get().StartCoroutine(MoveUIElement(rects[i], direction, magnitude, easeFunction, duration));
                yield return new WaitForSeconds(offset);
            }
            yield return null;
        }
        
        public static IEnumerator ScaleUiElement(RectTransform rect, float magnitude, Func<float,float> easeFunction, float duration = 1f )
        {
            float t = 0f;

            Vector3 start = rect.localScale;
            Vector3 end = start * magnitude;
            
            while (t < duration)
            {                                                         
                rect.localScale = MbsMath.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator ScaleUiElement(RectTransform rect, Vector3 final, Func<float,float> easeFunction, float duration = 1f)
        {
            float t = 0f;

            Vector3 start = rect.localScale;
            Vector3 end = final;

            while (t < duration)
            {
                rect.localScale = MbsMath.Lerp(start, end, easeFunction(t / duration));
                // rect.localScale = new Vector3(            
                //     MbsMath.Lerp(start.x, end.x, easeFunction(t / duration)),
                //     MbsMath.Lerp(start.y, end.y, easeFunction(t / duration)),
                //     MbsMath.Lerp(start.z, end.z, easeFunction(t / duration)));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator ScaleUiElement(RectTransform rect, Vector3 start, Vector3 end, Func<float,float> easeFunction, float duration = 1f)
        {
            float t = 0f;
            
            while (t < duration)
            {
                // rect.localScale = new Vector3(            
                //     MbsMath.Lerp(start.x, end.x, easeFunction(t / duration)),
                //     MbsMath.Lerp(start.y, end.y, easeFunction(t / duration)),
                //     MbsMath.Lerp(start.z, end.z, easeFunction(t / duration)));
                rect.localScale = MbsMath.Lerp(start, end, easeFunction(t / duration));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator ScaleUiElement(Transform tfrm, Vector3 final, Func<float,float> easeFunction, float duration = 1f)
        {
            float t = 0f;

            Vector3 start = tfrm.localScale;
            Vector3 end = final;
            
            while (t < duration)
            {
                tfrm.localScale = new Vector3(            
                    MbsMath.Lerp(start.x, end.x, easeFunction(t / duration)),
                    MbsMath.Lerp(start.y, end.y, easeFunction(t / duration)),
                    MbsMath.Lerp(start.z, end.z, easeFunction(t / duration)));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator ScaleUiElementHeight(RectTransform rect, float start, float end, Func<float,float> easeFunction, float duration = 1f)
        {
            float t = 0f;
            
            while (t < duration)
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, MbsMath.Lerp(start, end, easeFunction(t / duration)));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator EaseMaterialFloat(Material mat, string floatName, float start, float end, Func<float,float> easeFunction, float duration = 1f )
        {
            float t = 0f;
            
            while (t < duration)
            {
                mat.SetFloat(floatName, MbsMath.Lerp(start, end, easeFunction(t / duration)));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator EaseMaterialFloat(Material mat, string floatName, float end, Func<float,float> easeFunction, float duration = 1f )
        {
            float t = 0f;
            float start = mat.GetFloat(floatName);
            
            while (t < duration)
            {
                mat.SetFloat(floatName, MbsMath.Lerp(start, end, easeFunction(t / duration)));
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        
        
        // public static IEnumerator RotateUiElement(RectTransform rect, float endZRotation, float duration = 1f)
        // {
        //     float t = 0f;
        //
        //     float start = rect.rotation.z;
        //     float end = start + endZRotation;
        //
        //     float value;
        //     float v = 0.0f;
        //     while (t < duration)
        //     {
        //         value = t / duration;
        //         TfMath.Spring(ref value, ref v, end,4f, 8f, _timeStep);
        //         
        //         rect.rotation = new Quaternion(
        //             rect.rotation.x,
        //             rect.rotation.y,
        //             MbsMath.Lerp(start, end, value), 
        //             rect.rotation.w);
        //         
        //         //MbsMath.Lerp(start, end, value)
        //         t += _timeStep;
        //         yield return new WaitForSeconds(_timeStep);
        //     }
        // }
        
        public static IEnumerator MoveYElementSpring(RectTransform rect, float magnitude, float omega, float zeta, float duration = 1f )
        {
            float t = 0f;
            
            float value = rect.position.y;
            float v = 0.0f;
            float end =  value + magnitude;

            while (t < duration)
            {
                TfMath.Spring(ref value, ref v, end,zeta, omega, _timeStep);
                
                Vector3 movement = rect.position;
                movement.y = value;
                rect.position = movement;
                
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator MoveXElementSpring(RectTransform rect, float magnitude, float omega, float zeta, float duration = 1f )
        {
            float t = 0f;
            
            float value = rect.position.x;
            float v = 0.0f;
            float end =  value + magnitude;

            while (t < duration)
            {
                TfMath.Spring(ref value, ref v, end,zeta, omega, _timeStep);
                
                Vector3 movement = rect.position;
                movement.x = value;
                rect.position = movement;
                
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
        
        public static IEnumerator MoveZElementSpring(RectTransform rect, float magnitude, float omega, float zeta, float duration = 1f )
        {
            float t = 0f;
            
            float value = rect.position.z;
            float v = 0.0f;
            float end =  value + magnitude;

            while (t < duration)
            {
                TfMath.Spring(ref value, ref v, end,zeta, omega, _timeStep);
                
                Vector3 movement = rect.position;
                movement.z = value;
                rect.position = movement;
                
                t += _timeStep;
                yield return new WaitForSeconds(_timeStep);
            }
        }
    }
}