using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueSetter : MonoBehaviour
{
    public TextMeshProUGUI valueText;

    public Slider slider;

    private void Start()
    {
        updateValue();
    }

    public void updateValue()
    {
        valueText.text = ((int)slider.value).ToString();
    }
}
