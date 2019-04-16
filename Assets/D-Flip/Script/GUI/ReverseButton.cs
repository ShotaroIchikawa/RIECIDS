using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReverseButton : MonoBehaviour {

    public Sprite normalImage;
    public Sprite reverseImage;

    public void OnButtonClick()
    {
        if (SystemManager.zAxisDirection == true)
        {
            SystemManager.zAxisDirection = false;
            gameObject.transform.Find("Background").gameObject.GetComponent<Image>().sprite = reverseImage;

        }
        else
        {
            SystemManager.zAxisDirection = true;
            gameObject.transform.Find("Background").gameObject.GetComponent<Image>().sprite = normalImage;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
