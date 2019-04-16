using UnityEngine;
using System.Collections;

public class PhotoAdjacency {

    public DflipPhoto photo;
    public Vector2 direction;
    public float angle;

    public PhotoAdjacency(DflipPhoto _photo, Vector2 _direction, float _angle)
    {
        photo = _photo;
        direction = _direction;
        angle = _angle;
    }
}
