using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorScaleUpMouse : MonoBehaviour, IAttractorSelection {

    public float weight_ = 1;
    public float ClickedPhotoSize = 1f;
    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {
        //weight_ = weight.ScaleUpMouseWeight;

        foreach (DflipPhoto a in photos)
        {
            float ds = 0;

            if(a.isClicked == true)
            {
                ds += (ClickedPhotoSize - a.Size) * 0.01f * weight_;
            }

            a.AddScale(ds);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
