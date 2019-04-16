using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StrokeManager : MonoBehaviour
{
    public static StrokeManager Instance;
    public List<Stroke> strokes;

    public GameObject keywordFieldPrefab;
    public GameObject transformStrokePrefab;
    float scaleFactor;
    Camera camera;

    public void CreateKeywordField(Stroke stroke)
    {
        GameObject inputField = Instantiate(keywordFieldPrefab);
        scaleFactor = GameObject.Find("Canvas").gameObject.GetComponent<Canvas>().scaleFactor / 1.2f; //1.2はプレファブを作った時のscaleFactor
        inputField.transform.localScale *= scaleFactor;
        inputField.transform.localPosition = camera.WorldToScreenPoint(stroke.Strokes[0]);
        inputField.transform.SetParent(GameObject.Find("StrokeKeywordInputs").transform);
        var keywordInput = inputField.AddComponent<KeywordInput>();
        stroke.inputField = inputField;
        keywordInput.Initialize(stroke);
    }


    public void CreateTransformButton(Stroke stroke, Transform parent)
    {
        GameObject transformImage = Instantiate(transformStrokePrefab);
        transformImage.transform.localScale *= scaleFactor;
        transformImage.transform.localPosition = camera.WorldToScreenPoint(stroke.Center);
        transformImage.transform.SetParent(parent);
        var _t = transformImage.AddComponent<TransformStroke>();
        _t.Initialize(stroke, camera, parent);
    }



    void Awake()
    {
        strokes = new List<Stroke>();
    }

    void Start()
    {
        camera = GetComponent<Camera>();
        Instance = this;
    }

    void Update()
    {

    }
}
