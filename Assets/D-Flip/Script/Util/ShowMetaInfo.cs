using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ShowMetaInfo : MonoBehaviour {

    private Text text;
    public DflipPhoto myPhoto;
    public bool showText = false;
	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<Text>();
        myPhoto = gameObject.transform.parent.gameObject.GetComponent<WallLabel>().myPhoto;
        //Dictionary<string, int> tempCode = PhotoManager.Instance.keywordCode;
	}
	
	// Update is called once per frame
	void Update () {
        if (myPhoto.showWall)
        {
            if (!showText)
            {
                Dictionary<string, int> tempCode = PhotoManager.Instance.keywordCode;
                foreach (KeyValuePair<string, int> pair in tempCode)
                {
                    if (myPhoto.metaword.ContainsKey(pair.Value))
                    {
                        for (int i = 0; i < myPhoto.metaword[pair.Value].Count; i++)
                        {
                            Debug.Log(pair.Key + " " + myPhoto.metaword[pair.Value][i]);
                            //text.text += pair.Key + ":" + myPhoto.metaword[pair.Value][i] + "\n";
                            text.text += myPhoto.metaword[pair.Value][i] + "\n";
                        }

                    }
                } 
                showText = true;
            }

        }
	}
}
