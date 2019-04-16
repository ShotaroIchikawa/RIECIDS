using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeywordInput : MonoBehaviour {
    private Stroke myStroke;

    public void Initialize (Stroke stroke)
    {
        myStroke = stroke;
        gameObject.transform.Find("CancelButton").GetComponent<Button>().onClick.AddListener(() => DestroyStrokeObject());
        Transform tb = gameObject.transform;
        gameObject.transform.Find("TransformButton").GetComponent<Button>().onClick.AddListener(() => StrokeManager.Instance.CreateTransformButton(myStroke, tb));
        gameObject.GetComponent<InputField>().onEndEdit.AddListener((a) => SetKeyword(a));
        gameObject.GetComponent<InputField>().ActivateInputField();
    }

    public void DestroyStrokeObject()
    {
        Destroy(myStroke.gameObject);
        Destroy(this.gameObject);
        GameObject.Find("Main Camera").GetComponent<StrokeManager>().strokes.Remove(myStroke);
    }


    public void SetKeyword(string keyword)
    {
        string tagword = "";
        string metaword = "";
        if(keyword.Contains("[]"))
        {
            tagword = keyword.Remove(1);
            metaword = keyword.Remove(0, 1);
        }

        if (tagword == "[]") 
        {
            //タグ言語的な形で写真にタグ付けを行う
        }
        else
        {
            myStroke.keyword = keyword;
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
