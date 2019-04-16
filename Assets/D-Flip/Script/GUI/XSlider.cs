using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XSlider : MonoBehaviour {

    Slider slider;
    public GameObject text;
    int lastKey = -1;
    float min = 0;
    float max = 0;
    float currentValue;
    public GameObject col;
    Background BG;

    public void OnValueChanged()
    {
        int key = col.GetComponent<PhotoManager>().hSortKey;
        if (key > 0)
        {
            //参照するパラメータの最大値、最小値を持ってくる
            if (lastKey != key)
            {
                
                if ((SystemManager.attractorState & SystemManager.AttractorState.GEOGRAPH) == SystemManager.AttractorState.GEOGRAPH)
                {
                    min = 0;
                    max = BG.BackgroundSprite.texture.width;//pixel数指定
                    print("V: " + max);
                }
                else
                {
                    min = PhotoManager.minValue[key];
                    max = PhotoManager.maxValue[key];
                }
                lastKey = key;
            }
        }
        if (SystemManager.keys[0].Contains("年月")|| SystemManager.keys[0].Contains("[Y:M]") || SystemManager.keys[0].Contains("date"))
        {
            text.GetComponent<Text>().text = PhotoManager.Instance.DeserializeYM(slider.value);
        }
        else
        {
            //currentValue = slider.value * (max - min) * 0.1f + min;
            currentValue = slider.value;
            text.GetComponent<Text>().text = currentValue.ToString("0.0");
        }
        //currentValue = slider.value * (max - min) * 0.1f + min;
        //text.GetComponent<Text>().text = currentValue.ToString("0.0");
    }

    // Use this for initialization

    void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        BG = GetComponent<Background>();
    }
}
