using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorAdjustScale : MonoBehaviour, IAttractorSelection {

    public float weight_;
    int lastKey;
    float min, max;
    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {
        int key = GetComponent<PhotoManager>().sSortKey;
        if (key > 0)
        {
            //参照するパラメータの最大値、最小値を持ってくる
            if (lastKey != key)
            {
                min = PhotoManager.minValue[key];
                max = PhotoManager.maxValue[key];
                lastKey = key;
            }

            foreach (DflipPhoto a in photos)
            {
                //スケール変更用速度
                float ds = 0;
                if (a.metadata.ContainsKey(key) == true && min != max)
                {
                    //目標スケール
                    float target = PhotoManager.Instance.AdjustScaleMax;              
                    target = target * Mathf.Pow((a.metadata[key] - min) / (max - min), 1) + 0.5f;
                    ds = (target - a.Size)* 0.005f * weight_;
                }
                a.AddScale(ds);
            }
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
