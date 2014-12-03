using UnityEngine;
using System.Collections;



public class Points : MonoBehaviour {
    public Font Font;
	private static readonly int[] PlayerPoints = {0,0};

    
	public void AddPoints(int playerID, int points)
	{
        Debug.Log("player"+playerID+" got a point");
	    PlayerPoints[playerID+-1] += points;
	}



	//Display on screen
	void OnGUI()
	{
        Utils.DrawPoints(PlayerPoints,Font);
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

