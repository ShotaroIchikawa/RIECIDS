using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AdjustScaleButton : MonoBehaviour {

    public GameObject sBar;
    public GameObject colliderToggle;
    public GameObject sSlider;

    public void MyOnClick(string key)
    {
        SetBar(key);
        SystemManager.Instance.weight.AvoidWeight = 2.5f;
        sSlider.GetComponent<Slider>().value = 20 * (1 - PhotoManager.midValue[PhotoManager.Instance.keydataCode[key]] / PhotoManager.maxValue[PhotoManager.Instance.keydataCode[key]]);
        colliderToggle.GetComponent<Toggle>().interactable = true;
        PhotoManager.Instance.sSortKey = PhotoManager.Instance.keydataCode[key];
        SystemManager.Instance.AddAttractor(SystemManager.AttractorState.ADJUST_SCALE);
        SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.AVOID_SCALE);
        SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.SCALE_UP);
        buttonS.Close();
    }

    SortButton buttonS;
    // Use this for initialization
    public void Initialize () {
        //GUIManagerから参照をもらう
        GUIManager gm = GameObject.Find("Main Camera").gameObject.GetComponent<GUIManager>();
        buttonS = gm.buttonS.GetComponent<SortButton>();
        sBar = gm.sAxisBar;
        colliderToggle = gm.colliderToggle;
        sSlider = gm.sSlider;
    }
	
    void SetBar(string key)
    {
        string buttonText = key;
        switch (buttonText)
        {
            case "輝度":
                buttonText = "Luminance";
                break;
            case "彩度":
                buttonText = "Saturation";
                break;
            case "撮影年月":
                buttonText = "Shot Date";
                break;
            default:
                break;
        }
        if (buttonText.Contains("[Y:M]"))
        {
            buttonText = buttonText.Replace("[Y:M]", "");
        }

        if (sBar.activeSelf == false)
        {
            sBar.SetActive(true);
        }
        sBar.transform.Find("Text").GetComponent<Text>().text = buttonText;
        string max, min;
        if (key == "撮影年月" || key.Contains("[Y:M]"))
        {
            max = LengthCheck(PhotoManager.Instance.DeserializeYM(PhotoManager.maxValue[PhotoManager.Instance.keydataCode[key]]));
            min = LengthCheck(PhotoManager.Instance.DeserializeYM(PhotoManager.minValue[PhotoManager.Instance.keydataCode[key]]));
            sBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = max;
            sBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = min;
        }
        else
        {
            max = LengthCheck(PhotoManager.maxValue[PhotoManager.Instance.keydataCode[key]].ToString("R"));
            min = LengthCheck(PhotoManager.minValue[PhotoManager.Instance.keydataCode[key]].ToString("R"));
            sBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = max;
            sBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = min;
        }

    }

    string LengthCheck(string value)
    {
        if (value.Length > 8)
        {
            value = value.Remove(8, value.Length - 8);
        }
        return value;
    }

  

    // Update is called once per frame
    void Update () {
		
	}
}
