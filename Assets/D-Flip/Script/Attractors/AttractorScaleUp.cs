using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AttractorScaleUp : MonoBehaviour, IAttractorSelection {

    public float weight_ = 1;
    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {
        float MaxPhotoSize = PhotoManager.Instance.Max;
        float MinPhotoSize = PhotoManager.Instance.Min;
        //float eachPhotoSize = Screen.width * Screen.height / photos.Count;

        //weight_ = weight.ScaleUpWeight;

        // アトラクター選択
        foreach (DflipPhoto a in photos)
        {
            //float MaxPhotoSize = PhotoManager.Instance.MinPhotoSize(a, eachPhotoSize);
            //float MinPhotoSize = PhotoManager.Instance.MaxPhotoSize(a, eachPhotoSize);

            // スケール速度
            float ds = 0;

            // 重ならないように制約
            if (a.Adjacency.Count == 0)
            {
                // 周りに画像がなければMaxPhotoSizeまで拡大させる
                ds += (MaxPhotoSize - a.Size) * 0.001f * weight_;
            }

            // サイズがMinPhotoSize以下もしくはMaxPhotoSize以上になるのを防ぐ制約
            if (a.Size < MinPhotoSize  )
            {
                ds += (MinPhotoSize - a.Size) * 0.002f * weight_;
            }
            else if (a.Size > MaxPhotoSize)
            {
                ds -= (a.Size - MaxPhotoSize) * 0.002f * weight_;
            }

            ////ノイズを付加する
            //if (true)
            //{
            //    //float variance = weight.NoiseWeight * 0.2f;

            //    float noise = UnityEngine.Random.value * 0.00001f;
            //    ds += noise;
            //}

            a.AddScale(ds);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
