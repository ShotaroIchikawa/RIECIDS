using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SortMenuButton : MonoBehaviour {

    public void MyOnClick(string key, SortButton.axes axis)
    {
        switch (axis)
        {
            case SortButton.axes.X:
                SystemManager.keys[0] = key;
                break;
            case SortButton.axes.Y:
                SystemManager.keys[1] = key;
                break;
            case SortButton.axes.Z:
                SystemManager.keys[2] = key;
                break;
            case SortButton.axes.S:
                SystemManager.keys[3] = key;
                break;
        }
        SetAxisBar(axis, key);
        SetAttractor(axis, key);
    }

    void SetAttractor(SortButton.axes axis, string key)
    {
        if (axis == SortButton.axes.X)
        {
            pm.hSortKey = pm.keydataCode[key];
            SystemManager.Instance.AddAttractor(SystemManager.AttractorState.HORIZONTAL_SORT);
            if (key != "経度" && key != "longitude")
            {
                SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.GEOGRAPH);

            }
            buttonX.Close();
        }
        else if(axis == SortButton.axes.Y)
        {
            pm.vSortKey = pm.keydataCode[key];
            SystemManager.Instance.AddAttractor(SystemManager.AttractorState.VERTICAL_SORT);
            if (key != "緯度" && key != "latitude")
            {
                SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.GEOGRAPH);

            }
            buttonY.Close();
        }
        else
        {
            pm.dSortKey = pm.keydataCode[key];
            SystemManager.Instance.AddAttractor(SystemManager.AttractorState.DEPTH_SORT);
            buttonZ.Close();
        }

        foreach (DflipPhoto a in pm.photos)
        {
            a.Adjacency.Clear();
        }
    }

    void SetAxisBar(SortButton.axes axis, string key)
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

        if (axis == SortButton.axes.X)
        {
            if (xAxisBar.activeSelf == false)
            {
                xAxisBar.SetActive(true);
            }
            if (key =="撮影年月" || key.Contains("[Y:M]"))
            {
                string max = LengthCheck(PhotoManager.Instance.DeserializeYM(PhotoManager.maxValue[pm.keydataCode[key]]));
                string min = LengthCheck(PhotoManager.Instance.DeserializeYM(PhotoManager.minValue[pm.keydataCode[key]]));
                xAxisBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = max;
                xAxisBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = min;
                xSliderMin.maxValue = PhotoManager.maxValue[pm.keydataCode[key]];
                xSliderMin.minValue = PhotoManager.minValue[pm.keydataCode[key]];
                xSliderMin.value = xSliderMin.minValue;
                xSliderMax.maxValue = PhotoManager.maxValue[pm.keydataCode[key]];
                xSliderMax.minValue = PhotoManager.minValue[pm.keydataCode[key]];
                xSliderMax.value = xSliderMax.maxValue;
            }
            else
            {
                string max = LengthCheck(PhotoManager.maxValue[pm.keydataCode[key]].ToString("R"));
                string min = LengthCheck(PhotoManager.minValue[pm.keydataCode[key]].ToString("R"));
                xAxisBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = max;
                xAxisBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = min;
                xSliderMin.maxValue = float.Parse(max);
                xSliderMin.minValue = float.Parse(min);
                xSliderMin.value = xSliderMin.minValue;
                xSliderMax.maxValue = float.Parse(max);
                xSliderMax.minValue = float.Parse(min);
                xSliderMax.value = xSliderMax.maxValue;
            }
            xAxisBar.transform.Find("Text").GetComponent<Text>().text = buttonText;

        }
        else if(axis == SortButton.axes.Y)
        {
            if (yAxisBar.activeSelf == false)
            {
                yAxisBar.SetActive(true);
            }
            yAxisBar.transform.Find("Text").GetComponent<Text>().text = buttonText;
            if (key == "撮影年月" || key.Contains("[Y:M]"))
            {
                string max = LengthCheck(PhotoManager.Instance.DeserializeYM(PhotoManager.maxValue[pm.keydataCode[key]]));
                string min = LengthCheck(PhotoManager.Instance.DeserializeYM(PhotoManager.minValue[pm.keydataCode[key]]));
                yAxisBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = max;
                yAxisBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = min;
                ySliderMin.maxValue = PhotoManager.maxValue[pm.keydataCode[key]];
                ySliderMin.minValue = PhotoManager.minValue[pm.keydataCode[key]];
                ySliderMin.value = ySliderMin.minValue;
                ySliderMax.maxValue = PhotoManager.maxValue[pm.keydataCode[key]];
                ySliderMax.minValue = PhotoManager.minValue[pm.keydataCode[key]];
                ySliderMax.value = ySliderMax.maxValue;
            }
            else
            {
                string max = LengthCheck(PhotoManager.maxValue[pm.keydataCode[key]].ToString("R"));
                string min = LengthCheck(PhotoManager.minValue[pm.keydataCode[key]].ToString("R"));
                yAxisBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = max;
                yAxisBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = min;
                ySliderMin.maxValue = float.Parse(max);
                ySliderMin.minValue = float.Parse(min);
                ySliderMin.value = ySliderMin.minValue;
                ySliderMax.maxValue = float.Parse(max);
                ySliderMax.minValue = float.Parse(min);
                ySliderMax.value = ySliderMax.maxValue;
            }
        }
        else
        {
            if (zAxisBar.activeSelf == false)
            {
                zAxisBar.SetActive(true);
            }
            zAxisBar.transform.Find("Text").GetComponent<Text>().text = buttonText;
            //string max, min;
            if (key == "撮影年月" || key.Contains("[Y:M]"))
            {
                string max = LengthCheck(PhotoManager.Instance.DeserializeYM(PhotoManager.maxValue[pm.keydataCode[key]]));
                string min = LengthCheck(PhotoManager.Instance.DeserializeYM(PhotoManager.minValue[pm.keydataCode[key]]));
                zAxisBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = max;
                zAxisBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = min;
                zSlider.maxValue = PhotoManager.maxValue[pm.keydataCode[key]];
                zSlider.minValue = PhotoManager.minValue[pm.keydataCode[key]];
                zSlider.value = zSlider.minValue;
            }
            else
            {
                string max = LengthCheck(PhotoManager.maxValue[pm.keydataCode[key]].ToString("R"));
                string min = LengthCheck(PhotoManager.minValue[pm.keydataCode[key]].ToString("R"));
                zAxisBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = max;
                zAxisBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = min;
                zSlider.maxValue = float.Parse(max);
                zSlider.minValue = float.Parse(min);
                zSlider.value = zSlider.minValue;
            }
            
            zSlider.gameObject.GetComponent<ZAxisSlider>().OnValueChanging();
        }
    }

    string LengthCheck(string value)
    {
        if(value.Length > 16)
        {
            value = value.Remove(16, value.Length - 16);
        }
        return value;
    }

    PhotoManager pm;
    SortButton buttonX;
    SortButton buttonY;
    SortButton buttonZ;
    GameObject xAxisBar;
    GameObject yAxisBar;
    GameObject zAxisBar;
    Slider zSlider;
    Slider xSliderMin; 
    Slider xSliderMax;
    Slider ySliderMin;
    Slider ySliderMax;
    public void Initialize()
    {
        //GUIManagerから参照をもらう
        GameObject camera = GameObject.Find("Main Camera").gameObject;
        pm = camera.GetComponent<PhotoManager>();
        GUIManager gm = camera.GetComponent<GUIManager>();
        xAxisBar = gm.xAxisBar;
        yAxisBar = gm.yAxisBar;
        zAxisBar = gm.zAxisBar;
        buttonX = gm.buttonX.GetComponent<SortButton>();
        buttonY = gm.buttonY.GetComponent<SortButton>();
        buttonZ = gm.buttonZ.GetComponent<SortButton>();
        if (zAxisBar != null)
        {
            zSlider = gm.zSlider.GetComponent<Slider>();
        }
        if (xAxisBar != null) // Sort Slider
        {
            xSliderMin = gm.xSliderMin.GetComponent<Slider>();
            xSliderMax = gm.xSliderMax.GetComponent<Slider>();
        }
        if (yAxisBar != null)
        {
            ySliderMin = gm.ySliderMin.GetComponent<Slider>();
            ySliderMax = gm.ySliderMax.GetComponent<Slider>();
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
