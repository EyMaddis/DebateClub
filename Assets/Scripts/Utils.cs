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

    /*********************************************
     * Display on Screen
     * *******************************************/      
     
    public static void DrawWinner(int player, Font font)
    {
        var centeredStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            font = font,
            fontSize = 50,
            normal = { textColor = player == 1 ? Color.blue : Color.red }
        };

        GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), "Player " + player + " won!", centeredStyle);
    }



    public static void DrawPoints(int[] points, Font font)
    {
        var centeredStyle = new GUIStyle()
        {
            alignment = TextAnchor.UpperCenter,
            fontStyle = FontStyle.Bold,
            font = font,
            fontSize = 20,
            richText = true
        };
        
        var xCenter = (float)Screen.width / 2;


        //var oldColor = GUI.color;
        GUILayout.BeginArea(new Rect(xCenter - 50, 15, 100, 50));
        GUILayout.BeginHorizontal(centeredStyle);

        GUILayout.Label("<color=blue>" + (points[0] - points[1]) + "</color>");

        GUILayout.FlexibleSpace();

        GUILayout.Label("<color=red>" + (points[1] - points[0]) + "</color>");

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        //GUI.color = oldColor;
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
