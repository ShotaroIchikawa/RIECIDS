using UnityEngine;
using System.Collections;

public class AreaAdjustor  {

    public GameObject wall;
    public Vector3 direction;

    public AreaAdjustor(GameObject Wall, Vector2 _direction)
    {
        direction = _direction;
        wall = Wall;
    }
}
