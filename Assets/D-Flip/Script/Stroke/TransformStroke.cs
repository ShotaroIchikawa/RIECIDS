using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TransformStroke : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler{
    Vector3 mousePosition;
    LineRenderer line;
    Vector3 uiPosition;

    public void OnPointerDown(PointerEventData ed)
    {
        uiPosition = vennUITransform.transform.position - Input.mousePosition;
    }

    public void OnDrag(PointerEventData ed)
    {
        mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = mousePosition - stroke.Center;
        mousePosition.z = 0;
        for(int i = 0; i < stroke.Strokes.Count; i++)
        {
            stroke.Strokes[i] += mousePosition;
        }
        stroke.RenderVenn();
        stroke.CalcCenter(stroke.Strokes);

        gameObject.transform.position = Input.mousePosition;
        vennUITransform.position = gameObject.transform.position + uiPosition;
    }

    public void OnPointerUp(PointerEventData ed)
    {
        Destroy(gameObject);
    }

    Camera camera;
    Stroke stroke;
    Transform vennUITransform;
    public void Initialize(Stroke str, Camera cam, Transform parent)
    {
        stroke = str;
        camera = cam;
        vennUITransform = parent; 
        line = new LineRenderer();
        line = stroke.gameObject.GetComponent<LineRenderer>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
