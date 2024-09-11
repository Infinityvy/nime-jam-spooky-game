using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public Vector3 axisA = Vector3.one;

    public bool lerpingAxis = false;
    public float lerpSpeed = 1.0f;
    public Vector3 axisB = Vector3.one;

    void Update()
    {
        float t = (Mathf.Sin(Time.time * lerpSpeed) + 1.0f) * 0.5f * (lerpingAxis ? 1 : 0);

        transform.Rotate(Vector3.Lerp(axisA, axisB, t), rotationSpeed * Time.deltaTime);
    }
}
