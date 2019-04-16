using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.UI;
using System.IO;

public class FileOpenDialog : MonoBehaviour {

    public GameObject FileMenu;

    private List<string> GetFiles(string filePath)
    {
        DirectoryInfo files = new DirectoryInfo(filePath);
        FileInfo[] allFileName = files.GetFiles("*.*");
        string fileName;
        List<string> list = new List<string>();

        for (int i = 0; i < allFileName.Length; i++)
        {
            //read file name (Tolower)
            fileName = allFileName[i].Name;
            if (fileName.EndsWith(".png"))
            {
                fileName = Application.dataPath + "/StreamingAssets/Photo/" + fileName;
                list.Add(fileName);
            }
        }
        return list;
    }

    public void OnButtonClick()
    {
        if ((SystemManager.systemState & SystemManager.SystemState.MAIN) == SystemManager.SystemState.MAIN)
        {
            var path = Application.dataPath + "/StreamingAssets/Photo";

            List<string> filePaths = new List<string>();
            filePaths = GetFiles(path);

            var wordPath = Application.dataPath + "/StreamingAssets/Photo/keyword.txt";
            filePaths.Add(wordPath);

            if (filePaths != null)
            {
                List<string> files = new List<string>();
                List<string> data = new List<string>();
                foreach (string a in filePaths)
                {
                    switch (a.Split('.')[1]) // Show the type of file
                    {
                        case "txt":
                            data.Add(a);
                            break;
                        case "png":
                            files.Add(a);
                            break;
                        default:
                            break;
                    }
                }
                Debug.Log(files[1]);
                PhotoCreator photoCreator = GameObject.Find("Main Camera").gameObject.GetComponent<PhotoCreator>();
                if (PhotoManager.LOADED == false)
                {
                    if (files.Count > 0 && data.Count > 0) //画像とデータを同時に
                    {
                        photoCreator.ImageAndDataCreate(files, data);
                        PhotoManager.LOADED = true;
                    }
                    else
                    {
                        if (files.Count > 0 && data.Count < 1) //画像のみ
                        {
                            photoCreator.ImageCreate(files);
                            PhotoManager.LOADED = true;
                        }
                    }
                }
                else
                {
                    bool resetChecker = false;
                    if (files.Count > 0 || data.Count > 0)
                    {
                        resetChecker = GameObject.Find("Main Camera").gameObject.GetComponent<GUIManager>().AllReset();
                    }

                    PhotoManager pm = GameObject.Find("Main Camera").gameObject.GetComponent<PhotoManager>();
                    if (resetChecker == true)
                    {
                        if (files.Count > 0 && data.Count > 0) //画像とデータを同時に
                        {
                            ClearOldPhotos(pm);
                            photoCreator.ImageAndDataCreate(files, data);
                        }
                        else if (files.Count > 0 && data.Count < 1) //画像のみ
                        {
                            ClearOldPhotos(pm);
                            photoCreator.ImageCreate(files);
                        }
                    }
                    if (data.Count > 0 && files.Count < 1) //データのみ
                    {
                        photoCreator.DataCreate(data);
                    }
                }
            }
        }
        FileMenu.GetComponent<OpenMenu>().OnButtonClick();
    }

    void ClearOldPhotos(PhotoManager pm)
    {
        pm.AttractorDisable();
        for (int i = pm.photos.Count - 1; i > -1; i--)
        {
            Destroy(pm.photos[i].sprite.gameObject);
            Destroy(pm.photos[i].gameObject);
        }
        pm.photos.Clear();
        pm.keydataCode.Clear();
        pm.keywordCode.Clear();
        PhotoManager.minValue.Clear();
        PhotoManager.maxValue.Clear();
        PhotoManager.midValue.Clear();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
