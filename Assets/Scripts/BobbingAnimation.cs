using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float amp = 10f;
    public float freq = 1f;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        StartCoroutine(Bobbing());
    }

    IEnumerator Bobbing()
    {
        while (true)
        {
            float newY = startPosition.y + amp * Mathf.Sin(freq * Time.time);
            transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
            yield return null;
        }
    }
}
