using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System;
using UnityEngine.UI;
using UnityEngine.Video;


public class PhotoCreator : MonoBehaviour
{

    //普通にUnity上に画像を読み込んで処理するより，バイナリデータとして読み込んだほうがモバイルとか考えたときいいような気がしてこうしました．

    public PhysicMaterial pm;
    public float photoMargin;
    PhotoManager photoManager;
    public Material spriteMaterial;
    public GameObject message;
    public static bool last = false;

    #region Data Load
    public void DataCreate(List<string> data)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        StartCoroutine(ListAllData(data));
        sw.Stop();
        print("data : " + sw.ElapsedMilliseconds);
    }

    IEnumerator ListAllData(List<string> data)
    {
        #region 前処理：アトラクターの一時停止とシステム状態の遷移
        photoManager.AttractorDisable();
        if ((SystemManager.systemState & SystemManager.SystemState.MAIN) == SystemManager.SystemState.MAIN)
        {
            SystemManager.systemState = ~SystemManager.SystemState.MAIN & SystemManager.systemState;
            SystemManager.systemState |= SystemManager.SystemState.LOAD;
        }
        #endregion

        CSVTagger tagger = GetComponent<CSVTagger>();
        List<List<string>> csvData = new List<List<string>>();
        List<List<string>> csvWord = new List<List<string>>();
        foreach (string dataName in data)
        {
            string csvText = File.ReadAllText(dataName);
            if (csvText != null)
            {
                string[] metadataPath = dataName.Split('/');

                #region keyCodeの生成とcsvのリスト化
                if (metadataPath[metadataPath.Length - 1].Contains("data"))
                {
                    csvData.AddRange(tagger.CSVToList(csvText));
                    photoManager.CreateKeydataCode(csvData);
                    photoManager.CreateMaxMinCode(csvData);
                }
                else if (metadataPath[metadataPath.Length - 1].Contains("keyword"))
                {
                    csvWord.AddRange(tagger.CSVToList(csvText));
                    photoManager.CreateKeywordCode(csvWord);
                }
                #endregion
            }
        }

        //int counter = (int)(1 / Time.deltaTime);
        #region タグ付け
        foreach (DflipPhoto a in photoManager.photos)
        {
            string filename = a.fileName;
            if (csvData != null)
            {
                tagger.TaggingData(csvData, a, filename);
            }
            if (csvWord != null)
            {
                tagger.TaggingWord(csvWord, a, filename);
            }

            //if (counter < 1)
            //{
            //    text.text = "Now Loading . . .";
            //    counter = (int)((1/2) * (1 / Time.deltaTime));
            //}
            //else if (counter > 1 && counter < (int)((1/6)  * (1/ Time.deltaTime)))
            //{
            //    text.text = "Now Loading .";
            //}
            //else if (counter > (int)((1 / 6) * (1 / Time.deltaTime)) && counter < (int)((1 / 3) * (1 / Time.deltaTime)))
            //{
            //    text.text = "Now Loading . .";
            //}

            //counter--;
            yield return null;
        }
        #endregion

        #region 後処理
        GetComponent<GUIManager>().CreateButtons();
        GetComponent<GUIManager>().GUIInteractable();
        #region exifに関しては全画像についてexifデータが揃っていなければinteractableをオフにする
        if (checker != photoManager.photos.Count)
        {
            gameObject.GetComponent<GUIManager>().Deinteractable("撮影年月");
        }
        else
        {
            photoManager.CreateMaxMinCode("撮影年月");
        }
        #endregion
        SystemManager.systemState = ~SystemManager.SystemState.LOAD & SystemManager.systemState;
        SystemManager.systemState |= SystemManager.SystemState.MAIN;
        photoManager.AttractorEnable();
        #endregion
    }
    #endregion

    #region Image Load
    public void ImageCreate(List<string> files)
    {
        if (photoManager == null)
        {
            photoManager = GetComponent<PhotoManager>();
        }
        Stopwatch sw = new Stopwatch();
        sw.Start();
        StartCoroutine(ListAllImages(files));
        sw.Stop();
        print("image : " + sw.ElapsedMilliseconds);
    }

    IEnumerator ListAllImages(List<string> files)
    {
        #region 前処理：アトラクターの一時停止とシステム状態の遷移
        photoManager.AttractorDisable();
        if ((SystemManager.systemState & SystemManager.SystemState.MAIN) == SystemManager.SystemState.MAIN)
        {
            SystemManager.systemState = ~SystemManager.SystemState.MAIN & SystemManager.systemState;
            SystemManager.systemState |= SystemManager.SystemState.LOAD;
        }
        #endregion

        #region デフォルトのメタデータのキーコードを生成
        photoManager.CreateKeydataCode("輝度");
        photoManager.CreateKeydataCode("彩度");
        photoManager.CreateKeydataCode("撮影年月");
        photoManager.CreateKeywordCode("カラー");
        #endregion

        //ファイルを均等な配列で生成
        int column = (int)Mathf.Sqrt(files.Count) + 1;
        var span_x = (AutoDisplayAdjuster.Instance.NormalizedSize.x) / (column + 1);
        var span_y = (AutoDisplayAdjuster.Instance.NormalizedSize.y) / (column + 1);
        Vector3 createPosition = new Vector3(AutoDisplayAdjuster.Instance.BottomLeft().x + photoMargin + span_x, AutoDisplayAdjuster.Instance.TopRight().y, 0);

        for (var i = 0; i < files.Count; i++)
        {
            GameObject photo = new GameObject() as GameObject;
            GameObject sprite = new GameObject() as GameObject;            
            var sp = sprite.AddComponent<SpriteRenderer>();
            sp.material = Instantiate(spriteMaterial);
            sp.material.SetFloat("_Offset", 0.1f);
            
            var _p = photo.AddComponent<DflipPhoto>();
            var _s = sprite.AddComponent<PhotoSprite>();
            
            _p.sprite = _s;
            _p.ID = i;
            string[] temp = files[i].Split('/');
            _p.fileName = temp[temp.Length - 1].Split('.')[0];

            //画像ロード
            if (i >= files.Count - 1)
            {
                last = true;
            }
            Bitmap bit = TaggingFromExif(files[i], _p);
            _s.Load(files[i], photo, bit);

            #region D-FLIPアルゴリズムの衝突判定用コライダ
            var boxCollider = photo.AddComponent<BoxCollider>();
            boxCollider.material = pm;
            boxCollider.size = new Vector3(sprite.GetComponent<SpriteRenderer>().bounds.size.x, sprite.GetComponent<SpriteRenderer>().bounds.size.y, 0) + 0.3f * Vector3.one;
            boxCollider.isTrigger = true;
            Rigidbody rigid = photo.AddComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.FreezePositionZ;
            rigid.freezeRotation = true;
            rigid.useGravity = false;
            rigid.drag = 1f;
            #endregion

            #region タッチ用コライダ
            var boxCollider_s = sprite.AddComponent<BoxCollider>();
            boxCollider_s.material = pm;
            boxCollider_s.size = sprite.GetComponent<SpriteRenderer>().bounds.size;
            boxCollider_s.isTrigger = true;
            Rigidbody rigid_s = sprite.AddComponent<Rigidbody>();
            rigid_s.constraints = RigidbodyConstraints.FreezePositionZ;
            rigid_s.freezeRotation = true;
            rigid_s.useGravity = false;
            rigid_s.drag = 1f;
            sprite.AddComponent<PhotoInteraction>();
            #endregion

            #region 初期位置＆スケール
            photo.transform.localPosition = createPosition + PhotoManager.offset;
            sprite.transform.localPosition = createPosition;
            sp.sortingOrder = 1;
            photo.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
            sprite.transform.localScale = photo.transform.localScale;
            #endregion

            photoManager.AddPhoto(_p);

            #region CreatePositionの調整
            if ((i + 1) % column == 0)
            {
                createPosition = new Vector3(AutoDisplayAdjuster.Instance.BottomLeft().x + photoMargin, createPosition.y, createPosition.z);
                createPosition -= new Vector3(0, span_y, 0);
            }
            createPosition += new Vector3(span_x, 0, 0);
            #endregion

            yield return null;
        }

        #region 後処理
        GetComponent<GUIManager>().CreateButtons();
        GetComponent<GUIManager>().GUIInteractable();
        #region exifに関しては全画像についてexifデータが揃っていなければinteractableをオフにする
        if (checker != photoManager.photos.Count)
        {
            gameObject.GetComponent<GUIManager>().Deinteractable("撮影年月");
        }
        else
        {
            photoManager.CreateMaxMinCode("撮影年月");
        }
        #endregion
        SystemManager.systemState = ~SystemManager.SystemState.LOAD & SystemManager.systemState;
        SystemManager.systemState |= SystemManager.SystemState.MAIN;
        photoManager.AttractorEnable();
        #endregion
    }

    #endregion

    #region Image & Data Load
    public void ImageAndDataCreate(List<string> files, List<string> data)
    {
        if (photoManager == null)
        {
            photoManager = GetComponent<PhotoManager>();
        }
        Stopwatch sw = new Stopwatch();
        sw.Start();
        StartCoroutine(ListAllFiles(files, data));
        sw.Stop();
        print("image & data : " + sw.ElapsedMilliseconds);
    }

    IEnumerator ListAllFiles(List<string> files, List<string> data)
    {
        #region 前処理：アトラクターの一時停止とシステム状態の遷移
        photoManager.AttractorDisable();
        if ((SystemManager.systemState & SystemManager.SystemState.MAIN) == SystemManager.SystemState.MAIN)
        {
            SystemManager.systemState = ~SystemManager.SystemState.MAIN & SystemManager.systemState;
            SystemManager.systemState |= SystemManager.SystemState.LOAD;
        }
        #endregion

        #region デフォルトのメタデータのキーコードを生成
        photoManager.CreateKeydataCode("輝度");
        photoManager.CreateKeydataCode("彩度");
        photoManager.CreateKeydataCode("撮影年月");
        photoManager.CreateKeywordCode("カラー");
        #endregion

        #region keyCodeの生成とcsvのリスト化
        CSVTagger tagger = GetComponent<CSVTagger>();
        List<List<string>> csvData = new List<List<string>>();
        List<List<string>> csvWord = new List<List<string>>();
        foreach (string dataName in data)
        {
            string csvText = File.ReadAllText(dataName);
            if (csvText != null)
            {
                string[] metadataPath = dataName.Split('/');

                if (metadataPath[metadataPath.Length - 1].Contains("data"))
                {
                    csvData.AddRange(tagger.CSVToList(csvText));
                    photoManager.CreateKeydataCode(csvData);
                    photoManager.CreateMaxMinCode(csvData);
                }
                else if (metadataPath[metadataPath.Length - 1].Contains("keyword"))
                {
                    csvWord.AddRange(tagger.CSVToList(csvText));
                    photoManager.CreateKeywordCode(csvWord);
                }
            }
        }
        #endregion

        //ファイルを均等な配列で生成
        int column = (int)Mathf.Sqrt(files.Count) + 1;
        var span_x = (AutoDisplayAdjuster.Instance.NormalizedSize.x) / (column + 1);
        var span_y = (AutoDisplayAdjuster.Instance.NormalizedSize.y) / (column + 1);
        Vector3 createPosition = new Vector3(AutoDisplayAdjuster.Instance.BottomLeft().x + photoMargin + span_x, AutoDisplayAdjuster.Instance.TopRight().y, 0);
        for (var i = 0; i < files.Count; i++)
        {
            GameObject photo = new GameObject() as GameObject;
            GameObject sprite = new GameObject() as GameObject;
            var sp = sprite.AddComponent<SpriteRenderer>();
            sp.material = Instantiate(spriteMaterial);
            sp.material.SetFloat("_Offset", 0.1f);

            var _p = photo.AddComponent<DflipPhoto>();
            var _s = sprite.AddComponent<PhotoSprite>();

            _p.sprite = _s;
            _p.ID = i;
            string[] temp = files[i].Split('/');
            _p.fileName = temp[temp.Length - 1].Split('.')[0];

            #region タグ付け
            string filename = _p.fileName;
            if (csvData != null)
            {
                tagger.TaggingData(csvData, _p, filename);
            }
            if (csvWord != null)
            {
                tagger.TaggingWord(csvWord, _p, filename);
            }
            #endregion

            //画像ロード
            if (i >= files.Count-1)
            {
                last = true;
            }
            Bitmap bit = TaggingFromExif(files[i], _p);
            _s.Load(files[i], photo, bit);

            #region D-FLIPアルゴリズムの衝突判定用コライダ
            var boxCollider = photo.AddComponent<BoxCollider>();
            boxCollider.material = pm;
            boxCollider.size = new Vector3(sprite.GetComponent<SpriteRenderer>().bounds.size.x, sprite.GetComponent<SpriteRenderer>().bounds.size.y, 0) + 0.3f * Vector3.one;
            boxCollider.isTrigger = true;
            Rigidbody rigid = photo.AddComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.FreezePositionZ;
            rigid.freezeRotation = true;
            rigid.useGravity = false;
            rigid.drag = 1f;
            #endregion

            #region タッチ用コライダ
            var boxCollider_s = sprite.AddComponent<BoxCollider>();
            boxCollider_s.material = pm;
            boxCollider_s.size = sprite.GetComponent<SpriteRenderer>().bounds.size;
            boxCollider_s.isTrigger = true;
            Rigidbody rigid_s = sprite.AddComponent<Rigidbody>();
            rigid_s.constraints = RigidbodyConstraints.FreezePositionZ;
            rigid_s.freezeRotation = true;
            rigid_s.useGravity = false;
            rigid_s.drag = 1f;
            sprite.AddComponent<PhotoInteraction>();
            #endregion

            #region 初期位置＆スケール
            photo.transform.localPosition = createPosition + PhotoManager.offset;
            sprite.transform.localPosition = createPosition;
            sp.sortingOrder = 1;
            photo.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
            sprite.transform.localScale = photo.transform.localScale;
            #endregion

            photoManager.AddPhoto(_p);

            #region CreatePositionの調整
            if ((i + 1) % column == 0)
            {
                createPosition = new Vector3(AutoDisplayAdjuster.Instance.BottomLeft().x + photoMargin, createPosition.y, createPosition.z);
                createPosition -= new Vector3(0, span_y, 0);
            }
            createPosition += new Vector3(span_x, 0, 0);
            #endregion

            yield return null;
        }


        #region 後処理
        GetComponent<GUIManager>().CreateButtons();
        GetComponent<GUIManager>().GUIInteractable();
        #region exifに関しては全画像についてexifデータが揃っていなければinteractableをオフにする
        if (checker != photoManager.photos.Count)
        {
            gameObject.GetComponent<GUIManager>().Deinteractable("撮影年月");
        }
        else
        {
            photoManager.CreateMaxMinCode("撮影年月");
        }
        #endregion
        SystemManager.systemState = ~SystemManager.SystemState.LOAD & SystemManager.systemState;
        SystemManager.systemState |= SystemManager.SystemState.MAIN;
        photoManager.AttractorEnable();
        #endregion
    }

    #endregion

    int checker;
    Bitmap TaggingFromExif(string path, DflipPhoto photo) //bitmapを返しつつExifをチェック
    {
        Bitmap bit = new Bitmap(@path);
        if (bit.PropertyItems != null)
        {
            for (int i = 0; i < bit.PropertyItems.Length; i++)
            {
                if (bit.PropertyItems[i].Type == 2 && bit.PropertyItems[i].Id == 36867) //IDがexifのタグ
                {
                    string str = System.Text.Encoding.ASCII.GetString(bit.PropertyItems[i].Value);
                    str = str.Trim(new char[] { '\0' });
                    string[] strs = str.Split(':');
                    float shotTime = (DateTime.Parse(strs[0] + "/" + strs[1])).ToBinary() / 10000000 / 60 / 60 / 24; //日単位でシリアル値化
                    photo.metadata.Add(photoManager.keydataCode["撮影年月"], shotTime);
                    checker++;
                }
            }
        }
        return bit;
    }

    // Update is called once per frame
    void Update()
    {

    }
}