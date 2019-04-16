using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBackgroundImage : MonoBehaviour {

    public GameObject background;
    public GameObject FileMenu;
    public void OnButtonClick()
    {
        if (background.activeSelf == true)
        {
            background.SetActive(false);
        }
        FileMenu.GetComponent<OpenMenu>().OnButtonClick();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
