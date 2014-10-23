using System;
using UnityEngine;
using System.Collections;

public class Utils {

    public static float LimitValue(float value, float min , float max)
    {
        value = Math.Max(value, min);
        value = Math.Min(value, max);
        return value;
    }

    public static Vector2 DeleteZDimension(Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }
}
