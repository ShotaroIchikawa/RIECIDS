using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttractorAvoidScale : MonoBehaviour, IAttractorSelection {

    public float weight_;
    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {

        float MaxPhotoSize = PhotoManager.Instance.Max;
        float MinPhotoSize = PhotoManager.Instance.Min;

        // アトラクター選択
        foreach (DflipPhoto a in photos)
        {
            // スケール変更速度
            float ds = 0;

            // 隣接している写真が存在する場合
            if (a.Adjacency.Count > 0)
            {
                //weight_ /= (1f + 0.03f * a.Adjacency.Count);
                foreach (PhotoAdjacency b in a.Adjacency)
                {
                    // 隣接している写真のほうが自身より小さい場合
                    if (b.photo.Size < a.Size)
                    {
                        if (a.isClicked == true)
                        {
                            ds -= (a.Size - MinPhotoSize) * 0.0001f * weight_;
                        }
                        else
                        {
                            ds -= (a.Size - MinPhotoSize) * 0.001f * weight_;
                        }
                    }
                }
            }

            // 写真が最大・最小を超えないようする制約
            if (a.Size < MinPhotoSize)
            {
                ds += (MinPhotoSize - a.Size) * 0.002f * weight_;
            }
            else if (a.Size > MaxPhotoSize)
            {
                ds -= (a.Size - MaxPhotoSize) * 0.002f * weight_;
            }

            //// ノイズ付加
            //if (true)
            //{
            //    //float variance = weight.NoiseWeight * 0.2f;
            //    //float noise = (float)randbm.NextDouble(variance);
            //    //ds += noise;

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
