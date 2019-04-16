using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorKeyword : MonoBehaviour, IAttractorSelection {

    public float weight_;
    int key = 0;
    
    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {
        key = GetComponent<PhotoManager>().mouseAttractorKey;
        List<ActivePhoto> active = InputManager.Instance.activePhoto;

        if (active.Count > 0)
        {
            foreach (ActivePhoto a in active)
            {
                List<DflipPhoto> relativePhotos = new List<DflipPhoto>();

                //foreach (DflipPhoto b in photos)
                //{
                //    //寄ってくる写真を決定
                //    foreach (string c in b.metaword[key])
                //    {
                //        if (c!= "empty")
                //        {
                //            if (a.photo.metaword[key].Contains(c))
                //            {
                //                relativePhotos.Add(b);
                //                b.Adjacency.Clear();
                //            }
                //        }

                //    }

                //}

                string keyword = a.photo.metaword[key][0];
                //Debug.Log(keyword);
                foreach (DflipPhoto b in photos)
                {
                    //寄ってくる写真を決定
                    if (keyword != "empty")
                    {
                        if (b.metaword[key].Contains(keyword))
                        {
                            relativePhotos.Add(b);
                            b.Adjacency.Clear();
                        }
                    }

                }

                foreach (DflipPhoto c in relativePhotos)
                {
                    // ポジション変更速度
                    Vector2 v = Vector2.zero;
                    //目標地点
                    c.target = a.photo.gameObject.transform.localPosition;

                    v = (a.photo.gameObject.transform.localPosition - c.gameObject.transform.localPosition) * 0.1f * weight_;

                    // enlarge relative photos
                    if (c != a.photo && c.Size < a.photo.Size / 5)
                    {
                        float ds = 0;
                        ds += (a.photo.Size / 5 - c.Size) * 0.01f * 40;
                        c.AddScale(ds);
                    }

                    c.AddPosition(v);
                    c.AvoidWall();
                }
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
