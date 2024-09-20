using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public static Flashlight instance;

    private new Light light;

    void Start()
    {
        light = GetComponent<Light>();
        instance = this;
    }

    void Update()
    {
        rotateFlashlightToCursor();
    }

    public void toggleFlashlight(bool state)
    {
        light.enabled = state;
    }

    private void rotateFlashlightToCursor()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Vector3 mousePosition = clampMousePosition(Input.mousePosition);

        float xOffset = mousePosition.x - screenCenter.x;
        float zOffset = mousePosition.y - screenCenter.y;

        float angle = Vector3.Angle(Vector3.forward, new Vector3(xOffset, 0, zOffset));

        angle *= (xOffset > 0 ? 1 : -1);

        transform.rotation = Quaternion.Euler(30, angle + 45, 0f);
    }

    private Vector3 clampMousePosition(Vector3 mousePosition)
    {
        float x = Mathf.Clamp(mousePosition.x, 0, Screen.width);
        float y = Mathf.Clamp(mousePosition.y, 0, Screen.height);

        return new Vector3(x, y, 0);
    }
}
