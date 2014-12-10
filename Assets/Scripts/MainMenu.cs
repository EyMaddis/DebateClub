using System;
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    public Sprite StartGameSprite;
    public Sprite StartGameSpriteActive;
    public Sprite ExitGameSprite;
    public Sprite ExitGameSpriteActive;

    public int Margin = 15;

    private GUIStyle _blankStyle = new GUIStyle();

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
        if (GUILayout.Button(StartGameSprite.texture, _blankStyle))
        {
            Application.LoadLevel(1);
        }
        GUILayout.FlexibleSpace();
	    
        if (GUILayout.Button(ExitGameSprite.texture, _blankStyle))
        {
            Application.Quit();
        }



        GUILayout.EndVertical();
        GUILayout.EndArea();

//        if (GUI.Button(new Rect(Screen.width / 2F - rect.width / 2f, Screen.height / 2f - rect.height / 2f, rect.width, rect.height), StartGameSprite.texture, _blankStyle))
//		{
//			Application.LoadLevel(1);
//		}
//
//        if (GUI.Button(new Rect(Screen.width / 2.5f, Screen.height / 2, Screen.width / 5, Screen.height / 10), "Exit", _blankStyle))
//		{
//			Application.Quit();
//		}
	}

}
