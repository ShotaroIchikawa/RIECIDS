using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadSettingFlies : MonoBehaviour {

    private enum variables
    {
        end,
        scaleFactor,
        topWall,
        bottomWall,
        rightWall,
        leftWall
    }

    public void Load()
    {
        string dataPath = Application.dataPath + "/Settings/";
        List<List<string>> settings = new List<List<string>>();
        try
        {
            string[] data = Directory.GetFiles(dataPath, "*.txt", SearchOption.AllDirectories);
            CSVTagger tagger = GetComponent<CSVTagger>();
            foreach (string dataName in data)
            {
                string settingsText = File.ReadAllText(dataName);
                if (settingsText != null)
                {
                    settings.AddRange(tagger.CSVToList(settingsText));
                }
            }
            Setting(settings);
        }
        catch
        {
            //失敗したらデフォルトで適当に入れる
            GameObject.Find("Canvas").gameObject.GetComponent<Canvas>().scaleFactor = 1.2f;
            AutoDisplayAdjuster.topOffset = 0.6f;
            AutoDisplayAdjuster.bottomOffset = 0.4f;
            AutoDisplayAdjuster.leftOffset = 0.6f;
            AutoDisplayAdjuster.rightOffset = 0.4f;

            //指定したパスにフォルダが無ければフォルダとtxtファイルを作る
            if (Directory.Exists(Application.dataPath + "/Settings") == false)
            {
                CreateDirectoryAndFile();
            }
        }

    }

    void Setting(List<List<string>> settings)
    {
        foreach (List<string> a in settings)
        {
            switch (a[0])
            {
                case "scaleFactor":
                    GameObject.Find("Canvas").gameObject.GetComponent<Canvas>().scaleFactor = float.Parse(a[1]);
                    break;
                case "topWallOffset":
                    AutoDisplayAdjuster.topOffset = float.Parse(a[1]);
                    break;
                case "bottomWallOffset":
                    AutoDisplayAdjuster.bottomOffset = float.Parse(a[1]);
                    break;
                case "leftWallOffset":
                    AutoDisplayAdjuster.leftOffset = float.Parse(a[1]);
                    break;
                case "rightWallOffset":
                    AutoDisplayAdjuster.rightOffset = float.Parse(a[1]);
                    break;
                case "follower":
                    AttractorColor.photoNum = int.Parse(a[1]);
                    break;
                default:
                    break;
            }
        }
    }

    void CreateDirectoryAndFile()
    {
        DirectoryInfo di = Directory.CreateDirectory(Application.dataPath + "/Settings");
        File.Create(Application.dataPath + "/Settings/settings.txt");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
