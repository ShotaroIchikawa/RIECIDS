using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvoidChangeToggle : MonoBehaviour {

    float currentAvoid;
    public void OnValueChanged()
    {
        Toggle toggle = GetComponent<Toggle>();
        if(toggle.isOn == true)
        {
            currentAvoid = SystemManager.Instance.weight.AvoidWeight;
            SystemManager.Instance.weight.AvoidWeight = 0;
            SystemManager.Instance.AddAttractor(SystemManager.AttractorState.AVOID);
            StartCoroutine(StartAvoid());
        }
        else
        {
            SystemManager.Instance.RemoveAttractor(SystemManager.AttractorState.AVOID);
        }
    }

    IEnumerator StartAvoid()
    {
        for (float i = 0; i < currentAvoid * 10; i++) //0.1ずつ増やすので１０倍している
        {
            SystemManager.Instance.weight.AvoidWeight += 0.1f;
            yield return null;
        }
        print(currentAvoid);
        print(SystemManager.Instance.weight.AvoidWeight);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
