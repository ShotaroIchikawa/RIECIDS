using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour {

    public static SystemManager Instance;
    //システム状態
    public enum SystemState
    {
        LOAD            = 0x001,
        MAIN            = 0x002,
        FINALIZE        = 0x004
    }
    //ポインティングデバイス状態
    public enum PointingState
    {
        POINTING        = 0x001,
        VENN            = 0x002
    }
    //アトラクター状態
    public enum AttractorState
    {
        AVOID           = 0x001,
        AVOID_SCALE     = 0x002,
        SCALE_UP        = 0x004,
        VERTICAL_SORT   = 0x008,
        HORIZONTAL_SORT = 0x010,
        DEPTH_SORT      = 0x020,
        FRAME           = 0x040,
        SCALE_UP_MOUSE  = 0x080,
        ADJUST_SCALE    = 0x100,
        GEOGRAPH        = 0x200,
        COLOR           = 0x400,
        KEYWORD         = 0x800,
        LANGUAGE        = 0x1000
    }
    //カメラ状態
    public enum CameraState
    {
        PERSPECTIVE     = 0x001,
        ORTHOGRAPHIC    = 0x002,
        ZOOM_IN         = 0x004,
        ZOOM_OUT        = 0x008
    }
    //その他状態
    public static bool zAxisDirection; //画像枠線の白→黒方向の状態
    public static bool zoomEnabled; //ズームのONOFF用
    public static bool interactiveEnabled; //perspectiveで写真に触れるようにする用
    public static bool widthBar; //X軸幅変更用




    public void AddAttractor(AttractorState state)
    {
        if ((attractorState & state) != state)
        {
            attractorState |= state;
        }
    }

    public void RemoveAttractor(AttractorState state)
    {
        if((attractorState & state) == state)
        {
            attractorState = attractorState & ~state;
        }
    }

    public static string[] keys = new string[4] {"", "", "", ""}; //0:X, 1:Y, 2:Z, 3:S　のkeyを保持しておく
    public static SystemState    systemState;
    public static PointingState     pointingState;
    public static AttractorState attractorState;
    public static AttractorState lastattractorState;
    public static CameraState cameraState;
    public AttractorWeight weight;
	// Use this for initialization
	void Start () {
        Instance = this;
        weight = new AttractorWeight(3.5f,0,0,0,0,0,0,100,0);

        systemState |= SystemState.LOAD;

        #region システム
        if ((systemState & SystemState.LOAD) == SystemState.LOAD)
        {
            //カメラ状態設定
            cameraState |= CameraState.ORTHOGRAPHIC | CameraState.ZOOM_OUT;
            zoomEnabled = false;
            interactiveEnabled = false;

            //初期設定読み込み
            LoadSettingFlies lsf = GetComponent<LoadSettingFlies>();
            lsf.Load();

            //Wall設定
            AutoDisplayAdjuster wall = GetComponent<AutoDisplayAdjuster>();
            wall.Load();

            //GUI処理
            GUIManager gui = GetComponent<GUIManager>();
            gui.Initialize();

        }
        #endregion

        #region ポインティング
        pointingState |= PointingState.POINTING;
        #endregion

        #region アトラクター
        attractorState = attractorState | AttractorState.AVOID | AttractorState.AVOID_SCALE | AttractorState.SCALE_UP | AttractorState.FRAME;
        if((pointingState & PointingState.POINTING) == PointingState.POINTING)
        {
            attractorState |= AttractorState.SCALE_UP_MOUSE;
        }
        else if((pointingState & PointingState.VENN) == PointingState.VENN)
        {
            //attractorState |= AttractorState.FRAME;
        }
        zAxisDirection = true;
        #endregion

        systemState = ~SystemState.LOAD & systemState;
        systemState |= SystemState.MAIN;
    }
    public float WaitingTime = 2.0f;
    // Update is called once per frame
    void Update () {
        //WaitingTime -= Time.deltaTime; // Make the photos come to still stand as they reach some kind of equibrillibrium
        //if (systemState == SystemState.MAIN)
        //{
        //    if (lastattractorState != attractorState)
        //    {
        //        lastattractorState = attractorState;
        //        weight = new AttractorWeight(3.5f, 0, 0, 0, 0, 0, 0, 100, 0);
        //    }
        //    else
        //    {
        //        if (WaitingTime <= 0 && weight.AvoidWeight >0.8)
        //        {
        //            weight.AvoidWeight -= 0.1f;
        //            WaitingTime = 2.0f;
        //        }
        //    }
        //}
	}
}
