using UnityEngine;
using System.Collections;

public class IntroAnimation : MonoBehaviour
{

    public GameObject Player;
    public float AspectRatio = 21/9;

    private SpriteRenderer _spriteRenderer;
    private Animator _animationController;
    private Character _character;
    private TwoPlayerCamera2D _twoPlayerCamera;
    private GameLogic _gameLogic;

    void Start ()
	{
	    _spriteRenderer = Player.GetComponent<SpriteRenderer>();
	    _animationController = Player.GetComponent<Animator>();
	    _animationController.enabled = false;
        _spriteRenderer.enabled = false;
	    _character = Player.GetComponent<Character>();
        _character.BlockInput(true);

        _twoPlayerCamera = Camera.main.GetComponent<TwoPlayerCamera2D>();
	    _twoPlayerCamera.enabled = false;
	    _gameLogic = FindObjectOfType<GameLogic>();
	}


    void Awake()
    {
        _spriteRenderer = Player.GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }
	

    public void ShowPlayer()
    {
        Debug.Log("Show player");
        _spriteRenderer.enabled = true;
        _animationController.enabled = true;
        _character.BlockInput(false);
        _twoPlayerCamera.enabled = true;
        _gameLogic.DeclareIntroEnd();
    }
}
