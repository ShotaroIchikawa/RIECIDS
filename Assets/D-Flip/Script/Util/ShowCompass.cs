using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCompass : MonoBehaviour
{
    public GameObject directionX;
    public GameObject directionY;
    public GameObject directionZ;

    // Use this for initialization
    void Start()
    {
        directionX.GetComponent<MeshRenderer>().material.color = Color.red;
        directionY.GetComponent<MeshRenderer>().material.color = Color.green;
        directionZ.GetComponent<MeshRenderer>().material.color = Color.cyan;
    }

    Vector3 lastPosition = Vector3.zero;

    // Update is called once per frame
    void Update()
    {

        if ((SystemManager.cameraState & SystemManager.CameraState.PERSPECTIVE) == SystemManager.CameraState.PERSPECTIVE)
        {
            gameObject.transform.localPosition = new Vector3(AutoDisplayAdjuster.Instance.BottomLeft().x + 5.5f, transform.localPosition.y, transform.localPosition.z);
        }
    }

}
