using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeoButton : MonoBehaviour {

    public GameObject backgroundImage;
    public GameObject MenuButton;
    public GameObject xAxisBar;
    public GameObject yAxisBar;
    public GameObject photoSizeSlider;
    public GameObject xMinSlider;
    public GameObject xMaxSlider;
    public GameObject yMinSlider;
    public GameObject yMaxSlider;

    bool activeSelf = false;
    public void OnButtonClick()
    {
        if ((SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
        {
            if (activeSelf == false)
            {
                if (PhotoManager.Instance.keydataCode.ContainsKey("経度") && PhotoManager.Instance.keydataCode.ContainsKey("緯度"))
                {
                    backgroundImage.GetComponent<SpriteRenderer>().sprite = worldMap;
                    backgroundImage.GetComponent<Background>().BackgroundSprite = worldMap;
                    backgroundImage.SetActive(true);
                    backgroundImage.GetComponent<Background>().AdjustScreen();
                    pm.currentMax = pm.Max;
                    pm.Max = photoSize.minValue;
                    photoSize.value = 2; //pm.Max;
                    SetAttractor("経度", "緯度");
                    SetAxisBar("経度", "緯度");
                }
                else if (PhotoManager.Instance.keydataCode.ContainsKey("longitude") && PhotoManager.Instance.keydataCode.ContainsKey("latitude"))
                {
                    backgroundImage.GetComponent<SpriteRenderer>().sprite = worldMap;
                    backgroundImage.GetComponent<Background>().BackgroundSprite = worldMap;
                    backgroundImage.SetActive(true);
                    backgroundImage.GetComponent<Background>().AdjustScreen();
                    pm.currentMax = pm.Max;
                    pm.Max = photoSize.minValue;
                    photoSize.value = 2; //pm.Max;
                    SetAttractor("longitude", "latitude");
                    SetAxisBar("longitude", "latitude");
                }
            }
            else
            {
                backgroundImage.SetActive(false);
                pm.Max = pm.currentMax;
                photoSize.value = pm.Max;
                ResetAttractor();
                ResetAxisBar();

            }
        }
    }

    void SetAxisBar(string xKey, string yKey)
    {
        xMaxSlider.SetActive(false);
        xMinSlider.SetActive(false);
        yMaxSlider.SetActive(false);
        yMinSlider.SetActive(false);
        if (xAxisBar.activeSelf == false)
        {
            xAxisBar.SetActive(true);
        }
        xAxisBar.transform.Find("Text").GetComponent<Text>().text = xKey;
        string xMax = LengthCheck(PhotoManager.maxValue[pm.keydataCode[xKey]].ToString("R"));
        string xMin = LengthCheck(PhotoManager.minValue[pm.keydataCode[xKey]].ToString("R"));
        xAxisBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = xMax;
        xAxisBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = xMin;

        if (yAxisBar.activeSelf == false)
        {
            yAxisBar.SetActive(true);
        }
        yAxisBar.transform.Find("Text").GetComponent<Text>().text = yKey;
        string yMax = LengthCheck(PhotoManager.maxValue[pm.keydataCode[yKey]].ToString("R"));
        string yMin = LengthCheck(PhotoManager.minValue[pm.keydataCode[yKey]].ToString("R"));
        yAxisBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = yMax;
        yAxisBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = yMin;
    }

    void SetAttractor(string xKey, string yKey)
    {
        SystemManager.Instance.AddAttractor(SystemManager.AttractorState.GEOGRAPH);
        pm.hSortKey = pm.keydataCode[xKey];
        SystemManager.Instance.AddAttractor(SystemManager.AttractorState.HORIZONTAL_SORT);

        pm.vSortKey = pm.keydataCode[yKey];
        SystemManager.Instance.AddAttractor(SystemManager.AttractorState.VERTICAL_SORT);

        foreach (DflipPhoto a in pm.photos)
        {
            a.Adjacency.Clear();
        }
    }

    void ResetAttractor()
    {
        SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.GEOGRAPH);
        pm.hSortKey = 0;
        SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.HORIZONTAL_SORT);
        pm.vSortKey = 0;
        SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.VERTICAL_SORT);
    }

    void ResetAxisBar()
    {
        xMaxSlider.SetActive(true);
        xMinSlider.SetActive(true);
        yMaxSlider.SetActive(true);
        yMinSlider.SetActive(true);
        if (xAxisBar.activeSelf == true)
        {
            xAxisBar.SetActive(false);
        }

        if (yAxisBar.activeSelf == true)
        {
            yAxisBar.SetActive(false);
        }
    }

    string LengthCheck(string value)
    {
        if (value.Length > 6)
        {
            value = value.Remove(6, value.Length - 6);
        }
        return value;
    }

    PhotoManager pm;
    Slider photoSize;
    Texture2D texture;
    Sprite worldMap;
    // Use this for initialization
    void Start()
    {
        photoSize = photoSizeSlider.GetComponent<Slider>();
        pm = pm = GameObject.Find("Main Camera").GetComponent<PhotoManager>();
        pm.currentMax = pm.Max;
        texture = Resources.Load("Elements/worldmap1") as Texture2D;
        UnityEngine.Rect rec = new UnityEngine.Rect(0, 0, texture.width, texture.height);
        worldMap = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
        backgroundImage.GetComponent<Background>().BackgroundSprite = worldMap;
    }

    // Update is called once per frame
    void Update()
    {
        if ((SystemManager.attractorState & SystemManager.AttractorState.GEOGRAPH) == SystemManager.AttractorState.GEOGRAPH)
        {
            activeSelf = true;
        }
        else
        {
            if (activeSelf == true)
            {
                activeSelf = false;
                backgroundImage.SetActive(false);
                pm.Max = pm.currentMax;
                photoSize.value = pm.Max;
            }
        }
    }
}
