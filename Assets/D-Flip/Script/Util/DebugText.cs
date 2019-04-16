using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour {
     
    private Text text;
    public static texts texts = new texts();
	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<Text>();
	}

    string updateText;
    string lastText;
	// Update is called once per frame
	void Update () {
        if (InputManager.settingMode == true)
        {
            text.text = ": " + texts.scaleFactor + "\n";
            text.text += ": " + texts.doubleClick + "\n";
            text.text += ": " + texts.interactive + "\n";
            text.text += ": " + texts.follower + "\n";
            text.text += "\n";
            text.text += "\n";
            text.text += ": " + texts.topOffset + "\n";
            text.text += ": " + texts.bottomOffset + "\n";
            text.text += ": " + texts.rightOffset + "\n";
            text.text += ": " + texts.leftOffset + "\n";

        }

        
    }
}
public struct texts
{
    public string scaleFactor;
    public string doubleClick;
    public string interactive;
    public string follower;
    public string topOffset;
    public string bottomOffset;
    public string rightOffset;
    public string leftOffset;

}
