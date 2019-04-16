using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeselectButton : MonoBehaviour {
    public SortButton.axes axis;
    PhotoManager pm;
    SortButton buttonX;
    SortButton buttonY;
    SortButton buttonZ;
    SortButton buttonS;
    GameObject xAxisBar;
    GameObject yAxisBar;
    GameObject zAxisBar;
    GameObject sAxisBar;
    Slider zSlider;
    GameObject colliderToggle;

    public void OnButtonClick()
    {
        ResetAxisBar(axis);
        ResetAttractor(axis);
    }

    public void Initiarize()
    {
        pm = GameObject.Find("Main Camera").GetComponent<PhotoManager>();
        xAxisBar = GameObject.Find("XAxisBar");
        yAxisBar = GameObject.Find("YAxisBar");
        zAxisBar = GameObject.Find("ZAxisBar");
        sAxisBar = GameObject.Find("SAxisBar");
        buttonX = GameObject.Find("Xbutton").transform.Find("Button").gameObject.GetComponent<SortButton>();
        buttonY = GameObject.Find("Ybutton").transform.Find("Button").gameObject.GetComponent<SortButton>();
        buttonZ = GameObject.Find("Zbutton").transform.Find("Button").gameObject.GetComponent<SortButton>();
        buttonS = GameObject.Find("Sbutton").transform.Find("Button").gameObject.GetComponent<SortButton>();
        if (zAxisBar != null)
        {
            zSlider = zAxisBar.transform.Find("ZSlider").gameObject.GetComponent<Slider>();
        }
        colliderToggle = GameObject.Find("ColliderToggle");
    }

    public void ResetAttractor(SortButton.axes axis)
    {
        if (axis == SortButton.axes.X)
        {
            pm.hSortKey = 0;
            SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.HORIZONTAL_SORT);
            buttonX.Close();
        }
        else if (axis == SortButton.axes.Y)
        {
            pm.vSortKey = 0;
            SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.VERTICAL_SORT);
            buttonY.Close();
        }
        else if(axis == SortButton.axes.Z)
        {
            pm.dSortKey = 0;
            SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.DEPTH_SORT);
            pm.ResetPhotosColor();
            buttonZ.Close();
        }
        else
        {
            pm.sSortKey = 0;
            SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.ADJUST_SCALE);
            SystemManager.Instance.AddAttractor(SystemManager.AttractorState.SCALE_UP);
            SystemManager.Instance.AddAttractor(SystemManager.AttractorState.AVOID_SCALE);
            if (colliderToggle.GetComponent<Toggle>().isOn == false)
            {
                StartCoroutine(ColliderIsTrigger(true));
                colliderToggle.GetComponent<Toggle>().isOn = true;
                colliderToggle.GetComponent<Toggle>().interactable = false;
            }
            buttonS.Close();
        }
    }

    public void ResetAxisBar(SortButton.axes axis)
    {
        if (axis == SortButton.axes.X)
        {
            if (xAxisBar.activeSelf == true)
            {
                xAxisBar.SetActive(false);
            }
        }
        else if (axis == SortButton.axes.Y)
        {
            if (yAxisBar.activeSelf == true)
            {
                yAxisBar.SetActive(false);
            }
        }
        else if(axis == SortButton.axes.Z)
        {
            if (zAxisBar.activeSelf == true)
            {
                zAxisBar.SetActive(false);
            }
        }
        else
        {
            if (sAxisBar.activeSelf == true)
            {
                sAxisBar.SetActive(false);
            }
        }
    }

    IEnumerator ColliderIsTrigger(bool active)
    {
        foreach (DflipPhoto a in PhotoManager.Instance.photos)
        {
            a.gameobject.GetComponent<BoxCollider>().isTrigger = active;
        }

        yield return null;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
