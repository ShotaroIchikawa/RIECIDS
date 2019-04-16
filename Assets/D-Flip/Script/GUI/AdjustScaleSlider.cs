using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustScaleSlider : MonoBehaviour {

    Slider slider;
    public GameObject text;
    public void OnValueChanged()
    {
        PhotoManager.Instance.AdjustScaleMax = slider.value;
        text.GetComponent<Text>().text = slider.value.ToString("0");
    }

    // Use this for initialization
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.maxValue = 50;
        slider.minValue = PhotoManager.Instance.AdjustScaleMin;
        slider.value = PhotoManager.Instance.AdjustScaleMax;
    }
}
