using UnityEngine;
using System.Collections;

public class AttractorWeight  {
    private float avoidWeight_;
    private float avoidScaleWeight_;
    private float scaleUpWeight_;

    private float hSortWeight_;
    private float vSortWeight_;
    private float frameWeight_;

    private float attractToMouseWeight_;
    private float scaleUpMouseWeight_;

    private float noiseWeight_;

    public AttractorWeight(
        float avoid,
        float avoidScale,
        float scaleUp,
        float hSort,
        float vSort,
        float frame,
        float attractToMouse,
        float scaleUpMouse,
        float noisew)
    {
        avoidWeight_ = avoid;
        avoidScaleWeight_ = avoidScale;
        scaleUpWeight_ = scaleUp;
        hSortWeight_ = hSort;
        vSortWeight_ = vSort;
        frameWeight_ = frame;
        attractToMouseWeight_ = attractToMouse;
        scaleUpMouseWeight_ = scaleUpMouse;
        noiseWeight_ = noisew;
    }

    #region property
    public float AvoidWeight
    {
        get
        {
            return avoidWeight_;
        }
        set
        {
            avoidWeight_ = value;
        }
    }
    public float AvoidScaleWeight
    {
        get
        {
            return avoidScaleWeight_;
        }
    }
    public float ScaleUpWeight
    {
        get
        {
            return scaleUpWeight_;
        }
    }
    public float HorizontalSortWeight
    {
        get
        {
            return hSortWeight_;
        }
    }
    public float VerticalSortWeight
    {
        get
        {
            return vSortWeight_;
        }
    }
    public float FrameWeight
    {
        get
        {
            return frameWeight_;
        }
    }
    public float AttractToMouseWeight
    {
        get
        {
            return attractToMouseWeight_;
        }
    }
    public float ScaleUpMouseWeight
    {
        get
        {
            return scaleUpMouseWeight_;
        }
    }
    public float NoiseWeight
    {
        get
        {
            return noiseWeight_;
        }
    }
    #endregion
}
