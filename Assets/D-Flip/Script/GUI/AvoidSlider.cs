using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvoidSlider : MonoBehaviour {

    Slider slider;
    public GameObject text;
    public void OnValueChanged()
    {
        SystemManager.Instance.weight.AvoidWeight = slider.value;
        text.GetComponent<Text>().text = slider.value.ToString("0.0");
    }


	// Use this for initialization
	void Awake () {
        slider = gameObject.GetComponent<Slider>();
    }
}
