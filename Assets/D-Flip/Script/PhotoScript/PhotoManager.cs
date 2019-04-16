using UnityEngine;
using System.Collections.Generic;
using System;

public class PhotoManager : MonoBehaviour {

    public static Vector3 offset = new Vector3(0, 0, -20);
    public static bool LOADED = false;
    public bool ShowWallLabel = false;
    public static PhotoManager Instance;
    public float Max, Min;
    public float currentMax; //Geograph解除時に元のMaxに戻すための変数
    public float AdjustScaleMax, AdjustScaleMin;
    public float ScaleWeight;
    bool attractorEnable;
    public Dictionary<string, int> keydataCode = new Dictionary<string, int>();
    public Dictionary<string, int> keywordCode = new Dictionary<string, int>();
    public enum filter {
        DEFAULT,
        SMA,
        LWMA,
        GAUSS
    };
    public filter filterSelector;

    #region property
    public List<DflipPhoto> photos
    {
        get;
        private set;
    }
    public int hSortKey
    {
        get;
        set;
    }
    public int vSortKey
    {
        get;
        set;
    }

    public int dSortKey
    {
        get;
        set;
    }
    public int sSortKey
    {
        get;
        set;
    }
    public int mouseAttractorKey
    {
        get;
        set;
    }
    public int mouseAttractorKey2
    {
        get;
        set;
    }
    #endregion

    void Awake()
    {
        Instance = this;
        LOADED = false;
        filterSelector = filter.SMA;
        Initialize();
    }

    void Initialize()
    {
        photos = new List<DflipPhoto>();
        photos.Clear();
        attractorEnable = false;
    }

    public void AddPhoto( DflipPhoto p )
    {
        photos.Add(p);
    }

    public void AttractorEnable()
    {
        attractorEnable = true;
    }

    public void AttractorDisable()
    {
        attractorEnable = false;
    }

    #region シリアル化
    public float SerializeYM(string data)
    {
        string[] strs = data.Split(':');
        float shotTime = (DateTime.Parse(strs[0] + "/" + strs[1])).ToBinary() / 10000000 / 60 / 60 / 24; //日単位でシリアル値化
        return shotTime;
    }

    public string DeserializeYM(float data)
    {
        DateTime change = DateTime.FromBinary((long)(data * 10000000 * 60 * 60 * 24));
        return change.Year.ToString() + "/" + change.Month.ToString();
    }
    #endregion

    private int dataIndex;
    public static Dictionary<int, float> maxValue = new Dictionary<int, float>();
    public static Dictionary<int, float> minValue = new Dictionary<int, float>();
    public static Dictionary<int, float> midValue = new Dictionary<int, float>();
    public void CreateKeydataCode(List<List<string>> data)
    {
        //ビットフラグとカラムをDictionaryで対応付け
        for (int i = 1; i < data[0].Count; ++i)
        {
            if (keydataCode.ContainsKey(data[0][i]) == false) //同名の属性が存在しなければ新たに作る　※同名の属性が存在するならば値は更新される
            {
                keydataCode.Add(data[0][i], (int)Mathf.Pow(2, dataIndex));
                dataIndex++;
            }
        }
    }

    public void CreateKeydataCode(string name)
    {
        if (keydataCode.ContainsKey(name) == false)
        {
            keydataCode.Add(name, (int)Mathf.Pow(2, dataIndex));
            ++dataIndex;
        }
    }

    private int wordIndex;
    public void CreateKeywordCode(List<List<string>> words)
    {
        //ビットフラグとカラムをDictionaryで対応付け
        for (int i = 0; i < words[0].Count; ++i) //ファイル名もタグとして登録するため i = 0 からスタート
        {
            if (keywordCode.ContainsKey(words[0][i]) == false) //同名の属性が存在しなければ新たに作る
            {
                keywordCode.Add(words[0][i], (int)Mathf.Pow(2, wordIndex));
                wordIndex++;
            }
        }
        
    }

