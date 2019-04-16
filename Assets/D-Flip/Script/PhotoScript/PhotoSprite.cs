using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;

public class PhotoSprite : MonoBehaviour
{

    public Texture2D image;
    public GameObject col;
    public DflipPhoto myPhoto;
    public WallLabel myLabel;
    private Material spriteMaterial;
    private bool colorFlag = false;
    System.Drawing.Bitmap bit;

    public List<Vector3> hsvFeature;
    public double variance;

    public bool Load(string filePath, GameObject photo, System.Drawing.Bitmap bitmap)
    {
        hsvFeature = new List<Vector3>();
        bit = bitmap;
        col = photo;
        myPhoto = col.GetComponent<DflipPhoto>();
        spriteMaterial = gameObject.GetComponent<SpriteRenderer>().material;

        //スプライトを生成
        var data = File.ReadAllBytes(filePath);
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(data, true);
        image = tex;
        UnityEngine.Rect rec = new UnityEngine.Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rec, new Vector2(0.5f, 0.5f), 100);
        GetComponent<SpriteRenderer>().sprite = sprite;

        //色特徴量を計算
        Thread t = new Thread(new ThreadStart(CalcFeature));
        t.Start();  

        return true;
    }

    public void CalcFeature()
    {
        Vector3 averageRGB = Vector3.zero;

        BitmapData bits = bit.LockBits(new System.Drawing.Rectangle(0, 0, bit.Width, bit.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        byte[] buf = new byte[bit.Width * bit.Height * 4];
        Marshal.Copy(bits.Scan0, buf, 0, buf.Length);

        int w = bit.Width;
        int h = bit.Height;
        int pixelCount = 0;
        Vector3 rgbAve = Vector3.zero;

        for(int i = 0; i < 5; ++i)
        {
            for (int j = 0; j < 5; ++j)
            {
                int count = 0;
                Vector3 rgb = Vector3.zero;
                Vector3 hsv = Vector3.zero;
                for (int ii = h * i / 5; ii < h * (i + 1) / 5; ++ii)
                {
                    for (int jj = w * j / 5; jj < w * (j + 1) / 5; ++jj)
                    {
                        int index = 4 * (jj + ii * w);
                        rgb.x += buf[index];     //R
                        rgb.y += buf[index + 1]; //G
                        rgb.z += buf[index + 2]; //B
                        averageRGB += new Vector3(buf[index], buf[index + 1], buf[index + 2]);
                        count++;
                        pixelCount++;
                    }
                }
                rgb /= (float)(count * 255);
                rgbAve += rgb;
                hsv = RGBtoHSV(rgb);
                hsvFeature.Add(hsv);
            }
        }

        Marshal.Copy(buf, 0, bits.Scan0, buf.Length);
        bit.UnlockBits(bits);

        Variance(RGBtoHSV(rgbAve/25));

        averageRGB /= pixelCount;
        float luminance = CalcPhotoColor(averageRGB, "L");
        myPhoto.metadata.Add(PhotoManager.Instance.keydataCode["輝度"], luminance);
        float saturation = CalcPhotoColor(averageRGB, "S");
        myPhoto.metadata.Add(PhotoManager.Instance.keydataCode["彩度"], saturation);

        if (PhotoCreator.last == true) //最後だったらminmaxcodeもつくる
        {
            PhotoManager.Instance.CreateMaxMinCode("彩度");
            PhotoManager.Instance.CreateMaxMinCode("輝度");
            PhotoCreator.last = false;
        }
    }

    Vector3 RGBtoHSV(Vector3 rgb)
    {
        Vector3 hsv = Vector3.zero;

        float min = Mathf.Min(rgb.x, rgb.y, rgb.z);
        float max = Mathf.Max(rgb.x, rgb.y, rgb.z);

        if (min == max)
        {
            hsv.x = 0;
        }
        else
        {
            if (max == rgb.x)
            {
                hsv.x = 60.0f * (rgb.y - rgb.z) / (max - min);
            }
            else if (max == rgb.y)
            {
                hsv.x = 60.0f * (rgb.z - rgb.x) / (max - min) + 120.0f;
            }
            else
            {
                hsv.x = 60.0f * (rgb.x - rgb.y) / (max - min) + 240.0f;
            }
        }
        if (hsv.x < 0)
        {
            hsv.x += 360;
        }
        hsv.y = max - min;
        hsv.z = max;

        return hsv;
    }

    void Variance(Vector3 hsvAve)
    {
        variance = 0;
        foreach(Vector3 ff in hsvFeature)
        {
            variance += AttractorColor.HsvDistance(ff, hsvAve);
        }
        variance /= 25;
    }

    // Use this for initialization
    void Start()
    {

    }

    public void UpdateSprite()
    {
        Vector3 currentPosition = new Vector3(col.transform.localPosition.x, col.transform.localPosition.y, 0);
        Vector3 currentScale = col.transform.localScale;

        currentPosition = Filter(currentPosition, true);
        currentScale = Filter(currentScale, false);

        if((SystemManager.attractorState & SystemManager.AttractorState.DEPTH_SORT) == SystemManager.AttractorState.DEPTH_SORT)
        {
            this.transform.localPosition = AttractorDepthSort(currentPosition);
        }
        else
        {
            this.transform.localPosition = currentPosition;
            if (colorFlag == false)
            {
                SetFrameColor();
            }
        }

        this.transform.localScale = currentScale;
    }

    private int lastKey;
    float min = 0;
    float max = 0;
    public Vector3 AttractorDepthSort(Vector3 position)
    {
        int key = PhotoManager.Instance.dSortKey;
        Vector3 spritePosition = new Vector3(position.x, position.y, gameObject.transform.localPosition.z);

        if (key > 0)
        {
            if(lastKey != key)
            {
                min = PhotoManager.minValue[key];
                max = PhotoManager.maxValue[key];
                lastKey = key;
            }
            if (myPhoto.metadata.ContainsKey(key) == true)
            {
                if(min != max)
                {
                    float difference = 10 * (myPhoto.metadata[key] - min) / (max - min) - gameObject.transform.localPosition.z;
                    spritePosition += new Vector3(0, 0, difference * 0.01f);
                    SetFrameColor(key);
                }
            }
            
        }
        return spritePosition;
    }

    private void SetFrameColor(int key)
    {
        float colorVector;
        if (SystemManager.zAxisDirection == true)
        {
            colorVector = (myPhoto.metadata[key] - min) / (max - min);
        }
        else
        {
            colorVector = 1 - (myPhoto.metadata[key] - min) / (max - min);
        }
        Color color = new Color();
        spriteMaterial.SetFloat("_Offset", 0.2f);
        if (SystemManager.zAxisDirection == true)
        {
            if (gameObject.transform.localPosition.z < ZAxisSlider.Instance.zSliderPosition.z && myPhoto.isClicked == false)
            {
                color = new Color(0.5f, 0.5f, 0.5f);
                myPhoto.transform.localScale = new Vector3(0, 0, 0); // Change scale
                //Destroy(col);Destroy(myPhoto);
            }
            else
            {
                color = Color.white;
            }
        }
        else
        {
            if (gameObject.transform.localPosition.z > ZAxisSlider.Instance.zSliderPosition.z && myPhoto.isClicked == false)
            {
                color = new Color(0.5f, 0.5f, 0.5f);
                myPhoto.transform.localScale = new Vector3(0, 0, 0); // Change scale
                //Destroy(col);Destroy(myPhoto);
            }
            else
            {
                color = Color.white;
            }
        }
        
        gameObject.GetComponent<SpriteRenderer>().color = color;
        color = new Color(colorVector, colorVector, colorVector);
        spriteMaterial.SetColor("_OutLineColor", color);
        colorFlag = false;
    }

    private void SetFrameColor()
    {
        //枠線の色を白に戻す
        spriteMaterial.SetColor("_OutLineColor", Color.white);
        spriteMaterial.SetFloat("_Offset", 0.1f);
        colorFlag = true;
    }

    public float CalcPhotoColor(Vector3 averageRGBA, string flag)
    {
        float averageValue = 0;
        switch (flag)
        {
            case "L":              
                averageValue = 0.299f * averageRGBA.x + 0.587f * averageRGBA.y + 0.114f * averageRGBA.z;
                break;
            case "S":
                float max = Mathf.Max(averageRGBA.x, averageRGBA.y, averageRGBA.z);
                averageValue = (max - Mathf.Min(averageRGBA.x, averageRGBA.y, averageRGBA.z)) / max;
                break;
            default:
                break;
        }
        return averageValue;
    }

    private Vector3 Filter(Vector3 value, bool state)
    {
        switch (PhotoManager.Instance.filterSelector)
        {
            case PhotoManager.filter.SMA:
                value = SMA(value, state);
                break;
            case PhotoManager.filter.LWMA:
                value = LWMA(value, state);
                break;
            case PhotoManager.filter.GAUSS:
                value = GAUSS(value, state);
                break;
            case PhotoManager.filter.DEFAULT:
                break;
            default:
                break;
        }
        return value;
    }

    //過去フレームの値を保存する用の変数
    List<Vector3> lastPosition = new List<Vector3>();
    List<Vector3> lastScale = new List<Vector3>();
    Vector3 sumPosition;
    Vector3 sumScale;
    private Vector3 SMA(Vector3 value, bool state) // SimpleMovingAverage単純移動平均
    {
        int frame = 20;
        List<Vector3> lastValue = state ? lastPosition : lastScale;
        Vector3 sumValue = state ? sumPosition : sumScale;

        lastValue.Add(value);
        sumValue += lastValue[lastValue.Count - 1];

        if (lastValue.Count > frame)
        {
            sumValue -= lastValue[0];
            lastValue.RemoveAt(0);
        }

        value = sumValue / frame;

        #region 過去フレーム値を保存する処理
        if (state == true)
        {
            lastPosition = lastValue;
            sumPosition = sumValue;
        }
        else
        {
            lastScale = lastValue;
            sumScale = sumValue;
        }
        #endregion

        return value;
    }

    private Vector3 LWMA(Vector3 value, bool state) //LinearWeightedMovingAverage線形加重移動平均
    {
        int frame = 20;
        List<Vector3> lastValue = state ? lastPosition : lastScale;
        Vector3 sumValue = state ? sumPosition : sumScale;

        lastValue.Add(value);
        sumValue += lastValue[lastValue.Count - 1];

        for (int i = 0; i < lastValue.Count; i++)
        {
            value += lastValue[i] * (i + 1);
        }
        value /= 21 * (lastValue.Count + 1) / 2;

        if(lastValue.Count > frame)
        {
            sumValue -= lastValue[0];
            lastValue.RemoveAt(0);
        }
        
        #region 過去フレーム値を保存する処理
        if (state == true)
        {
            lastPosition = lastValue;
            sumPosition = sumValue;
        }
        else
        {
            lastScale = lastValue;
            sumScale = sumValue;
        }
        #endregion

        return value;
    }

    private Vector3 GAUSS(Vector3 value, bool state) //Gaussianガウシアンフィルタ
    {
        int frame = 20;
        float sigma2 = (float)(7);
        List<Vector3> lastValue = state ? lastPosition : lastScale;
        Vector3 sumValue = state ? sumPosition : sumScale;

        lastValue.Add(value);
        sumValue += lastValue[lastValue.Count - 1];

        for (int i = 0; i < lastValue.Count; i++)
        {
            float x = lastValue.Count - i - 1;
            value += lastValue[i] *(1/3)* (1 / Mathf.Sqrt(2*Mathf.PI*sigma2)) * Mathf.Exp(-(x*x) / (2*sigma2));
        }

        if (lastValue.Count > frame)
        {
            sumValue -= lastValue[0];
            lastValue.RemoveAt(0);
        }

        #region 過去フレーム値を保存する処理
        if (state == true)
        {
            lastPosition = lastValue;
            sumPosition = sumValue;
        }
        else
        {
            lastScale = lastValue;
            sumScale = sumValue;
        }
        #endregion

        return value;
    }

}


