using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class PhotoInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler{

    Camera camera;
    DflipPhoto photo;
    Sprite tem_sprite;
    int ID;
    int key = 0;
    string language = "";
    bool dcStart = false;
    bool dcNext = false;
    float dcTime=0;
    int videoTime = 0;
    Texture2D tem_image;
    bool videoCheck = false;
    bool nextClickCheck = false;

    public void OnPointerClick(PointerEventData ed)
    {
        if (ed.button == PointerEventData.InputButton.Left && interactive == true)
        {
            if (SystemManager.zoomEnabled == true)
            {
                if (dcStart == true)
                {
                    if (dcTime < 0.4f)
                    {
                        dcStart = false;
                        dcTime = 0;
                        //ダブルクリック処理
                        if ((SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
                        {
                            if ((SystemManager.cameraState & SystemManager.CameraState.ZOOM_OUT) == SystemManager.CameraState.ZOOM_OUT)
                            {
                                StartCoroutine(ZoomIn(ed.position));
                            }
                            else
                            {
                                StartCoroutine(ZoomOut());

                            }
                        }
                    }
                }
                else
                {
                    dcStart = true;
                }
            }
        }     
    }

    public void OnPointerDown(PointerEventData ed)
    {
        if (ed.button == 0)
        {
            ID = ed.pointerId;
            InputManager.Instance.AddActivePhoto(photo, ed.pointerId);
            photo.sprite.gameObject.GetComponent<SpriteRenderer>().sortingOrder += 1;
            mousePosition = ed.position;

            foreach (DflipPhoto a in PhotoManager.Instance.photos)
            {
                if (a.isClicked)
                {
                    if (a.sprite.GetComponent<PhotoInteraction>().videoCheck)
                    {
                        a.sprite.GetComponent<PhotoInteraction>().StopVideo();
                    }
                    a.isClicked = false;
                }
            }

            photo.isClicked = true;
            nextClickCheck = true;
            SystemManager.Instance.weight = new AttractorWeight(3.5f, 0, 0, 0, 0, 0, 0, 100, 0);
            key = PhotoManager.Instance.mouseAttractorKey2;

            //Dictionary<string, int> tempCode = PhotoManager.Instance.keywordCode;
            //foreach (KeyValuePair<string, int> kv in tempCode)
            //{
            //    Debug.Log(kv.Key + "," + kv.Value);
            //}

            PlayVideo();           
        }
    }

    public void PlayVideo()
    {
        if (key == 8)
        {
            language = "jp";
        }
        else if (key == 16)
        {
            language = "en";
        }
        //Debug.Log(language);
        var path = Application.dataPath + "/StreamingAssets/Video_original_ogv/" + photo.fileName + "_" + language + ".ogv";//link of video
        //var path = Application.dataPath + "/StreamingAssets/Video/" + photo.fileName + "_jp" + ".wmv";//link of video
        if (System.IO.File.Exists(path))//check whether video exist or not
        {
            videoCheck = true;
            // Initialize videoplayer
            var vPlayer = Camera.main.GetComponent<VideoPlayer>();
            var aSource = Camera.main.GetComponent<AudioSource>();
            vPlayer.playOnAwake = false;
            aSource.playOnAwake = false;
            vPlayer.source = VideoSource.Url;
            vPlayer.url = path;
            vPlayer.renderMode = VideoRenderMode.MaterialOverride;           

            //Debug.Log(vPlayer.frameCount);
            //Debug.Log(vPlayer.frameRate);
            //videoTime = (int)time;

            vPlayer.targetMaterialRenderer = photo.sprite.gameObject.GetComponent<SpriteRenderer>();
            vPlayer.targetMaterialProperty = "_MainTex";

            vPlayer.Play();

            //Debug.Log(videoTime);

            tem_image = photo.sprite.image; // Save the old image

            ////Replace the video by the image when stop playing
            UnityEngine.Rect rec = new UnityEngine.Rect(0, 0, tem_image.width, tem_image.height);
            tem_sprite = Sprite.Create(tem_image, rec, new Vector2(0.5f, 0.5f), 100);
        }
    }

    public void StopVideo()
    {
        if (videoCheck)//Check whether this image have played video before
        {
            var vPlayer = Camera.main.GetComponent<VideoPlayer>();

            vPlayer.targetMaterialRenderer = null;

            //Replace the video by the image when stop playing
            //UnityEngine.Rect rec = new UnityEngine.Rect(0, 0, tem_image.width, tem_image.height);
            //Sprite sprite = Sprite.Create(tem_image, rec, new Vector2(0.5f, 0.5f), 100);
            photo.sprite.gameObject.GetComponent<SpriteRenderer>().sprite = tem_sprite;

            videoCheck = false;
            vPlayer.Stop();
        }
    }

    public void OnPointerUp(PointerEventData ed)
    {
        if (ed.button == 0)
        {
            InputManager.Instance.RemoveActivePhoto(ed.pointerId);
            nextClickCheck = false;
            if(videoCheck)
            {
                Invoke("InvokeResetSortingOrder", 25); //time that image zooms in 
            }
            //else
            //{
            //    Invoke("InvokeResetSortingOrder", 2); 
            //}
        }
    }

    public void OnDrag(PointerEventData ed)
    {
        if ((SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
        {
            if (ID == ed.pointerId)
            {
                mousePosition = ed.position;
                photo.gameobject.transform.localPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
                photo.gameObject.transform.localPosition = new Vector3(photo.gameObject.transform.localPosition.x, photo.gameObject.transform.localPosition.y, -20f);
            }
        }

    }

    private void InvokeResetSortingOrder()
    {
        if (nextClickCheck == false)
        {
            photo.sprite.gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 1;
            StopVideo();
            photo.isClicked = false;
        }
    }

    bool zoomLevel;
    bool interactive;
    IEnumerator ZoomIn(Vector2 pos)
    {
        interactive = false;
        Vector3 v = Camera.main.ScreenToWorldPoint(pos) - camera.transform.localPosition;
        v.z = 0;
        while (camera.orthographicSize > 2.5f)
        {
            camera.transform.localPosition += v / 50;

            camera.orthographicSize -= 2.5f / 50;
            yield return null;
        }
        SystemManager.cameraState = ~SystemManager.CameraState.ZOOM_OUT & SystemManager.cameraState;
        SystemManager.cameraState |= SystemManager.CameraState.ZOOM_IN;
        interactive = true;
    }

    IEnumerator ZoomOut()
    {
        interactive = false;
        Vector3 v = Vector3.zero + -10 * Vector3.forward - camera.transform.localPosition;
        while (camera.orthographicSize < 5)
        {
            camera.transform.localPosition += v / 50;
            camera.orthographicSize += 2.5f / 50;
            yield return null;
        }
        SystemManager.cameraState = ~SystemManager.CameraState.ZOOM_IN & SystemManager.cameraState;
        SystemManager.cameraState |= SystemManager.CameraState.ZOOM_OUT;
        interactive = true;
    }

    void Awake()
    {
        photo = gameObject.GetComponent<PhotoSprite>().myPhoto;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Use this for initialization
    void Start () {
        interactive = true;
	}

    bool mouseStop;
    Vector2 mousePosition = Vector2.zero;
    Vector2 lastPosition = Vector2.one;
	// Update is called once per frame
	void Update () {
        if(photo.isClicked == true)
        {
            mouseStop = (mousePosition == lastPosition) ? true : false;
            if (mouseStop == true)
            {
                if ((SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
                {
                    photo.gameobject.transform.localPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
                    photo.gameObject.transform.localPosition = new Vector3(photo.gameObject.transform.localPosition.x, photo.gameObject.transform.localPosition.y, -20f);
                }
            }
            lastPosition = mousePosition;
        }

        if (dcStart == true)
        {
            dcTime += Time.deltaTime;
        }
        if (dcTime >= 0.4f)
        {
            dcStart = false;
            dcTime = 0;
        }        
    }
}
