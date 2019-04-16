using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoSizeAdjuster : MonoBehaviour {

    Slider slider;
    public GameObject text;
    public void OnValueChanged()
    {
        PhotoManager.Instance.Max = slider.value;
        text.GetComponent<Text>().text = slider.value.ToString("0.0");
    }

    // Use this for initialization
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.minValue = PhotoManager.Instance.Min * 3;
        slider.value = PhotoManager.Instance.Max *3;
    }
}
