using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AxisCorner : MonoBehaviour, IPointerClickHandler {
    //axisに合わせて自動でenabledを変化させる
    private Image myself;
    private Text myText;
    public GameObject firstAxisBar;
    public GameObject secondAxisBar;
    public GameObject colliderToggle;

    public GameObject background;
    public Slider photoSizeSlider;

    public enum Corner
    {
        rightTop,
        leftTop,
        rightBottom,
        leftBottom
    }

    public Corner cornerPos;
	// Use this for initialization
	void Start () {
        myself = GetComponent<Image>();
        myText = gameObject.transform.Find("Text").GetComponent<Text>();
	}
    
	// Update is called once per frame
	void Update () {
        if (myself.enabled == true)
        {
            if (firstAxisBar.activeSelf == false && secondAxisBar.activeSelf == false)
            {
                myself.enabled = false;
                myText.enabled = false;
            }
        }
        else
        {
            if (firstAxisBar.activeSelf == true || secondAxisBar.activeSelf == true)
            {
                myself.enabled = true;
                myText.enabled = true;
            }
        }
		
	}

    public void OnPointerClick(PointerEventData ed)
    {
        if (ed.button == 0)
        {
            firstAxisBar.SetActive(false);
            secondAxisBar.SetActive(false);

            switch (cornerPos)
            {
                case Corner.rightBottom:
                    PhotoManager.Instance.hSortKey = 0;
                    SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.HORIZONTAL_SORT);
                    PhotoManager.Instance.dSortKey = 0;
                    SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.DEPTH_SORT);
                    PhotoManager.Instance.ResetPhotosColor();
                    DeactiveBG();
                    break;
                case Corner.leftBottom:
                    PhotoManager.Instance.hSortKey = 0;
                    SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.HORIZONTAL_SORT);
                    PhotoManager.Instance.vSortKey = 0;
                    SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.VERTICAL_SORT);
                    DeactiveBG();
                    break;
                case Corner.rightTop:
                    PhotoManager.Instance.dSortKey = 0;
                    SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.DEPTH_SORT);
                    PhotoManager.Instance.ResetPhotosColor();
                    PhotoManager.Instance.sSortKey = 0;
                    SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.ADJUST_SCALE);
                    StartCoroutine(ColliderIsTrigger(true));
                    colliderToggle.GetComponent<Toggle>().isOn = true;
                    colliderToggle.GetComponent<Toggle>().interactable = false;
                    break;
                case Corner.leftTop:
                    PhotoManager.Instance.vSortKey = 0;
                    SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.VERTICAL_SORT);
                    PhotoManager.Instance.sSortKey = 0;
                    SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.ADJUST_SCALE);            
                    StartCoroutine(ColliderIsTrigger(true));
                    colliderToggle.GetComponent<Toggle>().isOn = true;
                    colliderToggle.GetComponent<Toggle>().interactable = false;
                    DeactiveBG();
                    break;
                default:
                    break;

            }

        }
    }

    void DeactiveBG()
    {
        if ((SystemManager.attractorState & SystemManager.AttractorState.GEOGRAPH) == SystemManager.AttractorState.GEOGRAPH)
        {
            SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.GEOGRAPH);
            PhotoManager.Instance.Max = PhotoManager.Instance.currentMax;
            photoSizeSlider.value = PhotoManager.Instance.Max;
            if (background.activeSelf == true)
            {
                background.SetActive(false);
            }
        }
    }

    IEnumerator ColliderIsTrigger(bool active)
    {
        foreach (DflipPhoto a in PhotoManager.Instance.photos)
        {
            a.gameobject.GetComponent<BoxCollider>().isTrigger = active;
        }
        SystemManager.Instance.AddAttractor(SystemManager.AttractorState.SCALE_UP);
        SystemManager.Instance.AddAttractor(SystemManager.AttractorState.AVOID_SCALE);
        yield return null;
    }
}
