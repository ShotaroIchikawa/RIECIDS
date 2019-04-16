using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteSettingFiles : MonoBehaviour {

    bool saveFinished = false;
    void OnApplicationQuit()
    {
        if (saveFinished == false)
        {
            SystemManager.systemState = SystemManager.systemState & ~SystemManager.SystemState.MAIN;
            SystemManager.systemState |= SystemManager.SystemState.FINALIZE;
            Application.CancelQuit();
            Save();
        }
    }

    void Save()
    {


        //上書き処理
        StreamWriter sw = new StreamWriter(Application.dataPath + "/Settings/settings.txt", false, System.Text.Encoding.GetEncoding("utf_8"));
        try
        {
            sw.WriteLine("名称,値");
            sw.WriteLine("scaleFactor," + DebugText.texts.scaleFactor);
            sw.WriteLine("topWallOffset," + AutoDisplayAdjuster.topOffset.ToString());
            sw.WriteLine("bottomWallOffset," + AutoDisplayAdjuster.bottomOffset.ToString());
            sw.WriteLine("leftWallOffset," + AutoDisplayAdjuster.leftOffset.ToString());
            sw.WriteLine("rightWallOffset," + AutoDisplayAdjuster.rightOffset.ToString());
            sw.WriteLine("follower," + AttractorColor.photoNum.ToString());
        }
        finally
        {    
            sw.Flush();
            sw.Close();
            saveFinished = true;
            Application.Quit();
        }
    }


	// Use this for initialization

	void Start () {
        saveFinished = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
