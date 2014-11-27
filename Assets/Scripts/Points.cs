using UnityEngine;
using System.Collections;

public class Points : MonoBehaviour {

	private static readonly int[] PlayerPoints = {0,0};
    public int WinningDifference = 2;

    public Font Font;


	public void AddPoints(int playerID, int points)
	{
        Debug.Log("player"+playerID+" got a point");
	    PlayerPoints[playerID+-1] += points;
	}

    /// <summary>
    ///  Check if a player has won
    /// </summary>
    /// <returns>0 if nobody wins, 1 if player 1 wins, 2 for player 2</returns>
    public int CheckWinCondition()
    {
        var diff = PlayerPoints[0] - PlayerPoints[1];
        if (diff <= -WinningDifference) return 2;
        return diff >= WinningDifference ? 1 : 0;
    }


	//Display on screen
	void OnGUI()
	{
	    DrawPoints();
	    DrawWinner();
	}

    void DrawPoints()
    {
        var centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.fontStyle = FontStyle.Bold;
        centeredStyle.font = Font;
        var xCenter = (float)Screen.width / 2;


        GUILayout.BeginArea(new Rect(xCenter - 50, 15, 100, 50));
        GUILayout.BeginHorizontal(centeredStyle);

        GUI.color = Color.blue;
        GUILayout.Label("" + GetPoints(1));

        GUILayout.FlexibleSpace();

        GUI.color = Color.red;
        GUILayout.Label("" + GetPoints(2));

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void DrawWinner()
    {
        
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

