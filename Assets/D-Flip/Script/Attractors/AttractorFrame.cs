using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorFrame : MonoBehaviour, IAttractorSelection {

    public float gatherWeight_;
    public float avoidWeight_;
    Camera camera;
    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {
        //weight_ = weight.FrameWeight;
        foreach (Stroke a in strokes)
        {
            if(a.IsClosed == false)
            {
                continue;
            }

            foreach(DflipPhoto b in photos)
            {
                if (b.isClicked == true)
                {
                    continue;
                }

                bool inner = a.IsInternalCheck(b.transform.localPosition, camera.ScreenToWorldPoint(new Vector2( Screen.width, Screen.height)));
                Vector2 v = Vector2.zero;
                Vector2 v2n = Vector2.one * float.MaxValue;

                if (a.relatedPhotos.Contains(b) == true && inner == false) //関連写真がベン図外にあれば引き寄せる
                {
                    Vector2 dir = a.Center - b.transform.position;
                    v += dir / dir.magnitude;
                    v *= 0.1f * gatherWeight_;
                    b.Adjacency.Clear();
                }
                else if (a.relatedPhotos.Contains(b) == false && inner == true) //非関連写真がベン図内にあれば弾き出す
                {
                    Vector2 dir = b.transform.position - a.Center;
                    v += dir / dir.magnitude;
                    v *= 1f * avoidWeight_;
                }

                //ノイズ付加
                //if (true)
                //{
                //    //float variance = weight.NoiseWeight * 0.5f;
                //    //Vector2 noise = new Vector2((float)randbm.NextDouble(variance), (float)randbm.NextDouble(variance));

                //    Vector2 noise = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                //    v += noise;
                //}

                b.AddPosition(v);
            }

        }

    }





	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
