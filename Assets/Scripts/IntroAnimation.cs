using UnityEngine;
using System.Collections;

public class IntroAnimation : MonoBehaviour
{

    public GameObject player;

    private SpriteRenderer _spriteRenderer;
    private Animator _animationController;
   // private bool done = false;
	// Use this for initialization
	void Start ()
	{
	    _spriteRenderer = player.GetComponent<SpriteRenderer>();
	    _animationController = player.GetComponent<Animator>();
	    _animationController.enabled = false;
        _spriteRenderer.enabled = false;
	}

    void Awake()
    {
        _spriteRenderer = player.GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        //if(!done) _spriteRenderer.enabled = false;
	}

    public void ShowPlayer()
    {
        _spriteRenderer.enabled = true;
        _animationController.enabled = true;
        Debug.Log(player.name);
        //done = true;
    }
}
