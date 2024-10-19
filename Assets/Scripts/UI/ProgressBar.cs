using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public static ProgressBar instance { get; private set; }

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI textMesh;
    [SerializeField]
    private Camera mainCam;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public float getValue()
    {
        return slider.value;
    }

    public void setValue(float value)
    {
        slider.value = value;
    }

    public void setText(string text)
    {
        textMesh.text = text;
    }

    public void clearText()
    {
        textMesh.text = "";
    }

    /// <summary>
    /// Automatically converted from world to screen position.
    /// </summary>
    /// <param name="pos"></param>
    public void setPosition(Vector3 pos)
    {
        transform.position = mainCam.WorldToScreenPoint(pos);
    }
}
