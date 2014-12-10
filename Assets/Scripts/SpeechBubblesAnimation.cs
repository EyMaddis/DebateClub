using System.Collections;
using UnityEngine;

public class SpeechBubblesAnimation : MonoBehaviour
{

    public Transform BubblePosition1;
    public Transform BubblePosition2;

    public Color ColorPlayer1 = Color.blue;
    public Color ColorPlayer2 = Color.red;

    public int FontSize = 15;
    public Font Font;

    public Vector2 BubbleBox = new Vector2(150,100);
    public Sprite Sprite;

    public float Duration = 1f;

    private readonly GUIStyle _style = new GUIStyle();
    private bool _showBubble = false;
    private Vector3 _screenPos;
    private DialogSystem _dialogSystem;
    private DialogSystem.DialoguePhase _currentPhase;
    private Vector3 _bubblePos1;
    private Vector3 _bubblePos2;


    // Use this for initialization
	void Start ()
	{
	    _dialogSystem = GetComponent<DialogSystem>();
        _style.alignment = TextAnchor.LowerCenter;

	    _style.normal.background = Sprite.texture;
	    var b = 15;
        _style.padding = new RectOffset(b, b, b, b);
	    _style.wordWrap = true;
	    _style.fontSize = FontSize;
	    _style.font = Font;
	}
    
    void Update()
    {
        _bubblePos1 = Camera.main.WorldToScreenPoint(BubblePosition1.position);
        _bubblePos2 = Camera.main.WorldToScreenPoint(BubblePosition2.position);
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


        _style.normal.textColor = ColorPlayer2;
        var bubblePos = _bubblePos2;
        if (showPlayer1)
        {
            bubblePos = _bubblePos1;
            _style.normal.textColor = ColorPlayer1;
        }
        var renderedHeight = _style.CalcHeight(new GUIContent(text),BubbleBox.x);
        var boxRect = new Rect(bubblePos.x - BubbleBox.x / 2, bubblePos.y - renderedHeight - BubbleBox.y, BubbleBox.x, BubbleBox.y);

        GUILayout.BeginArea(boxRect);
        GUILayout.Box(text, _style);
        GUILayout.EndArea();

    }

    public void ShowDialoguePart(DialogSystem.DialoguePhase phase)
    {

        StopCoroutine("Hide");
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

	public AudioClip clip;
	
	public void PlayBoxExplosionSound()
	{
		audio.clip = clip;
		audio.Play ();
	}

}
