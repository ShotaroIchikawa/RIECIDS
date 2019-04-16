using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortButton : MonoBehaviour {
    public GameObject buttonPrefab;
    public List<GameObject> buttonList;
    public GameObject deselectButton;
    public GameObject fileButton;
    public enum axes
    {
        X,
        Y,
        Z,
        S
    }
    public axes axis;
    public bool openChecker;

    public void Close()
    {
        foreach (GameObject a in buttonList)
        {
            a.SetActive(false);
        }
        openChecker = false;
    }

    public void OnSortButtonClick()
    {
        if (openChecker == false)
        {
            foreach(GameObject a in buttonList)
            {
                a.SetActive(true);
            }
            if (fileButton.GetComponent<OpenMenu>().openChecker == true)
            {
                fileButton.GetComponent<OpenMenu>().OnButtonClick();
            }
        }
        else
        {
            foreach (GameObject a in buttonList)
            {
                a.SetActive(false);
            }
        }
        openChecker = openChecker ? false : true;
    }

    public void CreateSortMenu(List<string> data) 
    {
        buttonList = new List<GameObject>();
        foreach (string a in data)
        {
            Transform Parent = gameObject.transform.parent.gameObject.transform;
            GameObject menuButton = Instantiate(buttonPrefab) as GameObject;
            if (axis == axes.S)
            {
                var asb = menuButton.AddComponent<AdjustScaleButton>();
                asb.Initialize();
                menuButton.GetComponent<Button>().onClick.AddListener(() => asb.MyOnClick(a));
            }
            else
            {
                var smb = menuButton.AddComponent<SortMenuButton>();
                smb.Initialize();
                menuButton.GetComponent<Button>().onClick.AddListener(() => smb.MyOnClick(a, axis));
            }
            menuButton.transform.SetParent(Parent, false);

            string buttonText = a;
            switch (a)
            {
                case "輝度":
                    buttonText = "Luminance";
                    break;
                case "彩度":
                    buttonText = "Saturation";
                    break;
                case "撮影年月":
                    buttonText = "Shot Date";
                    break;
                default:
                    break;
            }
            if (buttonText.Contains("[Y:M]"))
            {
                buttonText = buttonText.Replace("[Y:M]", "");
            }
            if(buttonText.Length > 12)
            {
                //文字がbuttonのrectを超える場合の処理
                buttonText = buttonText.Remove(10, buttonText.Length - 10);
                buttonText = buttonText.Insert(10, "…");
            }
   
            menuButton.transform.Find("Text").GetComponent<Text>().text = buttonText;

            if (deselectButton == null)
            {
                deselectButton = gameObject.transform.parent.Find("Deselect").gameObject;
            }
            buttonList.Add(menuButton);
        }
        buttonList.Add(deselectButton);

        foreach (GameObject a in buttonList)
        {
            a.SetActive(false);
        }
    }



	// Use this for initialization
	void Start () {
        openChecker = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
