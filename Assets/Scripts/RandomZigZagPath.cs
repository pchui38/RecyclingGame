using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomZigZagPath : MonoBehaviour
{

    public int numberOfPoints = 10; // The number of points in the path
    public float pathWidth = 5f; // The width of the path
    public float minZigZagAmplitude = 0.5f; // The minimum amplitude of the zigzags
    public float maxZigZagAmplitude = 2f; // The maximum amplitude of the zigzags

    private LineRenderer lineRenderer; // The LineRenderer component

    void Start()
    {
        // Get the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();

        // Set the number of points in the path
        lineRenderer.positionCount = numberOfPoints;

        // Generate the points for the path
        for (int i = 0; i < numberOfPoints; i++)
        {
            float x = (float)i / (numberOfPoints - 1) * pathWidth - pathWidth / 2f;
            float z = Random.Range(minZigZagAmplitude, maxZigZagAmplitude) * Mathf.Sign(Random.Range(-1f, 1f));
            Vector3 position = new Vector3(x, 0f, z);
            lineRenderer.SetPosition(i, position);
        }

        // Set the width of the path
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }
}

