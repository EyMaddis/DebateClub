using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class CharacterCombat : MonoBehaviour
{
    private Character _character;
    private Points points;
    private Animator _animator;
    private string _punchInputName;
    private bool _punch;
    
    //Actions
    private bool _divekicking = false;
    private bool _punching = false;
    private bool _groundkicking = false;


	public AudioClip[] audioClip;

    void Awake()
	{
		points = FindObjectOfType<Points> ();
	}

    // Use this for initialization
	void Start ()
	{
	    _character = GetComponentInParent<Character>();
	    _animator = GetComponentInParent<Animator>();
	    SetInputs();
		//points = GetComponent<Points> ();
	}

    private void SetInputs()
    {
        var id = _character.PlayerId;
        _punchInputName = "Punch";
        if (id != 1)
        {
            _punchInputName += id;
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
	    GetInput();
        UpdateAnimator();
	}

    void FixedUpdate()
    {
        UpdateStates();
        HandleCombat();
        ClearInput();
    }

    private void GetInput()
    {
        if(_punch) return;
        _punch = Input.GetButtonDown(_punchInputName);
    }

    private void ClearInput()
    {
        _punch = false;
    }

    private void UpdateStates()
    {
        if (_character.IsGrounded || _character.IsWallSliding)
        {
            _divekicking = false;
        }
         _punching = false;
         _groundkicking = false;
        
    }
    
    private void HandleCombat()
    {
        if (!_punch) return;
        /* Get Players State via Movement System */

        if (_character.IsGrounded) //Ground Attack
        {
            if (_character.IsCrouching) //Chrouch Attack
            {
                _groundkicking = true;
                if (_character.LowHitTriggered)
                {
                    //OnHit();
                }
            }
            else //Normal Punch
            {
                _punching = true;
                if (_character.MidHitTriggered)
                {
                   OnHit();
					PlaySound (0);
                }  
            }
            
        }
        else // Air Attack
        {
            if (_character.IsWallSliding) //WallSliding Attack
            {
                
            }
            else // Divekick;
            {
                _divekicking = true;
                if (_character.LowHitTriggered)
                {
                   OnHit(); 
                }
            }
        }
    }

    private void OnHit() 
    {
        points.AddPoints(_character.PlayerId, 1);

    }

    private void UpdateAnimator()
    {
        if (_punching) _animator.SetTrigger("Punching");
        _animator.SetBool("Divekicking", _divekicking);
//        _animator.SetBool("Groundkicking", _groundkicking);
    }
	void PlaySound(int clip)
	{
		audio.clip =audioClip[clip];
		audio.Play ();
	}
}
