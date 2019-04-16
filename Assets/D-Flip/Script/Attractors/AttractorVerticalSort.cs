using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttractorVerticalSort : MonoBehaviour, IAttractorSelection {

    public float weight_;
    int lastKey = -1;
    float min = 0;
    float max = 0;
    public Background bg;
    //Wallのワールド座標
    Vector2 topCenter = Vector2.zero;
    Vector2 bottomCenter = Vector2.zero;
    public GameObject mintext;
    public GameObject maxtext;
    public Slider ySliderMin;
    public Slider ySliderMax;

    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {
        int key = GetComponent<PhotoManager>().vSortKey;
        if (key > 0)
        {
            //参照するパラメータの最大値、最小値を持ってくる
            if (lastKey != key)
            {
                if ((SystemManager.attractorState & SystemManager.AttractorState.GEOGRAPH) == SystemManager.AttractorState.GEOGRAPH)
                {
                    min = 0;
                    max = bg.BackgroundSprite.texture.height; //pixel数指定
                    print("H: "+max);
                }
                else
                {
                    min = PhotoManager.minValue[key];
                    max = PhotoManager.maxValue[key];
                }
                lastKey = key;
            }
            if ((SystemManager.attractorState & SystemManager.AttractorState.GEOGRAPH) != SystemManager.AttractorState.GEOGRAPH)
            {
                if (SystemManager.keys[1].Contains("年月") || SystemManager.keys[1].Contains("Y:M") || SystemManager.keys[1].Contains("date"))
                {
                    min = ySliderMin.value;
                    max = ySliderMax.value;
                }
                else
                {
                    min = float.Parse(mintext.GetComponent<Text>().text);
                    max = float.Parse(maxtext.GetComponent<Text>().text);
                }

            }
            //min = float.Parse(mintext.GetComponent<Text>().text);
            //max = float.Parse(maxtext.GetComponent<Text>().text);
            if (AutoDisplayAdjuster.screenChange == true || AutoDisplayAdjuster.wallChange == true || (topCenter == Vector2.zero && bottomCenter == Vector2.zero))
            {
                topCenter = new Vector2(0, AutoDisplayAdjuster.Instance.TopRight().y);
                bottomCenter = new Vector2(0, AutoDisplayAdjuster.Instance.BottomLeft().y);
            }
            
            foreach (DflipPhoto a in photos)
            {
                //いずれかのベン図の関連写真であればソートの対象外とする
                bool relationCheck = false;
                foreach (Stroke b in strokes)
                {
                    if (b.relatedPhotos.Contains(a))
                    {
                        relationCheck = true;
                    }
                }
                if (relationCheck == true)
                {
                    a.target = a.transform.localPosition;
                    continue;
                }

                if (a.metadata.ContainsKey(key) == true && min != max)
                {
                    //ポジション変更用速度
                    Vector2 v = Vector2.zero;
                    //目標地点
                    Vector2 target = Vector2.zero;

                    target = topCenter - bottomCenter;
                    target = new Vector2(a.transform.localPosition.x, target.y * (a.metadata[key] - min) / (max - min));
                    target = target + bottomCenter;
                    v = (target - (Vector2)a.transform.localPosition) * weight_;
                    a.target.y = target.y;
                    a.AddPosition(v);
                }
                a.AvoidWall();
            }
        }
    }

    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
