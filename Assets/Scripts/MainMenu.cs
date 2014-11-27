using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width/2.5f,Screen.height/3,Screen.width/5,Screen.height/10), "Start Game"))
		{
			Application.LoadLevel(1);
		}

		if(GUI.Button(new Rect(Screen.width/2.5f,Screen.height/2,Screen.width/5,Screen.height/10), "Exit"))
		{
			Application.Quit();
		}
	}

}
