using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContaminationBar : MonoBehaviour
{
    public Slider slider;

    public float animationDuration = 1f;
    public float startValue = 0f;
    public float endValue = 1f;
    private float currentTime = 0f;
    private bool isAnimating = false;

    /*
    public void SetMaxContamination(int maxContamination)
    {
        slider.maxValue = maxContamination;
        slider.value = maxContamination;
    }
    */

    public void SetContamination(int contamination)
    {
        slider.value = contamination;
    }

    public void SetAnimatedContamination(int previousContaminationAmount, int newContaminationAmount)
    {
        // Reset animation variables
        currentTime = 0f;
        isAnimating = true;

        // Run animated slider
        StartCoroutine(AnimateSlider(previousContaminationAmount, newContaminationAmount));
    }

    private IEnumerator AnimateSlider(float startValue, float endValue)
    {
        while (isAnimating)
        {
            currentTime += Time.deltaTime;

            if (currentTime <= animationDuration)
            {
                float t = currentTime / animationDuration;
                float animatedValue = Mathf.Lerp(startValue, endValue, t);

                slider.value = animatedValue;
            }
            else
            {
                // Animation complete
                isAnimating = false;
            }

            yield return null;
        }
    }
}