    public void CreateKeywordCode(string name)
    {
        if (keywordCode.ContainsKey(name) == false)
        {
            keywordCode.Add(name, (int)Mathf.Pow(2, wordIndex));
            ++wordIndex;
        }
    }


    public void CreateMaxMinCode(List<List<string>> data)
    {
        for (int i = 1; i < data[0].Count; ++i)
        {
            float tempMax;
            float tempMin;
            if (data[0][i].Contains("[Y:M]"))
            {
                float shotTime = SerializeYM(data[1][i]);
                tempMax = shotTime;
                tempMin = shotTime;
            }
            else
            {
                tempMax = float.Parse(data[1][i]);
                tempMin = float.Parse(data[1][i]);
            }
            
            float tempMid = 0;
            for (int j = 1; j < data.Count; j++)
            {
                float temp;
                if (data[0][i].Contains("[Y:M]"))
                {
                    temp = SerializeYM(data[j][i]);
                }
                else
                {
                    temp = float.Parse(data[j][i]);
                }

                tempMid += temp;
                if (tempMax < temp)
                {
                    tempMax = temp;
                }
                if (tempMin > temp)
                {
                    tempMin = temp;
                }
            }
            if (maxValue.ContainsKey(keydataCode[data[0][i]]) == false)
            { 
                //値の登録
                maxValue.Add(keydataCode[data[0][i]], tempMax);
                minValue.Add(keydataCode[data[0][i]], tempMin);
                midValue.Add(keydataCode[data[0][i]], tempMid / (data.Count - 1));
            }
            else
            {
                //値の更新
                maxValue[keydataCode[data[0][i]]] = tempMax;
                minValue[keydataCode[data[0][i]]] = tempMin;
                midValue[keydataCode[data[0][i]]] = tempMid / (data.Count - 1);
            }
        }
        
        hSortKey = keydataCode[data[0][1]];
        vSortKey = keydataCode[data[0][1]];
        dSortKey = keydataCode[data[0][1]];
    }

    public void CreateMaxMinCode(string name)
    {
        int key = keydataCode[name];
        if (maxValue.ContainsKey(key) == false)
        {
            float tempMax = photos[0].metadata[key];
            float tempMin = photos[0].metadata[key];
            float tempMid = 0;
            foreach (DflipPhoto a in photos)
            {
                float temp = a.metadata[key];
                tempMid += temp;
                if (tempMax < temp)
                {
                    tempMax = temp;
                }
                if (tempMin > temp)
                {
                    tempMin = temp;
                }
            }
            if (maxValue.ContainsKey(keydataCode[name]) == false)
            {
                //値の登録
                maxValue.Add(keydataCode[name], tempMax);
                minValue.Add(keydataCode[name], tempMin);
                midValue.Add(keydataCode[name], tempMid / photos.Count);
            }
            else
            {
                //値の更新
                maxValue[keydataCode[name]] = tempMax;
                minValue[keydataCode[name]] = tempMin;
                midValue[keydataCode[name]] = tempMid / photos.Count;
            }
        }
        else
        {
            //値の更新
        }
    }

    public void DeleteKeydataCode(string key)
    {
        keydataCode.Remove(key);
    }

    public void ResetPhotosColor()
    {
        foreach (DflipPhoto a in photos)
        {
            a.sprite.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        }
    }

