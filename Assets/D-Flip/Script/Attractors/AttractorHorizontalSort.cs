using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttractorHorizontalSort : MonoBehaviour, IAttractorSelection {

    Camera camera;
    public float weight_;
    int lastKey = -1;
    float min = 0;
    float max = 0;
    public Background bg;
    Vector2 centerRight = Vector2.zero;
    Vector2 centerLeft = Vector2.zero;
    public GameObject mintext;
    public GameObject maxtext;
    public Slider xSliderMin;
    public Slider xSliderMax;

    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {
        int key = GetComponent<PhotoManager>().hSortKey;
        if (key > 0)
        {
            //参照するパラメータの最大値、最小値を持ってくる
            if (lastKey != key)
            {
                if ((SystemManager.attractorState & SystemManager.AttractorState.GEOGRAPH) == SystemManager.AttractorState.GEOGRAPH)
                {
                    min = 0;
                    max = bg.BackgroundSprite.texture.width;//pixel数指定
                    print("V: " + max);
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
                if (SystemManager.keys[0].Contains("年月") || SystemManager.keys[0].Contains("Y:M") || SystemManager.keys[0].Contains("date"))
                {
                    min = xSliderMin.value;
                    max = xSliderMax.value;
                }
                else
                {
                    min = float.Parse(mintext.GetComponent<Text>().text);
                    max = float.Parse(maxtext.GetComponent<Text>().text);
                }

            }
            //if ((SystemManager.attractorState & SystemManager.AttractorState.GEOGRAPH) != SystemManager.AttractorState.GEOGRAPH)
            //{
            //    min = float.Parse(mintext.GetComponent<Text>().text);
            //    max = float.Parse(maxtext.GetComponent<Text>().text);
            //}
            //min = float.Parse(mintext.GetComponent<Text>().text);
            //max = float.Parse(maxtext.GetComponent<Text>().text);
            if (AutoDisplayAdjuster.screenChange == true || AutoDisplayAdjuster.wallChange == true || (centerRight == Vector2.zero && centerLeft == Vector2.zero))
            {
                centerRight = new Vector2(AutoDisplayAdjuster.Instance.TopRight().x, 0);
                centerLeft = new Vector2(AutoDisplayAdjuster.Instance.BottomLeft().x, 0);
            }

            foreach (DflipPhoto a in photos)
            {
                #region いずれかのベン図の関連写真であればソートの対象外とする
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
                #endregion

                if (a.metadata.ContainsKey(key) == true && min != max)
                {
                    //ポジション変更用速度
                    Vector2 v = Vector2.zero;
                    //目標地点
                    Vector2 target = Vector2.zero;

                    target = centerRight - centerLeft;
                    target = new Vector2(target.x * (a.metadata[key] - min) / (max - min), a.transform.localPosition.y);
                    target = target + centerLeft;
                    v = (target - (Vector2)a.transform.localPosition) * weight_;
                    a.target.x = target.x;
                    a.AddPosition(v);
                }
                a.AvoidWall();
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
