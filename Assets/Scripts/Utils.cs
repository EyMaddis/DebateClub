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

    public static void DrawRound(int round, float time, Font font)
    {
        if (time < 5)
        {
            var centeredStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                font = font,
                fontSize = 50,
                normal = {textColor = Color.green}
            };

            GUI.Label(new Rect(0f, -100f, Screen.width, Screen.height), "Round " + round, centeredStyle);
        }
        else
        {
            var centeredStyle = new GUIStyle
            {
                alignment = TextAnchor.UpperRight,
                font = font,
                fontSize = 20,
                richText = true,
                normal = { textColor = Color.green }
            };

            GUI.Label(new Rect(-100f, 0f, Screen.width, Screen.height), "Round " + round, centeredStyle);
        }

        
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
}
