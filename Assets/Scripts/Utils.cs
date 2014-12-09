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


    private static float _lerpT = 0;
    public static void DrawRound(int round, float time, Font font)
    {
        var box = new Rect(Screen.width/2f - 100f, Screen.height/2f - 50f, 200f, 100f);
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

            GUI.Label(box, "Round " + round, centeredStyle);
        }
        else
        {
            var centeredStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                font = font,
                fontSize = (int)Mathf.Lerp(50f, 20f, _lerpT),
                richText = true,
                normal = { textColor = Color.Lerp(Color.green, Color.white, _lerpT) }
            };

            box = new Rect(
                Mathf.Lerp(box.xMin, Screen.width - box.width - 15, _lerpT), 
                Mathf.Lerp(box.yMin, 0, _lerpT), 
                box.width, box.height);
            
            GUI.Label(box, "Round " + round, centeredStyle);
            _lerpT += Time.deltaTime;
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
