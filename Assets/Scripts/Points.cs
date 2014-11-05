using UnityEngine;
using System.Collections;

public class Points : MonoBehaviour {

	public int Player1Points = 0;
	public int Player2Points = 0;



	/*public void AddPoints( string player)
	{
		if (player == "player1") 
		{
			Player1Points +=1;
		}

		if (player == "player2") 
		{
			Player2Points +=1;
		}
	}
*/
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
		GUILayout.Label("Player1: " + Player1Points);
		GUILayout.Label("Player2: " + Player2Points);
		GUILayout.EndArea();
	}
}

