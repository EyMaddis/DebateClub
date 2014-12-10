using System;
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private bool _startPressed;
    private bool _backPressed;
    public Sprite StartGameSprite;
    public Sprite ExitGameSprite;

    public int Margin = 15;

    private readonly GUIStyle _blankStyle = new GUIStyle();

    void Start()
    {

    }

	void OnGUI()
	{
	    var startRect = StartGameSprite.textureRect;
	    var exitRect = ExitGameSprite.textureRect;

	    var width = Math.Max(startRect.width, exitRect.width);
	    var height = startRect.height + exitRect.height + Margin;

	    var box = new Rect(Screen.width/2F - width/2f, Screen.height/2f - height/2f, width, height);

        GUILayout.BeginArea(box, _blankStyle);
        GUILayout.BeginVertical(_blankStyle);
        if (GUILayout.Button(StartGameSprite.texture, _blankStyle) || _startPressed)
        {
            Application.LoadLevel(1);
        }
        GUILayout.FlexibleSpace();
	    
        if (GUILayout.Button(ExitGameSprite.texture, _blankStyle) || _backPressed)
        {
            Application.Quit();
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

	}

    void Update()
    {
        _backPressed = Input.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.Escape);
        _startPressed = Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.Return);
    }


}