    AttractorScaleUp scaleUp;
    AttractorAvoidScale avoidScale;
    AttractorAvoid avoid;
    AttractorHorizontalSort hSort;
    AttractorVerticalSort vSort;
    AttractorScaleUpMouse scaleUpMouse;
    AttractorFrame frame;
    public AttractorColor color;
    AttractorKeyword keyword;
    AttractorLanguage language;
    AttractorAdjustScale adjustScale;
    public AttractorWeight weight;
    StrokeManager stroke;
    CreateWallLabel CWL;
    // Use this for initialization
    void Start () {
        dataIndex = 0;
        wordIndex = 0;

        hSortKey = 0;
        vSortKey = 0;
        dSortKey = 0;
        mouseAttractorKey = 0;
        mouseAttractorKey2 = 0;

        scaleUp = GetComponent<AttractorScaleUp>();
        avoidScale = GetComponent<AttractorAvoidScale>();
        avoid = GetComponent<AttractorAvoid>();
        hSort = GetComponent<AttractorHorizontalSort>();
        vSort = GetComponent<AttractorVerticalSort>();
        scaleUpMouse = GetComponent<AttractorScaleUpMouse>();
        frame = GetComponent<AttractorFrame>();
        color = GetComponent <AttractorColor>();
        keyword = GetComponent<AttractorKeyword>();
        language = GetComponent<AttractorLanguage>();
        adjustScale = GetComponent<AttractorAdjustScale>();
        stroke = GetComponent<StrokeManager>();
        CWL = GetComponent<CreateWallLabel>();
	}

    void Update()
    {
        weight = GetComponent<SystemManager>().weight;

        if ((SystemManager.systemState & SystemManager.SystemState.MAIN) == SystemManager.SystemState.MAIN)
        {
            if (attractorEnable)
            {
                #region selectの更新
                if ((SystemManager.attractorState & SystemManager.AttractorState.AVOID) == SystemManager.AttractorState.AVOID)
                {
                    avoid.select(weight, photos, stroke.strokes);
                }
                if ((SystemManager.attractorState & SystemManager.AttractorState.AVOID_SCALE) == SystemManager.AttractorState.AVOID_SCALE)
                {
                    avoidScale.select(weight, photos, stroke.strokes);
                }
                if ((SystemManager.attractorState & SystemManager.AttractorState.SCALE_UP) == SystemManager.AttractorState.SCALE_UP)
                {
                    scaleUp.select(weight, photos, stroke.strokes);
                }
                if ((SystemManager.attractorState & SystemManager.AttractorState.HORIZONTAL_SORT) == SystemManager.AttractorState.HORIZONTAL_SORT)
                {
                    hSort.select(weight, photos, stroke.strokes);
                }
                if ((SystemManager.attractorState & SystemManager.AttractorState.VERTICAL_SORT) == SystemManager.AttractorState.VERTICAL_SORT)
                {
                    vSort.select(weight, photos, stroke.strokes);
                }
                if ((SystemManager.attractorState & SystemManager.AttractorState.FRAME) == SystemManager.AttractorState.FRAME)
                {
                    frame.select(weight, photos, stroke.strokes);
                }
                if ((SystemManager.attractorState & SystemManager.AttractorState.ADJUST_SCALE) == SystemManager.AttractorState.ADJUST_SCALE)
                {
                    adjustScale.select(weight, photos, stroke.strokes);
                }
                if ((SystemManager.attractorState & SystemManager.AttractorState.COLOR) == SystemManager.AttractorState.COLOR)
                {
                    color.select(weight, photos, stroke.strokes);
                }
                if ((SystemManager.attractorState & SystemManager.AttractorState.KEYWORD) == SystemManager.AttractorState.KEYWORD)
                {
                    keyword.select(weight, photos, stroke.strokes);
                }
                if ((SystemManager.attractorState & SystemManager.AttractorState.LANGUAGE) == SystemManager.AttractorState.LANGUAGE)
                {
                    language.select(weight, photos, stroke.strokes);
                }


                if ((SystemManager.pointingState & SystemManager.PointingState.POINTING) == SystemManager.PointingState.POINTING)
                {
                    if ((SystemManager.attractorState & SystemManager.AttractorState.SCALE_UP_MOUSE) == SystemManager.AttractorState.SCALE_UP_MOUSE)
                    {
                        scaleUpMouse.select(weight, photos, stroke.strokes);                       
                    }
                    if (ShowWallLabel)
                    {
                        CWL.Create(photos);
                    }
                }
                #endregion
            }

            foreach (var a in photos)
            {
                a.UpdatePhoto();
            }
        }
    }
}
