using UnityEngine;
using System.Collections;

public class Points : MonoBehaviour {

	public int Player1Points = 0;
	public int Player2Points = 0;



	public void AddPoints(int playerID, int points)
	{
	    switch (playerID)
	    {
            case 1:
                Debug.Log("player2 got hit");
			    Player1Points += points;
                break;
            case 2:
                Debug.Log("player1 got hit");
			    Player2Points += points;
                break;
	    }
			
	}

	// Use this for initialization
	void Start () 
	{
	 
	}
	
	// Update is called once per frame
	void Update () 
	{
	

	}

	//Display on screen
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(5, 5, 500, 500));
		GUILayout.Label("Player 1: " + Player1Points);
		GUILayout.Label("Player 2: " + Player2Points);
		GUILayout.EndArea();
	}
}

