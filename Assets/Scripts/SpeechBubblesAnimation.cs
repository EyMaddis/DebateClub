using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEditorInternal;
using UnityEngine;

public class SpeechBubblesAnimation : MonoBehaviour
{

    public Vector2 OffsetPlayer1 = new Vector2(0.5f, 0.5f);
    public Vector2 OffsetPlayer2 = new Vector2(0.5f, 0.5f);
    public Color ColorPlayer1 = Color.blue;
    public Color ColorPlayer2 = Color.red;

    public float Duration = 1f;

    private readonly GUIStyle _style = new GUIStyle();
    private bool _showBubble = false;
    private Vector3 _screenPos;
    private DialogSystem _dialogSystem;
    private DialogSystem.DialoguePhase _currentPhase;


    // Use this for initialization
	void Start ()
	{
	    _dialogSystem = GetComponent<DialogSystem>();
        _style.alignment = TextAnchor.UpperCenter;
	}

    void Update()
    {
        _screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    }

    void OnGUI()
    {
        if (!_showBubble) return;
        var dia = _dialogSystem.GetCurrent();
        var text = dia.Player2;
        var showPlayer1 = false;
        switch (_currentPhase)
        {
            case DialogSystem.DialoguePhase.Player1Response:
                text = dia.Player1Response;
                showPlayer1 = true;
                break;
            case DialogSystem.DialoguePhase.Player2Response:
                text = dia.Player2Response;
                break;
        }


        _style.normal.textColor = ColorPlayer1;
        var offset = OffsetPlayer1;
        if (!showPlayer1)
        {
            offset = OffsetPlayer2;
            _style.normal.textColor = ColorPlayer2;
        }
        
        GUI.Label(new Rect(_screenPos.x + offset.x, Screen.height - _screenPos.y + offset.y, 150, 100), text, _style);
    }

    public void ShowDialoguePart(DialogSystem.DialoguePhase phase)
    {
        _currentPhase = phase;
        _showBubble = true;
        Debug.Log("Starting to show dialogue phase: "+phase);
        StartCoroutine("Hide");
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(Duration);
        _showBubble = false;
    }



}
