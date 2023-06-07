using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorAndFontSizeAnimation : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    public Color startTextColor = Color.black;
    public Color endTextColor = Color.black;

    public float startFontSize = 20f;
    public float endFontSize = 40f;
    public float animationDuration = 1f;

    private bool isAnimating = false;

    private void Start()
    {
        // Set the initial font size
        textMeshPro.color = startTextColor;
        textMeshPro.fontSize = startFontSize;
    }

    public void StartAnimation()
    {
        // Check if an animation is already in progress
        if (isAnimating)
            return;

        // Start the animation
        LeanTween.value(gameObject, endTextColor, startTextColor, animationDuration)
                    .setOnUpdate(UpdateColor)
                    .setOnComplete(ReverseAnimation);

        
        LeanTween.value(gameObject, startFontSize, endFontSize, animationDuration)
            .setOnUpdate(UpdateFontSize)
            .setOnComplete(ReverseAnimation);
        

        isAnimating = true;
    }

    private void UpdateFontSize(float fontSize)
    {
        // Update the font size during the animation
        textMeshPro.fontSize = fontSize;
    }

    private void UpdateColor(Color textColor)
    {
        // Update the color during the animation
        textMeshPro.color = textColor;
    }

    private void ReverseAnimation()
    {
        // Start the reverse animation


        LeanTween.value(gameObject, endTextColor, startTextColor, animationDuration)
                    .setOnUpdate(UpdateColor);
                    //.setOnComplete(AnimationComplete);

        
        LeanTween.value(gameObject, endFontSize, startFontSize, animationDuration)
            .setOnUpdate(UpdateFontSize)
            .setOnComplete(AnimationComplete);
        

        isAnimating = true;
    }

    private void AnimationComplete()
    {
        // Animation is complete
        isAnimating = false;
    }
}
