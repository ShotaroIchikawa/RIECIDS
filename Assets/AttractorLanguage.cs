using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorLanguage : MonoBehaviour, IAttractorSelection {

    public float weight_;
    int key = 0;

    public void select(AttractorWeight weight, List<DflipPhoto> photos, List<Stroke> strokes)
    {
        key = GetComponent<PhotoManager>().mouseAttractorKey2;
        foreach (DflipPhoto a in photos)
        {
            if (a.metaword[key].Contains("0"))
            {
                a.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
