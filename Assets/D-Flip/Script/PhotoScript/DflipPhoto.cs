using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DflipPhoto : MonoBehaviour
{
    public PhotoSprite sprite;
    public bool isClicked;
    public bool showWall = false;  // wall label
    public float Size;
    public Vector2 target;
    public string fileName;
    public int ID;

    //タグ用変数
    public Dictionary<int, List<string>> metaword;
    public Dictionary<int, float> metadata;

    #region property
    public List<PhotoAdjacency> Adjacency
    {
        get
        {
            return _Adjacency;
        }

    }

    public List<AreaAdjustor> AreaAdjustor
    {
        get
        {
            return _areaAdjustor;
        }
    }

    public GameObject gameobject
    {
        get
        {
            return gameObject;
        }
    }

    #endregion

    private Rigidbody rigid;
    public BoxCollider collider;

    void Awake()
    {
        metaword = new Dictionary<int, List<string>>();
        metadata = new Dictionary<int, float>();
        _Adjacency = new List<PhotoAdjacency>();
        _areaAdjustor = new List<AreaAdjustor>();
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        
    }

    void OnTriggerStay(Collider col)
    {
        var _photo = col.gameObject.GetComponent<DflipPhoto>();
        if (_photo != null && _Adjacency.Find(c => c.photo == _photo) == null)
        {
            Vector2 v = Vector2.zero;
            switch (col.gameObject.name)
            {
                case "Right":
                    v = -Vector2.right * 1.2f;
                    break;
                case "Left":
                    v = Vector2.right * 1.2f;
                    break;
                case "Top":
                    v = -Vector2.up * 1.2f;
                    break;
                case "Bottom":
                    v = Vector2.up * 1.2f;
                    break;
                default:
                    v = (transform.localPosition - col.gameObject.transform.localPosition);
                    break;
            }
            _Adjacency.Add(new PhotoAdjacency(_photo, v, 0));
        }
    }

    void OnTriggerExit(Collider col)
    {
        var _photo = col.gameObject.GetComponent<DflipPhoto>();
        if (_photo != null)
        {
            var a = _Adjacency.Find(c => c.photo == _photo);
            if (a != null)
            {
                _Adjacency.Remove(a);
            }
        }
    }


    public void AddScale(float scale)
    {
        if(Time.timeScale != 0)
        {
            gameObject.transform.localScale += new Vector3(scale * 1 / PhotoManager.Instance.ScaleWeight, scale * 1 / PhotoManager.Instance.ScaleWeight, 0);
            //tempScale = new Vector3(scale / PhotoManager.Instance.ScaleWeight, scale / PhotoManager.Instance.ScaleWeight,0);
        }
    }

    public void AddPosition(Vector2 vec)
    {
        if(Time.timeScale != 0)
        {
            rigid.AddForce(vec, ForceMode.Force);
        }
    }

    public void AvoidWall()
    {
        float x = transform.localPosition.x;
        float y = transform.localPosition.y;

        x = x < AutoDisplayAdjuster.Instance.BottomLeft().x ? AutoDisplayAdjuster.Instance.BottomLeft().x : x;
        x = x > AutoDisplayAdjuster.Instance.TopRight().x ? AutoDisplayAdjuster.Instance.TopRight().x : x;
        y = y < AutoDisplayAdjuster.Instance.BottomLeft().y ? AutoDisplayAdjuster.Instance.BottomLeft().y : y;
        y = y > AutoDisplayAdjuster.Instance.TopRight().y ? AutoDisplayAdjuster.Instance.TopRight().y : y;

        transform.localPosition = new Vector3(x, y, transform.localPosition.z);
    }


    public void UpdatePhoto()
    {
        Size = gameobject.transform.localScale.x * sprite.image.width * gameobject.transform.localScale.y * sprite.image.height * 0.001f;



        #region targetPositionの更新
        if ((SystemManager.attractorState & SystemManager.AttractorState.HORIZONTAL_SORT) != SystemManager.AttractorState.HORIZONTAL_SORT)
        {
            target.x = gameObject.transform.localPosition.x;
        }
        if((SystemManager.attractorState & SystemManager.AttractorState.VERTICAL_SORT) != SystemManager.AttractorState.VERTICAL_SORT)
        {
            target.y = gameObject.transform.localPosition.y;
        }
        #endregion

        sprite.UpdateSprite();
        if (isClicked == true && showWall == true)
        {
            sprite.myLabel.UpdateWallLable();
        }

    }

    public List<PhotoAdjacency> _Adjacency;
    public List<AreaAdjustor> _areaAdjustor;
}
