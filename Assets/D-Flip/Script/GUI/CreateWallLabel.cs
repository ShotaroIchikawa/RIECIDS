using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateWallLabel : MonoBehaviour {

	// Use this for initialization
    public GameObject WallLabel;
    //public DflipPhoto dflipPhoto;

    public void Create(List<DflipPhoto> photos)
    {
        foreach (DflipPhoto a in photos)
        {
            if (a.isClicked && a.showWall == false)
            {
                GameObject newWallLabel = Instantiate(WallLabel, a.sprite.gameObject.transform.position, a.sprite.gameObject.transform.rotation);
                a.showWall = true;
                a.sprite.myLabel = newWallLabel.GetComponent<WallLabel>();
                a.sprite.myLabel.mySprite = a.sprite;
                a.sprite.myLabel.myPhoto = a;

            }            
            else if (a.isClicked == false && a.showWall == true)
            {
                a.showWall = false;
                a.sprite.myLabel.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
                Destroy(a.sprite.myLabel.gameObject, 0.1f);
            }
        }
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {        
		
	}
}
