using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
    private bool _startPressed;
    private bool _backPressed;
	void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width/2.5f,Screen.height/3,Screen.width/5,Screen.height/10), "Start Game") || _startPressed)
		{
			Application.LoadLevel(1);
		}

		if(GUI.Button(new Rect(Screen.width/2.5f,Screen.height/2,Screen.width/5,Screen.height/10), "Exit") || _backPressed)
		{
			Application.Quit();
		}
	}

    void Update()
    {
        _backPressed = Input.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.Escape);
        _startPressed = Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.Return);
    }

}
