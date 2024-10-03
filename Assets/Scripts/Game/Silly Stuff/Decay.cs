using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : MonoBehaviour
{
    public float decayTime;

    private void Start()
    {
        Invoke(nameof(destroy), decayTime);
    }

    private void destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
