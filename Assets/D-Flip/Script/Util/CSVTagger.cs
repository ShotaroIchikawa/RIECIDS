using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVTagger : MonoBehaviour {

    public List<List<string>> CSVToList(TextAsset csvFile)
    {
        StringReader reader = new StringReader(csvFile.text);
        List<List<string>> csvData = new List<List<string>>();
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            List<string> temp = new List<string>();
            temp.AddRange(line.Split(','));
            csvData.Add(temp);
        }
        return csvData;
    }

    public List<List<string>> CSVToList(string csvText)
    {
        StringReader reader = new StringReader(csvText);
        List<List<string>> csvData = new List<List<string>>();
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            List<string> temp = new List<string>();
            temp.AddRange(line.Split(','));
            csvData.Add(temp);
        }
        return csvData;
    }

    public void TaggingData(List<List<string>> csvData, DflipPhoto photo, string filename)
    {
        for (int i = 0; i < csvData.Count; i++)
        {
            if (csvData[i][0] == filename) //ファイル名に応じてデータをタグ付け
            {
                for (int j = 1; j < csvData[i].Count; ++j)
                {
                    if (photo.metadata.ContainsKey(PhotoManager.Instance.keydataCode[csvData[0][j]]) == false)
                    {
                        //metadataに登録されていなければ登録する
                        if (csvData[0][j].Contains("[Y:M]")) //年月
                        {
                            photo.metadata.Add(PhotoManager.Instance.keydataCode[csvData[0][j]], PhotoManager.Instance.SerializeYM(csvData[i][j]));
                        }
                        else
                        {
                            photo.metadata.Add(PhotoManager.Instance.keydataCode[csvData[0][j]], float.Parse(csvData[i][j]));
                        }
                    }
                    else
                    {
                        //値の更新
                        if (csvData[0][j].Contains("[Y:M]")) //年月
                        {                        
                            photo.metadata[PhotoManager.Instance.keydataCode[csvData[0][j]]] = PhotoManager.Instance.SerializeYM(csvData[i][j]);
                        }
                        else
                        {
                            photo.metadata[PhotoManager.Instance.keydataCode[csvData[0][j]]] = float.Parse(csvData[i][j]);
                        }
                    }
                }
            }
        }
    }

    public void TaggingWord(List<List<string>> csvWord, DflipPhoto photo, string filename)
    {
        for (int i = 0; i < csvWord.Count; i++)
        {
            if (csvWord[i][0] == filename) //ファイル名に応じてデータをタグ付け
            {
                for (int j = 0; j < csvWord[i].Count; ++j)
                {
                    if (csvWord[i][j] != "")
                    {
                        if (photo.metaword.ContainsKey(PhotoManager.Instance.keywordCode[csvWord[0][j]]) == false)
                        {
                            //metawordに登録されていなければ登録する
                            photo.metaword.Add(PhotoManager.Instance.keywordCode[csvWord[0][j]], new List<string>());
                            photo.metaword[PhotoManager.Instance.keywordCode[csvWord[0][j]]].Add(csvWord[i][j]);
                        }
                        else
                        {
                            //既に登録されているならば追加する
                            photo.metaword[PhotoManager.Instance.keywordCode[csvWord[0][j]]].Add(csvWord[i][j]);
                        }
                    }
                }
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
