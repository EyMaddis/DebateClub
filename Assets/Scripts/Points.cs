using UnityEngine;
using System.Collections;

public class Points : MonoBehaviour {

	private static readonly int[] PlayerPoints = {0,0};

    public Font Font;


	public void AddPoints(int playerID, int points)
	{
        Debug.Log("player"+playerID+" got a point");
	    PlayerPoints[playerID+-1] += points;
	}



	//Display on screen
	void OnGUI()
	{
	    DrawPoints();
	}

    void DrawPoints()
    {
        var centeredStyle = new GUIStyle();
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.fontStyle = FontStyle.Bold;
        centeredStyle.font = Font;
        centeredStyle.fontSize = 20;
        centeredStyle.richText = true;
        var xCenter = (float)Screen.width / 2;


        //var oldColor = GUI.color;
        GUILayout.BeginArea(new Rect(xCenter - 50, 15, 100, 50));
        GUILayout.BeginHorizontal(centeredStyle);

        GUILayout.Label("<color=blue>" + (GetPoints(1)-GetPoints(2))+"</color>");

        GUILayout.FlexibleSpace();

        GUILayout.Label("<color=red>" + (GetPoints(2) - GetPoints(1)) + "</color>");

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        //GUI.color = oldColor;
    }


    public int GetPoints(int playerId)
    {
        return PlayerPoints[playerId - 1];
    }

    public void Reset()
    {
        PlayerPoints[0] = 0;
        PlayerPoints[1] = 0;
    }
}

