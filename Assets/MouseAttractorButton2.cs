using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseAttractorButton2 : MonoBehaviour
{
    public List<GameObject> buttonList;
    public GameObject buttonPrefab;
    public GameObject deselectButton;

    private bool openChecker;
    public void OnClick()
    {
        if (openChecker == false)
        {
            foreach (GameObject a in buttonList)
            {
                a.SetActive(true);
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

    public void Close()
    {
        foreach (GameObject a in buttonList)
        {
            a.SetActive(false);
        }
        openChecker = false;
    }

    public void CreateAttractorMenu(List<string> property)
    {
        buttonList = new List<GameObject>();
        foreach (string a in property)
        {
            string buttonText = a;
            if (buttonText != "jp" && buttonText != "en")
            {
                continue;
            }
            Transform Parent = gameObject.transform.parent.gameObject.transform;
            GameObject menuButton = Instantiate(buttonPrefab) as GameObject;
            menuButton.GetComponent<Button>().onClick.AddListener(() => Select(a));
            menuButton.transform.SetParent(Parent, false);           
            if (a.Length > 12)
            {
                //文字がbuttonのrectを超える場合の処理
                buttonText = buttonText.Remove(10, buttonText.Length - 10);
                buttonText = buttonText.Insert(10, "…");
            }
            menuButton.transform.Find("Text").GetComponent<Text>().text = buttonText;
            buttonList.Add(menuButton);
        }
        deselectButton.GetComponent<Button>().onClick.AddListener(() => Deselect());
        buttonList.Add(deselectButton);

        foreach (GameObject a in buttonList)
        {
            a.SetActive(false);
        }
    }

    public void Select(string key)
    {
        //SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.COLOR);
        PhotoManager.Instance.mouseAttractorKey2 = PhotoManager.Instance.keywordCode[key];
        SystemManager.Instance.AddAttractor(SystemManager.AttractorState.LANGUAGE);
        gameObject.transform.Find("Text").GetComponent<Text>().text = key;

        Close();
    }

    public void Deselect()
    {
        SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.LANGUAGE);
        //SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.COLOR);
        gameObject.transform.Find("Text").GetComponent<Text>().text = "-";
        Close();
    }


    // Use this for initialization
    void Start()
    {
        openChecker = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
