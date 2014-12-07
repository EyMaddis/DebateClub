using UnityEngine;
using System.Collections;

public class IntroAnimation : MonoBehaviour
{

    public GameObject Player;

    private SpriteRenderer _spriteRenderer;
    private Animator _animationController;

	void Start ()
	{
	    _spriteRenderer = Player.GetComponent<SpriteRenderer>();
	    _animationController = Player.GetComponent<Animator>();
	    _animationController.enabled = false;
        _spriteRenderer.enabled = false;
	}

    void Awake()
    {
        _spriteRenderer = Player.GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }
	

    public void ShowPlayer()
    {
        _spriteRenderer.enabled = true;
        _animationController.enabled = true;
        Debug.Log(Player.name);
    }
}
