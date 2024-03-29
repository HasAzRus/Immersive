using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Oxygen
{
    public class ProcedureAnimation
    {
        public static IEnumerator PlayRoutine(float duration, Action<float> valueChanged, Action completed)
        {
            var elapsedTime = 0f;
            float progress = 0;

            while (progress <= 1)
            {
                valueChanged?.Invoke(progress);

                elapsedTime += Time.deltaTime;
                progress = elapsedTime / duration;

                yield return null;
            }
            
            valueChanged?.Invoke(1f);
            
            completed?.Invoke();
        }

        public static IEnumerator CrossFadeImage(float alpha, float duration, Image image)
        {
            yield return PlayRoutine(duration, deltaTime =>
            {
                var color = image.color;
                color.a = Mathf.Lerp(color.a, alpha, deltaTime);

                image.color = color;
            }, null);
        }

        public static IEnumerator CrossFadeLight(float intensity, float duration, Light light)
        {
            yield return PlayRoutine(duration, deltaTime =>
            {
                var currentIntensity = light.intensity;
                currentIntensity = Mathf.Lerp(currentIntensity, intensity, deltaTime);

                light.intensity = currentIntensity;
            }, null);
        }
    }
}