using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Stroke : MonoBehaviour {

    public LineRenderer myLineRenderer;
    public LineRenderer myEndLineRenderer;

    public GameObject inputField;

    private List<Vector3> strokes_;
    private Vector3 center_ = Vector3.zero;
    private bool isClosed_ = false;
    private Color color_ = Color.red;
    public string keyword = "";
    private string lastKeyword;
    public int inputID;

    #region property
    public List<Vector3> Strokes
    {
        get
        {
            return strokes_;
        }
    }

    public Vector3 Center
    {
        get
        {
            return center_;
        }
    }

    public bool IsClosed
    {
        get
        {
            return isClosed_;
        }
    }

    public Color Color
    {
        get
        {
            return color_;
        }
        set
        {
            color_ = value;
        }
    }
    public List<DflipPhoto> photos
    {
        get;
        private set;
    }
    public List<DflipPhoto> relatedPhotos
    {
        get;
        private set;
    }
    #endregion

    public void AddStroke(Vector2 ap)
    {
        if(strokes_.Count < 1)
        {
            strokes_.Add(ap);
        }
        else if(strokes_.Count > 0)
        {
            if(((Vector2)strokes_[strokes_.Count - 1] - ap).magnitude > 0.1)
            {
                strokes_.Add(ap);
            }
        }
    }

    private void InterporateStroke()
    {
        Vector3 space = strokes_[0] - strokes_[strokes_.Count - 1];
        Vector3 lastStroke = strokes_[strokes_.Count - 1];
        float spaceLength = space.magnitude;
        if (spaceLength > 1)
        {
            Vector3 normalizedSpace = space / spaceLength;
            for (float i = 0; i < spaceLength; i += 0.5f)
            {
                strokes_.Add(lastStroke + normalizedSpace * i);

            }
        }

    }

    public void CalcCenter(List<Vector3> strokes)
    {
        foreach (Vector3 s in strokes)
        {
            center_ += s;
        }
        center_ /= strokes.Count;
    }

    public void RenderEnd()
    {
        InterporateStroke();
        CalcCenter(strokes_);

        strokes_.Add(strokes_[0]);
        RenderVenn();
        
        isClosed_ = true;
    }

    public void RenderEndLine()
    {
        SetRenderParams(myEndLineRenderer);
        myEndLineRenderer.positionCount = 2;
        myEndLineRenderer.SetPosition(0, strokes_[strokes_.Count - 1]);
        myEndLineRenderer.SetPosition(1, strokes_[0]);
    }

    public void RenderVenn()
    {
        SetRenderParams(myLineRenderer);
        myLineRenderer.positionCount = strokes_.Count;
        myLineRenderer.SetPositions(strokes_.ToArray());
    }

    private void SetRenderParams(LineRenderer line)
    {
        line.startWidth = 0.07f;
        line.endWidth = 0.07f;       
    }

    public bool IsInternalCheck(Vector2 targetPosition, Vector2 rightTop) //rightTopは画面右端の座標
    {
        //交差数による包含判定の基準点をスクリーンの外に設定
        Vector2[] root = new Vector2[4];
        //root[0]:画面外左上, [1]:左下, [2]:右下, [3]:右上
        root[0] = 2 * new Vector2(-rightTop.x, +rightTop.y);
        root[1] = 2 * new Vector2(-rightTop.x, -rightTop.y);
        root[2] = 2 * new Vector2(+rightTop.x, -rightTop.y);
        root[3] = 2 * new Vector2(+rightTop.x, +rightTop.y);

        //包含判定
        int trueCount = 0;
        foreach (Vector2 r in root)
        {
            int crossCount = 0;
            for (int i = 0; i + 1 < strokes_.Count - 5; i+=5) //５点ごとに判定
            {
                Vector2 now = strokes_[i];
                Vector2 next = strokes_[i + 5];
                Vector2 a = targetPosition - r;
                Vector2 b = now - r;
                int s1 = (int)Mathf.Sign(a.x * b.y - a.y * b.x);
                b = next - r;
                int s2 = (int)Mathf.Sign(a.x * b.y - a.y * b.x);
                if (s1 != s2)
                {
                    a = next - now;
                    b = r - now;
                    s1 = (int)Mathf.Sign(a.x * b.y - a.y * b.x);
                    b = targetPosition - now;
                    s2 = (int)Mathf.Sign(a.x * b.y - a.y * b.x);
                    if (s1 != s2)
                    {
                        ++crossCount;
                    }
                }
            }
            if(crossCount % 2 == 1)
            {
                ++trueCount;
            }
        }
        if(trueCount > (root.Length + 1) / 2 - 1) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Camera camera;
    void Awake()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        strokes_ = new List<Vector3>();
        photos = new List<DflipPhoto>();
        relatedPhotos = new List<DflipPhoto>();
    }

    private List<DflipPhoto> unrelatedPhotos = new List<DflipPhoto>();
    // Update is called once per frame
    void Update()
    {
        if(keyword != lastKeyword)
        {
            if(relatedPhotos.Count > 0)
            {
                foreach (DflipPhoto a in relatedPhotos)
                {
                    foreach(List<string> b in a.metaword.Values)
                    {
                        if (b.Contains(keyword) == false)
                        {
                            unrelatedPhotos.Add(a);
                        }
                    }
                }
            }    
            if(unrelatedPhotos.Count > 0)
            {
                foreach (DflipPhoto a in unrelatedPhotos)
                {
                    relatedPhotos.Remove(a);
                }
            }
            unrelatedPhotos.Clear();
            foreach (DflipPhoto a in photos)
            {
                foreach (List<string> b in a.metaword.Values)
                {
                    if (b.Contains(keyword) == true)
                    {
                        relatedPhotos.Add(a);
                    }
                }
            }
            lastKeyword = keyword;
        }

    }
}
