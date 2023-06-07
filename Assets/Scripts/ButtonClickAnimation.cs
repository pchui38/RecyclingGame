using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float scaleAmount = 1.1f;
    public float duration = 0.2f;

    private Vector3 originalScale;
    private bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        StartCoroutine(AnimateButtonDown());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        StartCoroutine(AnimateButtonUp());
    }

    IEnumerator AnimateButtonDown()
    {
        float elapsed = 0f;
        while (isPressed && elapsed < duration)
        {
            float t = elapsed / duration;
            float scale = Mathf.Lerp(originalScale.x, originalScale.x * scaleAmount, t);
            transform.localScale = new Vector3(scale, scale, scale);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator AnimateButtonUp()
    {
        float elapsed = 0f;
        while (!isPressed && elapsed < duration)
        {
            float t = elapsed / duration;
            float scale = Mathf.Lerp(transform.localScale.x, originalScale.x, t);
            transform.localScale = new Vector3(scale, scale, scale);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (!isPressed)
        {
            transform.localScale = originalScale;
        }
    }

}
