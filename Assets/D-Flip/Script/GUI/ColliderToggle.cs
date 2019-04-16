using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderToggle : MonoBehaviour {


    public void OnToggleChange()
    {
        if ((SystemManager.attractorState & SystemManager.AttractorState.ADJUST_SCALE) == SystemManager.AttractorState.ADJUST_SCALE)
        {
            StartCoroutine(ColliderIsTrigger(this.GetComponent<Toggle>().isOn));
        }
        else
        {
            //エラー
        }
    }

    IEnumerator ColliderIsTrigger(bool active)
    {
        foreach (DflipPhoto a in PhotoManager.Instance.photos)
        {
            a.gameobject.GetComponent<BoxCollider>().isTrigger = active;
        }
        yield return null;
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
