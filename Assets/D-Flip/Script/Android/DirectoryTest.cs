using UnityEngine;
using System.Collections;
using System.IO;

public class DirectoryTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath+"/Face");
        FileInfo[] files = dir.GetFiles("*.jpg");
        foreach (var f in files)
        {
            print(f.Name);
        }
    }
}
