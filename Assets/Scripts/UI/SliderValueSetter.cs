using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueSetter : MonoBehaviour
{
    public AudioGroup audioGroup;

    public TextMeshProUGUI valueText;

    public Slider slider;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat(audioGroup.ToString() + "Volume", 50f);

        updateValue();
    }

    public void updateValue()
    {
        valueText.text = ((int)slider.value).ToString();
    }
}
