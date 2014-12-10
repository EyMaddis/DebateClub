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


    // http://forum.unity3d.com/threads/outlined-text.43698/
    public static void DrawOutline(Rect position, string text, GUIStyle style, Color outColor, int thickness = 1){
        var backupColor = style.normal.textColor;

        for(var i = 1; i <= thickness; i++)
        {
            style.normal.textColor = outColor;
            position.x-= i;
            GUI.Label(position, text, style);
            position.x += 2*i;
            GUI.Label(position, text, style);
            position.x-=i;
            position.y-=i;
            GUI.Label(position, text, style);
            position.y += 2*i;
            GUI.Label(position, text, style);
            position.y-=i;
            style.normal.textColor = backupColor;
            GUI.Label(position, text, style);
        } 
    }
}
