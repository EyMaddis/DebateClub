using System;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{

    public Vector2 TextOffset = new Vector2(0.5f, 0.5f);
    public Color Color = Color.white;
    public bool Enable = false;

    private readonly GUIStyle style = new GUIStyle();
    private String _text = "I hate you, fuck you!";
    private Vector3 _screenPos;

	// Use this for initialization
	void Start () {
	    
	}

    void Update()
    {
        _screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    }

    void OnGUI()
    {
        if (!Enable) return;
        style.normal.textColor = Color;
        GUI.Label(new Rect(_screenPos.x + TextOffset.x, Screen.height - _screenPos.y + TextOffset.y, 150, 100), _text, style);
    }

    public void SetText(String text)
    {
        _text = text;
    }
}
