using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExchangeSlider : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler {
    GameObject myBar;
    public enum Change
    {
        XY,
        XZ,
        YZ,
        XS,
        YS,
        ZS
    }

    public void OnPointerDown(PointerEventData ed)
    {
        GUIManager.startExchange = true;
        if (myBar.name == "XAxisBar")
        {
            GUIManager.startBar = GUIManager.BarName.XAxisBar;
        }
        else if(myBar.name == "YAxisBar")
        {
            GUIManager.startBar = GUIManager.BarName.YAxisBar;
        }
        else if (myBar.name == "ZAxisBar")
        {
            GUIManager.startBar = GUIManager.BarName.ZAxisBar;
        }
        else if (myBar.name == "SAxisBar")
        {
            GUIManager.startBar = GUIManager.BarName.SAxisBar;
        }
    }

    public void OnPointerUp(PointerEventData ed)
    {
        if (myBar.name == GUIManager.startBar.ToString()) // 交換開始軸で交換メソッドを呼ぶ
        {
            if (GUIManager.startExchange && GUIManager.endExchange)
            {
                if (GUIManager.startBar != GUIManager.endBar)
                {
                    Exchange(GUIManager.startBar, GUIManager.endBar);
                }
                else
                {
                    GUIManager.startExchange = false;
                    GUIManager.endExchange = false;
                }
            }
            else
            {
                GUIManager.startExchange = false;
                GUIManager.endExchange = false;
            }
        }
        
    }

    public void OnPointerEnter(PointerEventData ed)
    {
        GUIManager.endExchange = true;
        if (myBar.name == "XAxisBar")
        {
            GUIManager.endBar = GUIManager.BarName.XAxisBar;
        }
        else if (myBar.name == "YAxisBar")
        {
            GUIManager.endBar = GUIManager.BarName.YAxisBar;
        }
        else if (myBar.name == "ZAxisBar")
        {
            GUIManager.endBar = GUIManager.BarName.ZAxisBar;
            if (showerImage.activeSelf == false && (SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
            {
                showerImage.SetActive(true);
            }
        }
        else if (myBar.name == "SAxisBar")
        {
            GUIManager.endBar = GUIManager.BarName.SAxisBar;
        }
    }

    public void OnPointerExit(PointerEventData ed)
    {
        if (myBar.name == "ZAxisBar")
        {
            if (showerImage.activeSelf == true && (SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
            {
                showerImage.SetActive(false);
            }
        }
    }

    void Exchange(GUIManager.BarName start, GUIManager.BarName end)
    {
        switch (start)
        {
            case GUIManager.BarName.XAxisBar:
                switch (end)
                {
                    case GUIManager.BarName.YAxisBar:
                        ExchangeXY();
                        break;
                    case GUIManager.BarName.ZAxisBar:
                        ExchangeXZ();
                        break;
                    case GUIManager.BarName.SAxisBar:
                        ExchangeXS();
                        break;
                }
                break;
            case GUIManager.BarName.YAxisBar:
                switch (end)
                {
                    case GUIManager.BarName.XAxisBar:
                        ExchangeXY();
                        break;
                    case GUIManager.BarName.ZAxisBar:
                        ExchangeYZ();
                        break;
                    case GUIManager.BarName.SAxisBar:
                        ExchangeYS();
                        break;
                }
                break;
            case GUIManager.BarName.ZAxisBar:
                switch (end)
                {
                    case GUIManager.BarName.XAxisBar:
                        ExchangeXZ();
                        break;
                    case GUIManager.BarName.YAxisBar:
                        ExchangeYZ();
                        break;
                    case GUIManager.BarName.SAxisBar:
                        ExchangeZS();
                        break;
                }
                break;
            case GUIManager.BarName.SAxisBar:
                switch (end)
                {
                    case GUIManager.BarName.XAxisBar:
                        ExchangeXS();
                        break;
                    case GUIManager.BarName.YAxisBar:
                        ExchangeYS();
                        break;
                    case GUIManager.BarName.ZAxisBar:
                        ExchangeZS();
                        break;
                }
                break;
        }
    }

    void ExchangeXY()
    {
        int xKey = pm.hSortKey;
        int yKey = pm.vSortKey;
        ExchangeAttractor(xKey, yKey, Change.XY);
        ExchangeAxisBar(Change.XY);
    }

    void ExchangeXZ()
    {
        int xKey = pm.hSortKey;
        int zKey = pm.dSortKey;
        ExchangeAttractor(xKey, zKey, Change.XZ);
        ExchangeAxisBar(Change.XZ);
    }

    void ExchangeYZ()
    {
        int yKey = pm.vSortKey;
        int zKey = pm.dSortKey;
        ExchangeAttractor(yKey, zKey, Change.YZ);
        ExchangeAxisBar(Change.YZ);
    }

    void ExchangeXS()
    {
        int xKey = pm.hSortKey;
        int sKey = pm.sSortKey;
        ExchangeAttractor(xKey, sKey, Change.XS);
        ExchangeAxisBar(Change.XS);
    }

    void ExchangeYS()
    {
        int yKey = pm.vSortKey;
        int sKey = pm.sSortKey;
        ExchangeAttractor(yKey, sKey, Change.YS);
        ExchangeAxisBar(Change.YS);
    }

    void ExchangeZS()
    {
        int zKey = pm.dSortKey;
        int sKey = pm.sSortKey;
        ExchangeAttractor(zKey, sKey, Change.ZS);
        ExchangeAxisBar(Change.ZS);
    }

    public GameObject showerImage;
    GameObject otherBar1;
    GameObject otherBar2;
    GameObject otherBar3;
    GameObject zSlider;
    PhotoManager pm;
	// Use this for initialization
	void Start () {
        pm = PhotoManager.Instance;
        myBar = gameObject;
        if (myBar.name == "XAxisBar")
        {
            otherBar1 = GameObject.Find("YAxisBar");
            otherBar2 = GameObject.Find("ZAxisBar");
            otherBar3 = GameObject.Find("SAxisBar");
            zSlider = otherBar2.transform.Find("ZSlider").gameObject;
        }
        else if (myBar.name == "YAxisBar")
        {
            otherBar1 = GameObject.Find("XAxisBar");
            otherBar2 = GameObject.Find("ZAxisBar");
            otherBar3 = GameObject.Find("SAxisBar");
            zSlider = otherBar2.transform.Find("ZSlider").gameObject;
        }
        else if (myBar.name == "ZAxisBar")
        {
            otherBar1 = GameObject.Find("XAxisBar");
            otherBar2 = GameObject.Find("YAxisBar");
            otherBar3 = GameObject.Find("SAxisBar");
            zSlider = myBar.transform.Find("ZSlider").gameObject;
        }
        else if (myBar.name == "SAxisBar")
        {
            otherBar1 = GameObject.Find("XAxisBar");
            otherBar2 = GameObject.Find("YAxisBar");
            otherBar3 = GameObject.Find("ZAxisBar");
            zSlider = otherBar3.transform.Find("ZSlider").gameObject;
        }

    }

    void ExchangeAttractor(int firstKey, int secoundKey, Change bar)
    {
        if (bar == Change.XY)
        {
            pm.hSortKey = secoundKey;
            pm.vSortKey = firstKey;
        }
        else if (bar == Change.XZ)
        {
            pm.hSortKey = secoundKey;
            pm.dSortKey = firstKey;            
        }
        else if(bar == Change.YZ)
        {
            pm.vSortKey = secoundKey;
            pm.dSortKey = firstKey;
        }
        else if (bar == Change.XS)
        {
            pm.hSortKey = secoundKey;
            pm.sSortKey = firstKey;
        }
        else if (bar == Change.YS)
        {
            pm.vSortKey = secoundKey;
            pm.sSortKey = firstKey;
        }
        else if (bar == Change.ZS)
        {
            pm.dSortKey = secoundKey;
            pm.sSortKey = firstKey;
        }

        foreach (DflipPhoto a in pm.photos)
        {
            a.Adjacency.Clear();
        }
    }

    void ExchangeAxisBar(Change bar)
    {
        if (bar == Change.XY)
        {
            string temp = myBar.transform.Find("Text").GetComponent<Text>().text;
            myBar.transform.Find("Text").GetComponent<Text>().text = otherBar1.transform.Find("Text").GetComponent<Text>().text;
            otherBar1.transform.Find("Text").GetComponent<Text>().text = temp;

            temp = myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
            myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = otherBar1.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
            otherBar1.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text =temp;

            temp = myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
            myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = otherBar1.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
            otherBar1.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = temp;
        }
        else if (bar == Change.XZ)
        {

            if (myBar.name == "XAxisBar")
            {
                string max, min;
                string temp = myBar.transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Text").GetComponent<Text>().text = otherBar2.transform.Find("Text").GetComponent<Text>().text;
                otherBar2.transform.Find("Text").GetComponent<Text>().text = temp;

                temp = myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                max = temp;
                myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = otherBar2.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                otherBar2.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = temp;

                temp = myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                min = temp;
                myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = otherBar2.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                otherBar2.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = temp;

                Slider zs = zSlider.GetComponent<Slider>();
                zs.maxValue = float.Parse(max);
                zs.minValue = float.Parse(min);
                zs.value = zs.minValue;
                zSlider.GetComponent<ZAxisSlider>().OnValueChanging();
            }
            else
            {
                string max, min;
                string temp = otherBar1.transform.Find("Text").GetComponent<Text>().text;
                max = temp;
                otherBar1.transform.Find("Text").GetComponent<Text>().text = myBar.transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Text").GetComponent<Text>().text = temp;


                temp = otherBar1.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                max = temp;                
                otherBar1.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = temp;

                temp = otherBar1.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                min = temp;
                otherBar1.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = temp;

                Slider zs = zSlider.GetComponent<Slider>();
                zs.maxValue = float.Parse(max);
                zs.minValue = float.Parse(min);
                zs.value = zs.minValue;
                zSlider.GetComponent<ZAxisSlider>().OnValueChanging();
            }
        }
        else if(bar == Change.YZ)
        {
            string max, min;
            string otherMax, otherMin;
            string temp = myBar.transform.Find("Text").GetComponent<Text>().text;
            myBar.transform.Find("Text").GetComponent<Text>().text = otherBar2.transform.Find("Text").GetComponent<Text>().text;
            otherBar2.transform.Find("Text").GetComponent<Text>().text = temp;

      
            max = myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
            otherMax = otherBar2.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
            myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = otherMax;
            otherBar2.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = max;

            min = myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
            otherMin = otherBar2.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
            myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = otherMin;
            otherBar2.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = min;

            Slider zs = zSlider.GetComponent<Slider>();

            if (myBar.name == "ZAxisBar")
            {
                zs.maxValue = float.Parse(otherMax);
                zs.minValue = float.Parse(otherMin);
            }
            else
            {
                zs.maxValue = float.Parse(max);
                zs.minValue = float.Parse(min);
            }
            zs.value = zs.minValue;
            zSlider.GetComponent<ZAxisSlider>().OnValueChanging();
        }
        else if (bar == Change.XS)
        {
            if (myBar.name == "XAxisBar")
            {
                string max, min;
                string temp = myBar.transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Text").GetComponent<Text>().text = otherBar3.transform.Find("Text").GetComponent<Text>().text;
                otherBar3.transform.Find("Text").GetComponent<Text>().text = temp;

                temp = myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                max = temp;
                myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = otherBar3.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                otherBar3.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = temp;

                temp = myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                min = temp;
                myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = otherBar3.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                otherBar3.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = temp;
            }
            else
            {
                string max, min;
                string temp = otherBar1.transform.Find("Text").GetComponent<Text>().text;
                max = temp;
                otherBar1.transform.Find("Text").GetComponent<Text>().text = myBar.transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Text").GetComponent<Text>().text = temp;


                temp = otherBar1.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                max = temp;
                otherBar1.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = temp;

                temp = otherBar1.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                min = temp;
                otherBar1.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = temp;
            }
        }
        else if (bar == Change.YS)
        {
            if (myBar.name == "YAxisBar")
            {
                string max, min;
                string temp = myBar.transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Text").GetComponent<Text>().text = otherBar3.transform.Find("Text").GetComponent<Text>().text;
                otherBar3.transform.Find("Text").GetComponent<Text>().text = temp;

                temp = myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                max = temp;
                myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = otherBar3.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                otherBar3.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = temp;

                temp = myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                min = temp;
                myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = otherBar3.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                otherBar3.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = temp;
            }
            else
            {
                string max, min;
                string temp = otherBar2.transform.Find("Text").GetComponent<Text>().text;
                max = temp;
                otherBar2.transform.Find("Text").GetComponent<Text>().text = myBar.transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Text").GetComponent<Text>().text = temp;


                temp = otherBar2.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                max = temp;
                otherBar2.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = temp;

                temp = otherBar2.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                min = temp;
                otherBar2.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
                myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = temp;
            }
        }
        else if (bar == Change.ZS)
        {
            string max, min;
            string otherMax, otherMin;
            string temp = myBar.transform.Find("Text").GetComponent<Text>().text;
            myBar.transform.Find("Text").GetComponent<Text>().text = otherBar3.transform.Find("Text").GetComponent<Text>().text;
            otherBar3.transform.Find("Text").GetComponent<Text>().text = temp;


            max = myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
            otherMax = otherBar3.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text;
            myBar.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = otherMax;
            otherBar3.transform.Find("Max").transform.Find("Text").GetComponent<Text>().text = max;

            min = myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
            otherMin = otherBar3.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text;
            myBar.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = otherMin;
            otherBar3.transform.Find("Min").transform.Find("Text").GetComponent<Text>().text = min;

            Slider zs = zSlider.GetComponent<Slider>();

            if (myBar.name == "ZAxisBar")
            {
                zs.maxValue = float.Parse(otherMax);
                zs.minValue = float.Parse(otherMin);
            }
            else
            {
                zs.maxValue = float.Parse(max);
                zs.minValue = float.Parse(min);
            }
            zs.value = zs.minValue;
            zSlider.GetComponent<ZAxisSlider>().OnValueChanging();
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
