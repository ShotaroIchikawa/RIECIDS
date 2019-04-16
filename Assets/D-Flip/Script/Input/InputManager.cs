using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    Camera camera;
    public GameObject debugText;
    public enum walls
    {
        TOP,
        BOTTOM,
        RIGHT,
        LEFT
    }

    public walls wall;
    public GameObject inputField;

    public List<ActivePhoto> activePhoto = new List<ActivePhoto>();
    SystemManager system;

    void Start()
    {
        system = GetComponent<SystemManager>();
        camera = GetComponent<Camera>();
        Instance = this;
        wall = walls.TOP;
    }

    public void AddActivePhoto(DflipPhoto photo, int id)
    {
        activePhoto.Add(new ActivePhoto(id, photo));
    }

    public void RemoveActivePhoto(int ID)
    {
        activePhoto.RemoveAll((p) => p.ID == ID);
    }

    public static bool settingMode = false;
    private bool isDoubleClickStart;
    private float doubleClickTime = 0;
    private bool isSingleClickStart;
    bool stop = false;
    // Update is called once per frame
    void Update()
    {
        #region 設定モードのオンオフ
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.D))
        {
            if (settingMode == false)
            {
                settingMode = true;
                debugText.SetActive(true);
            }
            else
            {
                settingMode = false;
                debugText.SetActive(false);
            }
        }
        #endregion

        #region 設定モード
        if (settingMode == true)
        {
            //一時停止
            if (Input.GetKeyUp(KeyCode.Space))
            {
                stop = stop ? false : true;
                Time.timeScale = stop ? 0 : 1;
            }

            #region keyword Weightの調整
            if (Input.GetKeyDown(KeyCode.K))
            {
                PhotoManager.Instance.color.weight_--;
            }

            #endregion

            #region カラーで寄ってくる写真の数
            if (Input.GetKeyDown(KeyCode.F))
            {
                AttractorColor.photoNum++;
                DebugText.texts.follower = AttractorColor.photoNum.ToString();
            }
            if (Input.GetKeyDown(KeyCode.U) && AttractorColor.photoNum > 0)
            {
                AttractorColor.photoNum--;
                DebugText.texts.follower = AttractorColor.photoNum.ToString();
            }
            #endregion

            #region ズームのON-OFF
            if (Input.GetKeyDown(KeyCode.Z))
            {
                DebugText.texts.doubleClick = SystemManager.zoomEnabled ? "OFF" : "ON";
                SystemManager.zoomEnabled = SystemManager.zoomEnabled ? false : true;
            }
            #endregion

            #region perspectiveでの画像へのタッチインタラクションのON-OFF
            if (Input.GetKeyDown(KeyCode.I))
            {
                DebugText.texts.interactive = SystemManager.interactiveEnabled ? "OFF" : "ON";
                SystemManager.interactiveEnabled = SystemManager.interactiveEnabled ? false : true;
                if (SystemManager.interactiveEnabled == false)
                {
                    inputField.SetActive(true);
                }
                else
                {
                    inputField.SetActive(false);
                }
            }

            #endregion

            #region canvas scaler のscaleFactor調整用
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameObject.Find("Canvas").gameObject.GetComponent<Canvas>().scaleFactor += 0.1f;
                DebugText.texts.scaleFactor = GameObject.Find("Canvas").gameObject.GetComponent<Canvas>().scaleFactor.ToString("0.0");
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                GameObject.Find("Canvas").gameObject.GetComponent<Canvas>().scaleFactor -= 0.1f;
                DebugText.texts.scaleFactor = GameObject.Find("Canvas").gameObject.GetComponent<Canvas>().scaleFactor.ToString("0.0");
            }
            #endregion

            #region wall
            if (Input.GetKeyDown(KeyCode.T))
            {
                wall = walls.TOP;
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                wall = walls.BOTTOM;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                wall = walls.RIGHT;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                wall = walls.LEFT;
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                switch (wall)
                {
                    case walls.TOP:
                        AutoDisplayAdjuster.topOffset += 0.1f;
                        DebugText.texts.topOffset = AutoDisplayAdjuster.topOffset.ToString("0.0");
                        break;
                    case walls.BOTTOM:
                        AutoDisplayAdjuster.bottomOffset += 0.1f;
                        DebugText.texts.bottomOffset = AutoDisplayAdjuster.bottomOffset.ToString("0.0");
                        break;
                    case walls.RIGHT:
                        AutoDisplayAdjuster.rightOffset += 0.1f;
                        DebugText.texts.rightOffset = AutoDisplayAdjuster.rightOffset.ToString("0.0");
                        break;
                    case walls.LEFT:
                        AutoDisplayAdjuster.leftOffset += 0.1f;
                        DebugText.texts.leftOffset = AutoDisplayAdjuster.leftOffset.ToString("0.0");
                        break;
                }

            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                switch (wall)
                {
                    case walls.TOP:
                        if (AutoDisplayAdjuster.topOffset > 0.1f)
                        {
                            AutoDisplayAdjuster.topOffset -= 0.1f;
                            DebugText.texts.topOffset = AutoDisplayAdjuster.topOffset.ToString("0.0");
                        }
                        break;
                    case walls.BOTTOM:
                        if (AutoDisplayAdjuster.bottomOffset > 0.1f)
                        {
                            AutoDisplayAdjuster.bottomOffset -= 0.1f;
                            DebugText.texts.bottomOffset = AutoDisplayAdjuster.bottomOffset.ToString("0.0");
                        }
                        break;
                    case walls.RIGHT:
                        if (AutoDisplayAdjuster.rightOffset > 0.1f)
                        {
                            AutoDisplayAdjuster.rightOffset -= 0.1f;
                            DebugText.texts.rightOffset = AutoDisplayAdjuster.rightOffset.ToString("0.0");
                        }
                        break;
                    case walls.LEFT:
                        if (AutoDisplayAdjuster.leftOffset > 0.1f)
                        {
                            AutoDisplayAdjuster.leftOffset -= 0.1f;
                            DebugText.texts.leftOffset = AutoDisplayAdjuster.leftOffset.ToString("0.0");
                        }
                        break;
                }

            }
            #endregion
        }
        #endregion

        if ((SystemManager.systemState & SystemManager.SystemState.MAIN) == SystemManager.SystemState.MAIN)
        {
      

            if ((SystemManager.cameraState & SystemManager.CameraState.ORTHOGRAPHIC) == SystemManager.CameraState.ORTHOGRAPHIC)
            {

            }
            else if ((SystemManager.cameraState & SystemManager.CameraState.PERSPECTIVE) == SystemManager.CameraState.PERSPECTIVE)
            {
                #region マウス
                if (SystemManager.interactiveEnabled == true)
                {
                    if (Input.GetMouseButtonDown(2))
                    {
                        last = Input.mousePosition;
                    }
                    if (Input.GetMouseButton(2))
                    {
                        CameraTransform(Input.mousePosition);
                    }
                }
                #endregion
            }
        }
    }

    void CameraTransform(Vector2 pos)
    {
        Vector2 ed = pos - (Vector2)last;
        float angle = ed.magnitude / cameraTransformWeight;
        if (ed.x > 5)
        {
            zCamera.transform.RotateAround(new Vector3(0, zCamera.transform.localPosition.y, 5), Vector3.up, angle);
        }
        else if (ed.x < -5)
        {
            zCamera.transform.RotateAround(new Vector3(0, zCamera.transform.localPosition.y, 5), Vector3.up, -angle);
        }

        if (ed.y > 5 && zCamera.transform.localPosition.y > -4)
        {
            zCamera.transform.localPosition += -angle * 0.1f * Vector3.up;
        }
        else if (ed.y < -5 && zCamera.transform.localPosition.y < 4)
        {
            zCamera.transform.localPosition += angle * 0.1f * Vector3.up;
        }
        last = pos;
    }

    public float finger_threshold_2;
    public float cameraTransformWeight;
    Vector3 last;
    public Camera zCamera;
}

public class ActivePhoto
{
    public int ID;
    public DflipPhoto photo;

    public ActivePhoto(int _ID, DflipPhoto _photo)
    {
        ID = _ID;
        photo = _photo;
    }
}
