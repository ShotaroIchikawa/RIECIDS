using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void OnScreenSizeChange(Vector2 newScreenSize);
public class AutoDisplayAdjuster : MonoBehaviour {

    public static AutoDisplayAdjuster Instance;
    public static bool screenChange;
    public static bool wallChange;
    public OnScreenSizeChange OnScreenSizeChanged;

    public Camera targetCamera;
    public GameObject LeftWall;
    public GameObject RightWall;
    public GameObject BottomWall;
    public GameObject TopWall;

    public Vector3 bottomLeft;
    public Vector3 topRight;
    public Vector2 NormalizedSize;

    float RightWallOffset;
    float LeftWallOffset;
    float BottomWallOffset;
    float TopWallOffset;

    public static float rightOffset;
    public static float leftOffset;
    public static float topOffset;
    public static float bottomOffset;

    public static float BottomWallLength;
    public static float TopWallLength;

    float lastRightWallOffset;
    float lastLeftWallOffset;
    float lastBottomWallOffset;
    float lastTopWallOffset;
    Vector2 lastScreenSize;
    float _cameraSize;

    public void Load()
    {
        Instance = this;
        _cameraSize = targetCamera.orthographicSize;
        OnScreenSizeChanged = new OnScreenSizeChange(AdjustWall);

        BottomWallLength = 2;
        TopWallLength = 2;

        AdjustWallOffset();
        UpdateLate();
        lastScreenSize = new Vector2(Screen.width, Screen.height);
        screenChange = false;
        wallChange = false;
        AdjustWall(lastScreenSize);
    }

    void Update()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        AdjustWallOffset();

        ChangeCheck(screenSize);
        
        //ウォール調整
        if (screenChange)
        {
            UpdateLate(screenSize);
            UpdateLate();
            if (OnScreenSizeChanged != null)
                OnScreenSizeChanged(screenSize);
        }
        else if (wallChange)
        {
            UpdateLate();
            if (OnScreenSizeChanged != null)
                OnScreenSizeChanged(screenSize);
        }
    }



    void ChangeCheck(Vector2 screenSize)
    {
        if ((SystemManager.cameraState & SystemManager.CameraState.ZOOM_OUT) == SystemManager.CameraState.ZOOM_OUT)
        {
            screenChange = lastScreenSize != screenSize ? true : false;

            if (lastRightWallOffset != RightWallOffset
                || lastLeftWallOffset != LeftWallOffset
                || lastBottomWallOffset != BottomWallOffset
                || lastTopWallOffset != TopWallOffset)
            {
                wallChange = true;
            }
            else
            {
                wallChange = false;
            }
        }  
    }

    void UpdateLate(Vector2 screenSize)
    {
        lastScreenSize = screenSize;     
    }

    void UpdateLate()
    {
        lastRightWallOffset = RightWallOffset;
        lastLeftWallOffset = LeftWallOffset;
        lastBottomWallOffset = BottomWallOffset;
        lastTopWallOffset = TopWallOffset;
    }

    void AdjustWall(Vector2 size)
    {
        bottomLeft = targetCamera.ScreenToWorldPoint(new Vector3(0, 0, 10));
        topRight = targetCamera.ScreenToWorldPoint(new Vector3(size.x, size.y, 10));

        //NormalizedSize = new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        NormalizedSize = new Vector2(TopRight().x - BottomLeft().x, TopRight().y - BottomLeft().y);


        LeftWall.transform.localPosition = new Vector3(bottomLeft.x - LeftWall.transform.localScale.x / 2 + LeftWallOffset, 0, -20f);
        RightWall.transform.localPosition = new Vector3(topRight.x + RightWall.transform.localScale.x / 2 - RightWallOffset, 0, -20f);
        BottomWall.transform.localPosition = new Vector3(0, bottomLeft.y - BottomWall.transform.localScale.y/2 + BottomWallOffset, -20f);
        BottomWall.transform.localScale = new Vector3(topRight.x*BottomWallLength, 1, 1);
        TopWall.transform.localPosition = new Vector3(0, topRight.y + TopWall.transform.localScale.y / 2 - TopWallOffset, -20f);
        TopWall.transform.localScale = new Vector3(topRight.x*TopWallLength, 1, 1);
    }

    //現在のアトラクター状態を確認してオフセットを調整
    void AdjustWallOffset()
    {
        TopWallOffset = ((SystemManager.attractorState & SystemManager.AttractorState.ADJUST_SCALE) == SystemManager.AttractorState.ADJUST_SCALE) ? topOffset + 0.4f : topOffset;
        BottomWallOffset = ((SystemManager.attractorState & SystemManager.AttractorState.HORIZONTAL_SORT) == SystemManager.AttractorState.HORIZONTAL_SORT) ? bottomOffset : 0;
        LeftWallOffset = ((SystemManager.attractorState & SystemManager.AttractorState.VERTICAL_SORT) == SystemManager.AttractorState.VERTICAL_SORT) ? leftOffset : 0;
        RightWallOffset = ((SystemManager.attractorState & SystemManager.AttractorState.DEPTH_SORT) == SystemManager.AttractorState.DEPTH_SORT) ? rightOffset : 0;

    }

    public Vector2 TopRight()
    {
        return new Vector2(RightWall.transform.localPosition.x - RightWall.transform.localScale.x / 2, TopWall.transform.localPosition.y - TopWall.transform.localScale.y / 2);
    }

    public Vector2 BottomLeft()
    {
        return new Vector2(LeftWall.transform.localPosition.x + LeftWall.transform.localScale.x / 2, BottomWall.transform.localPosition.y + BottomWall.transform.localScale.y / 2);
    }

    public Vector2 ScreenLR()
    {
        Vector3 topRight = targetCamera.ScreenToWorldPoint(new Vector3(new Vector2(Screen.width, Screen.height).x, new Vector2(Screen.width, Screen.height).y, 10));
        Vector3 bottomLeft = targetCamera.ScreenToWorldPoint(new Vector3(0, 0, 10));
        return new Vector2(bottomLeft.x, topRight.x);
    }
}
