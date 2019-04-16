using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WallLabel : MonoBehaviour {

    public PhotoSprite mySprite;
    public DflipPhoto myPhoto;
    public float xLength= 0.1f;
	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
    public void UpdateWallLable()
    {
        Vector3 currentPosition = mySprite.gameObject.transform.localPosition;
        Vector3 currentScale = new Vector3(mySprite.GetComponent<Renderer>().bounds.size.x / 2, mySprite.GetComponent<Renderer>().bounds.size.y, 0.1f);
        this.transform.localPosition = new Vector3(currentPosition.x + 3 * currentScale.x / 2 + xLength, currentPosition.y, -20f);
        this.transform.localScale = currentScale;
        WallAvoidWall();
    }

    private void WallAvoidWall()
    {
        float x = transform.localPosition.x;
        float y = transform.localPosition.y;

        x = x < AutoDisplayAdjuster.Instance.BottomLeft().x ? AutoDisplayAdjuster.Instance.BottomLeft().x : x;
        x = x > AutoDisplayAdjuster.Instance.TopRight().x ? AutoDisplayAdjuster.Instance.TopRight().x : x;
        y = y < AutoDisplayAdjuster.Instance.BottomLeft().y ? AutoDisplayAdjuster.Instance.BottomLeft().y : y;
        y = y > AutoDisplayAdjuster.Instance.TopRight().y ? AutoDisplayAdjuster.Instance.TopRight().y : y;

        transform.localPosition = new Vector3(x, y, transform.localPosition.z);
    }
}
