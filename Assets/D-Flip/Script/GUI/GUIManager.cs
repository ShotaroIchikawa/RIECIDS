using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class GUIManager : MonoBehaviour {

    public GameObject buttonX;
    public GameObject buttonY;
    public GameObject buttonZ;
    public GameObject buttonS;
    public GameObject xAxisBar;
    public GameObject yAxisBar;
    public GameObject zAxisBar;
    public GameObject sAxisBar;
    public GameObject axisCornerXY;
    public GameObject axisCornerXZ;
    public GameObject background;
    public List<List<GameObject>> buttonList;
    public GameObject mapButton;
    public GameObject options;
    public GameObject colliderToggle;
    public GameObject sSlider;
    public GameObject zSlider;
    public Slider xSliderMin; // test
    public Slider xSliderMax;
    public Slider ySliderMin;
    public Slider ySliderMax; // teest
    public GameObject avoidSlider;
    public GameObject buttonM;
    public GameObject buttonL;
    public static bool startExchange;
    public static bool endExchange;
    public enum BarName
    {
        XAxisBar,
        YAxisBar,
        ZAxisBar,
        SAxisBar
    }
    public static BarName startBar;
    public static BarName endBar;

    public void Initialize()
    {
        buttonList = new List<List<GameObject>>();
        if (buttonX.activeSelf == false)
        {
            buttonX.SetActive(true);
            print("a");
        }
        buttonX.transform.parent.Find("Deselect").gameObject.GetComponent<DeselectButton>().Initiarize();
        buttonY.transform.parent.Find("Deselect").gameObject.GetComponent<DeselectButton>().Initiarize();
        buttonZ.transform.parent.Find("Deselect").gameObject.GetComponent<DeselectButton>().Initiarize();
        buttonS.transform.parent.Find("Deselect").gameObject.GetComponent<DeselectButton>().Initiarize();

        mapButton.GetComponent<Button>().interactable = false;
        background.SetActive(false);
        colliderToggle = options.transform.Find("OptionMenu/ColliderToggle").gameObject;
        sSlider = options.transform.Find("OptionMenu/AdjustScaleSlider").gameObject;
        options.SetActive(false);
        avoidSlider.GetComponent<Slider>().value = 2.5f;

        xAxisBar.SetActive(false);
        yAxisBar.SetActive(false);
        zAxisBar.SetActive(false);
        sAxisBar.SetActive(false);
        axisCornerXY.GetComponent<Image>().enabled = false;
        axisCornerXY.transform.Find("Text").GetComponent<Text>().enabled = false;
        axisCornerXZ.GetComponent<Image>().enabled = false;
        axisCornerXZ.transform.Find("Text").GetComponent<Text>().enabled = false;

        #region debug text
        DebugText.texts.scaleFactor = GameObject.Find("Canvas").gameObject.GetComponent<Canvas>().scaleFactor.ToString();
        DebugText.texts.topOffset = AutoDisplayAdjuster.topOffset.ToString();
        DebugText.texts.bottomOffset = AutoDisplayAdjuster.bottomOffset.ToString();
        DebugText.texts.rightOffset = AutoDisplayAdjuster.rightOffset.ToString();
        DebugText.texts.leftOffset = AutoDisplayAdjuster.leftOffset.ToString();
        DebugText.texts.doubleClick = "OFF";
        DebugText.texts.interactive = "OFF";
        DebugText.texts.follower = AttractorColor.photoNum.ToString();
        #endregion
    }

    
    public void CreateButtons()
    {
        #region Data
        bool dChecker = false;
        //既にボタンがある場合はDestroyして作り直す
        if (buttonList.Count > 0)
        {
            dChecker = true;
            for (int i = buttonList.Count - 1; i > -1; i--)
            {
                for (int j = buttonList[i].Count - 1; j > -1; j--)
                {
                    if (buttonList[i][j].transform.Find("Text").GetComponent<Text>().text != "Deselect")
                    {
                        Destroy(buttonList[i][j]); 
                    }
                }
            }
            buttonList.Clear();
            dChecker = false;
        }

        if (dChecker == false)
        {
            Create();
        }
        #endregion

        #region Keyword
        if (dChecker == false)
        {
            CreateKeyword();
        }
        #endregion

        foreach (List<GameObject> a in buttonList)
        {
            foreach (GameObject b in a)
            {
                b.GetComponent<Button>().interactable = false;
            }
        }
    }

    void Create()
    {
        Dictionary<string, int> keyCodeCopy = new Dictionary<string, int>();
        keyCodeCopy = PhotoManager.Instance.keydataCode;
        List<string> dataName = new List<string>(keyCodeCopy.Keys);

        buttonX.GetComponent<SortButton>().CreateSortMenu(dataName);
        buttonY.GetComponent<SortButton>().CreateSortMenu(dataName);
        buttonZ.GetComponent<SortButton>().CreateSortMenu(dataName);
        buttonS.GetComponent<SortButton>().CreateSortMenu(dataName);

        buttonList.Add(buttonX.GetComponent<SortButton>().buttonList);
        buttonList.Add(buttonY.GetComponent<SortButton>().buttonList);
        buttonList.Add(buttonZ.GetComponent<SortButton>().buttonList);
        buttonList.Add(buttonS.GetComponent<SortButton>().buttonList);
    }

    void CreateKeyword()
    {
        Dictionary<string, int> keywordCodeCopy = new Dictionary<string, int>();
        keywordCodeCopy = PhotoManager.Instance.keywordCode;
        List<string> propertyName = new List<string>();
        propertyName.AddRange(keywordCodeCopy.Keys);
        if (keywordCodeCopy.Count > 1) //キーワードのcsvを読み込んでいれば
        {
            propertyName.RemoveAt(1); //fileNameのボタンは作らない
        }

        buttonM.GetComponent<MouseAttractorButton>().CreateAttractorMenu(propertyName);
        buttonL.GetComponent<MouseAttractorButton2>().CreateAttractorMenu(propertyName);

        buttonList.Add(buttonM.GetComponent<MouseAttractorButton>().buttonList);
        buttonList.Add(buttonL.GetComponent<MouseAttractorButton2>().buttonList);
    }

    public void Deinteractable(string name)
    {
        if (name == "撮影年月")
        {
            name = "Shot Date";
        }
        foreach(List<GameObject> a in buttonList)
        {
            foreach (GameObject b in a)
            {
                if (b.transform.Find("Text").gameObject.GetComponent<Text>().text == name)
                {
                    b.GetComponent<Button>().interactable = false;
                }
            }
        }        
    }

    public void GUIInteractable()
    {
        foreach (List<GameObject> a in buttonList)
        {
            foreach (GameObject b in a)
            {
                b.GetComponent<Button>().interactable = true;
            }
        }
        mapButton.GetComponent<Button>().interactable = true;
    }

    public bool AllReset()
    {
        PhotoManager pm = gameObject.GetComponent<PhotoManager>();
        pm.hSortKey = 0;
        pm.vSortKey = 0;
        pm.dSortKey = 0;
        pm.sSortKey = 0;

        if (xAxisBar.activeSelf)
        {
            buttonX.GetComponent<SortButton>().deselectButton.GetComponent<DeselectButton>().OnButtonClick();
        }
        if (yAxisBar.activeSelf)
        {
            buttonY.GetComponent<SortButton>().deselectButton.GetComponent<DeselectButton>().OnButtonClick();
        }
        if (zAxisBar.activeSelf)
        {
            buttonZ.GetComponent<SortButton>().deselectButton.GetComponent<DeselectButton>().OnButtonClick();
        }
        if (sAxisBar.activeSelf)
        {
            buttonS.GetComponent<SortButton>().deselectButton.GetComponent<DeselectButton>().ResetAttractor(SortButton.axes.S);
            buttonS.GetComponent<SortButton>().deselectButton.GetComponent<DeselectButton>().ResetAxisBar(SortButton.axes.S);
        }

        StrokeManager sm = gameObject.GetComponent<StrokeManager>();
        for (int i = sm.strokes.Count - 1; i > -1; i--)
        {
            Destroy(sm.strokes[i].inputField);
            Destroy(sm.strokes[i].gameObject);
        }
        sm.strokes.Clear();

        if ((SystemManager.pointingState & SystemManager.PointingState.VENN) == SystemManager.PointingState.VENN)
        {
            SystemManager.pointingState = ~SystemManager.PointingState.VENN & SystemManager.pointingState;
            SystemManager.pointingState |= SystemManager.PointingState.POINTING;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        return true;
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
