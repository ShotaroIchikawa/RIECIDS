using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour {

    List<GameObject> myMenu;
    public GameObject parent_;
    public bool openChecker;

    public GameObject X;
    public GameObject Y;
    public GameObject Z;
    public GameObject S;

    public void OnButtonClick()
    {
        if(openChecker == false)
        {
            foreach (GameObject a in myMenu)
            {
                a.SetActive(true);
            }

            #region 他のボタンが開いていたら閉じる
            if (X.GetComponent<SortButton>().openChecker == true) 
            {
                X.GetComponent<SortButton>().Close();
            }
            if (Y.GetComponent<SortButton>().openChecker == true) 
            {
                Y.GetComponent<SortButton>().Close();
            }
            if (Z.GetComponent<SortButton>().openChecker == true) 
            {
                Z.GetComponent<SortButton>().Close();
            }
            if (S.GetComponent<SortButton>().openChecker == true) 
            {
                S.GetComponent<SortButton>().Close();
            }
            #endregion
        }
        else
        {
            foreach(GameObject a in myMenu)
            {
                a.SetActive(false);
            }
        }
        openChecker = openChecker ? false : true;
    }

    // Use this for initialization
    void Start () {
        myMenu = new List<GameObject>();
        Transform parent = parent_.transform;
        for (int i = 0; i < parent.childCount; i++)
        {
            myMenu.Add(parent.GetChild(i).gameObject);
        }
        myMenu.Remove(this.gameObject);

        foreach (GameObject a in myMenu)
        {
            a.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
