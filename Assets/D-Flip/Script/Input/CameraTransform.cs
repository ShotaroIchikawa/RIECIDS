using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraTransform : MonoBehaviour, IDragHandler {
    public Camera zCamera;
    public float cameraTransformWeight;

    public void OnDrag(PointerEventData ed)
    {
        float angle = ed.delta.magnitude / cameraTransformWeight;
        if (ed.delta.x > 0)
        {
            zCamera.transform.RotateAround(new Vector3(0, zCamera.transform.localPosition.y, 5), Vector3.up, angle);
        }
        else if(ed.delta.x < 0)
        {
            zCamera.transform.RotateAround(new Vector3(0, zCamera.transform.localPosition.y, 5), Vector3.up, -angle);
        }

        if(ed.delta.y > 1 && zCamera.transform.localPosition.y > -4)
        {
            zCamera.transform.localPosition += -angle * 0.1f * Vector3.up;
        }
        else if(ed.delta.y < 1 && zCamera.transform.localPosition.y < 4)
        {
            zCamera.transform.localPosition += angle * 0.1f * Vector3.up;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //temp
        if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.C))
        {
            Image temp = gameObject.GetComponent<Image>();
            if (temp.enabled == true)
            {
                gameObject.GetComponent<Image>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<Image>().enabled = true;
            }

        }
	}
}
