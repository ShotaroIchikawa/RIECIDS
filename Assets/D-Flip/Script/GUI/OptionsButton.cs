using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : MonoBehaviour {
    public GameObject options;

    public void OnButtonChange()
    {
        if(options.activeSelf == false)
        {
            options.SetActive(true);
        }
        else
        {
            options.SetActive(false);
        }
    }
}
