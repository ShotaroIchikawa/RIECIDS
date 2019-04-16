using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorColor : MonoBehaviour, IAttractorSelection {

    public float weight_;
    public static int photoNum;
    double threshold;
    int key = 0;

    public static double HsvDistance(Vector3 f1, Vector3 f2)
    {
        double x1 = f1.y * System.Math.Cos((double)(f1.x * System.Math.PI) / 180d);
        double y1 = f1.y * System.Math.Sin((double)(f1.x * System.Math.PI) / 180d);
        double z1 = f1.z * 0.7d;

        double x2 = f2.y * System.Math.Cos((double)(f2.x * System.Math.PI) / 180d);
        double y2 = f2.y * System.Math.Sin((double)(f2.x * System.Math.PI) / 180d);
        double z2 = f2.z * 0.7d;

        return System.Math.Sqrt((x1-x2)*(x1-x2) + (y1-y2)*(y1-y2) + (z1-z2)*(z1-z2));
    }

    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {

        key = GetComponent<PhotoManager>().keywordCode["カラー"];

        List<ActivePhoto> active = InputManager.Instance.activePhoto;

        if(active.Count > 0)
        {
            foreach (ActivePhoto a in active)
            {
                List<DflipPhoto> relativePhotos = new List<DflipPhoto>();
                SortedDictionary<double, DflipPhoto> dists_ = new SortedDictionary<double, DflipPhoto>();//小さい順に並べ替えて格納

                #region 閾値設定
                foreach (DflipPhoto b in photos)
                {
                    double dist = 0;
                    if (a.photo != b)
                    {
                        for (int i = 0; i < 25; ++i)
                        {
                            dist += HsvDistance(b.sprite.hsvFeature[i], a.photo.sprite.hsvFeature[i]);
                        }
                        //距離正規化
                        dist /= 25 * 2;
                        dist += (b.sprite.variance - a.photo.sprite.variance) * (b.sprite.variance - a.photo.sprite.variance);
                        dist /= 2;
                    }
                    if (dists_.ContainsKey(dist) == false) //要修正
                    {
                        dists_.Add(dist, b);
                    }
                }
                #endregion

                #region 寄ってくる写真を決定
                int count = 0;
                foreach (KeyValuePair<double, DflipPhoto> kvp in dists_)
                {
                    if (count > photoNum)
                    {
                        count = 0;
                        dists_.Clear();
                        break;
                    }
                    relativePhotos.Add(dists_[kvp.Key]);
                    count++;
                }
                #endregion

                foreach (DflipPhoto c in relativePhotos)
                {
                    Debug.Log(c.fileName);
                    // ポジション変更速度
                    Vector2 v = Vector2.zero;
                    //目標地点
                    c.target = a.photo.gameObject.transform.localPosition;

                    v = (a.photo.gameObject.transform.localPosition - c.gameObject.transform.localPosition) * 0.1f * weight_;

                    c.AddPosition(v);
                    c.AvoidWall();
                }
            }
        }
    }


    // Use this for initialization
    void Start()
    {

    }
}
