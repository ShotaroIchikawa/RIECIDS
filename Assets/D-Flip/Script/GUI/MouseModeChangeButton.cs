using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class MouseModeChangeButton : MonoBehaviour {

    public Texture2D VennMouseCursor;
    private bool clickFlag = false;
    private Image image;
    public void MouseStateChange()
    {
        if ((SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
        {
            if ((SystemManager.pointingState & SystemManager.PointingState.POINTING) == SystemManager.PointingState.POINTING)
            {
                SystemManager.pointingState = SystemManager.pointingState & ~SystemManager.PointingState.POINTING;
                SystemManager.pointingState |= SystemManager.PointingState.VENN;
                Cursor.SetCursor(VennMouseCursor, new Vector2(0, VennMouseCursor.height + 20), CursorMode.Auto);
            }
            else if ((SystemManager.pointingState & SystemManager.PointingState.VENN) == SystemManager.PointingState.VENN)
            {
                SystemManager.pointingState = SystemManager.pointingState & ~SystemManager.PointingState.VENN;
                SystemManager.pointingState |= SystemManager.PointingState.POINTING;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
        else
        {
            SystemManager.pointingState = SystemManager.pointingState & ~SystemManager.PointingState.VENN;
            SystemManager.pointingState |= SystemManager.PointingState.POINTING;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        image = GameObject.Find("Canvas/MainMenuBar/VennButton").GetComponent<Button>().gameObject.GetComponent<Image>();
        //Sprite tmp = new Sprite();
        clickFlag = !clickFlag;
        if (clickFlag)
        {
            Sprite tmp = Resources.Load("Elements/redpen", typeof(Sprite)) as Sprite;
            image.sprite = tmp;
        }
        else
        {
            Sprite tmp = Resources.Load("Elements/penSprite", typeof(Sprite)) as Sprite;
            image.sprite = tmp;
        }
        

    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
