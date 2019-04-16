using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraChangeToggle : MonoBehaviour {
    public GameObject CameraTransformInputs;
    public Camera camera;
    public Camera zCamera;
    public GameObject ZSlider;
    Slider slider;
    public GameObject axes;
    public GameObject MouseModeChangeButton;

    private Image image;
    public void OnToggleChanged()
    {
        image.enabled = image.enabled ? false : true;
        if ((SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
        {
            SystemManager.cameraState =  SystemManager.cameraState & ~SystemManager.CameraState.ORTHOGRAPHIC;
            SystemManager.cameraState |= SystemManager.CameraState.PERSPECTIVE;
            if ((SystemManager.pointingState & SystemManager.PointingState.VENN) == SystemManager.PointingState.VENN)
            {
                MouseModeChangeButton.GetComponent<MouseModeChangeButton>().MouseStateChange();
            }
            axes.SetActive(true);
            zCamera.gameObject.SetActive(true);
            camera.enabled = false;
            camera.gameObject.GetComponent<PhysicsRaycaster>().enabled = false;
        }
        else
        {
            SystemManager.cameraState = SystemManager.cameraState & ~SystemManager.CameraState.PERSPECTIVE;
            SystemManager.cameraState |= SystemManager.CameraState.ORTHOGRAPHIC;
            axes.SetActive(false);
            zCamera.gameObject.SetActive(false);
            camera.enabled = true;
            camera.gameObject.GetComponent<PhysicsRaycaster>().enabled = true;
        }
    }


	// Use this for initialization
	void Start () {
        image = CameraTransformInputs.GetComponent<Image>();
        slider = ZSlider.GetComponent<Slider>();
        slider.value = slider.minValue;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
