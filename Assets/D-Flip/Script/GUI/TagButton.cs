using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagButton : MonoBehaviour {

    private Image image;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnButtonClick()
    {
        PhotoManager.Instance.ShowWallLabel = !PhotoManager.Instance.ShowWallLabel;
        Debug.Log(PhotoManager.Instance.ShowWallLabel);
        image = GameObject.Find("Canvas/MainMenuBar/TagButton").GetComponent<Button>().gameObject.GetComponent<Image>();
       // Sprite tmp = new Sprite();
        if (PhotoManager.Instance.ShowWallLabel)
        {
            Sprite tmp = Resources.Load("Elements/RedTag", typeof(Sprite)) as Sprite;
            image.sprite = tmp;
        }
        else
        {
            Sprite tmp = Resources.Load("Elements/Tag", typeof(Sprite)) as Sprite;
            image.sprite = tmp;
        }
       
    }
}
