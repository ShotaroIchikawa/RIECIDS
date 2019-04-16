using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderValueShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject showerImage;

    public void OnPointerEnter(PointerEventData ed)
    {
        if(showerImage.activeSelf == false && (SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
        {
            showerImage.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData ed)
    {
        if(showerImage.activeSelf == true && (SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
        {
            showerImage.SetActive(false);
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
