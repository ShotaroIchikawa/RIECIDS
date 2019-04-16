using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttractorAvoid : MonoBehaviour, IAttractorSelection
{

    public float weight_;
    public float noiseWeight;
    public float threshold;

    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {
        weight_ = weight.AvoidWeight;

        foreach (DflipPhoto a in photos)
        {
            // ポジション変更速度
            Vector2 v = Vector2.zero;


            // 重なっている写真から離れるように移動する
            foreach (PhotoAdjacency b in a.Adjacency)
            {
                if (a.isClicked == true)
                {
                    v += b.direction * 0.1f * weight_;
                }
                else if ((a.target - (Vector2)a.transform.localPosition).magnitude > threshold)
                {
                    v += b.direction * 0.3f * weight_;
                }
                else
                {
                    v += b.direction * 1f * weight_;
                }
            }

            // ノイズ付加
            if (true)
            {
                Vector2 noise = noiseWeight * new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                v += noise;
            }


            a.AddPosition(v);
            a.AvoidWall();
        }
    }

    // Use this for initialization
    void Start()
    {

    }

  

}
