using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ZAxisSlider : MonoBehaviour {
    public Vector3 zSliderPosition;
    public static ZAxisSlider Instance;
    Slider slider;
    public Text showValueText;

    public void OnValueChanging()
    {
        if (SystemManager.zAxisDirection == true)
        {
            zSliderPosition = Vector3.forward * 10 * ((slider.value - slider.minValue) / (slider.maxValue - slider.minValue)) - Vector3.forward / 100;
        }
        else
        {
            zSliderPosition = Vector3.forward * 10 * ((slider.value - slider.minValue) / (slider.maxValue - slider.minValue)) + Vector3.forward / 100;
        }

        if (SystemManager.keys[2].Contains("年月") || SystemManager.keys[2].Contains("[Y:M]") || SystemManager.keys[2].Contains("date"))
        {
            showValueText.text = PhotoManager.Instance.DeserializeYM(slider.value);
        }
        else
        {
            showValueText.text = slider.value.ToString("N");
        }
    }

    public Camera zCamera;
	// Use this for initialization
	void Start () {
        slider = gameObject.GetComponent<Slider>();
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
