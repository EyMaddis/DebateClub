﻿using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class CharacterCombat : MonoBehaviour
{
    public AudioClip[] PunchingSounds;
    public float DiveKickForce = 5.0f;
    public float DiveKickVerticalForce = -2.0f;

    public float InputBlockFallback = 2f; // max 2 seconds

    private Character _character;
    private CharacterMovement _characterMovement;
    private GameLogic game;
    private Animator _animator;
    private string _punchInputName;
    private bool _punch;
    
    //Actions
    private bool _divekicking = false;
    private bool _punching = false;
    private bool _groundkicking = false;

    // states
    private bool _divekickHit = false;
    private bool _isPunching = false;

    void Awake()
	{
		game = FindObjectOfType<GameLogic> ();
	}

    // Use this for initialization
	void Start ()
	{
	    _character = GetComponentInParent<Character>();
	    _animator = GetComponentInParent<Animator>();
	    _characterMovement = GetComponentInParent<CharacterMovement>();
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
        if (_character.IsInputBlocked())
        {
            _punch = false;
            return;
        }
        if (_punch) return;
        _punch = Input.GetButtonDown(_punchInputName);
    }

    private void ClearInput()
    {
        _punch = false;
    }

    private void UpdateStates()
    {
        if (_divekicking && (_character.IsGrounded || _character.IsWallSliding))
        {
            _divekicking = false;
        }

        if(!_divekicking) _divekickHit = false;

         _punching = false;
         _groundkicking = false;

    }
    
    private void HandleCombat()
    {
        HandleDivekick();

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
                else
                {
                    OnMiss();
                }
            }
            else //Normal Punch
            {
                _punching = true;
                _character.BlockInput(true);
                StartCoroutine(StopPunchingCoroutine());
                if (_character.MidHitTriggered)
                {
                    OnHit();
                }
                else
                {
                    OnMiss();
                }
            }
            
        }
        else // Air Attack
        {
            if (_character.IsWallSliding) //WallSliding Attack
            {
                //TODO?
            }
            else // Divekick;
            {

                if (!_divekicking) OnMiss();
                _divekicking = true;
                Vector2 diveDirection = new Vector2(_character.Direction, DiveKickVerticalForce).normalized;
                _character.rigidbody2D.velocity = diveDirection*DiveKickForce;
            }
        }
    }

    private void HandleDivekick()
    {
        if (!_divekicking) return;

        if (!_divekickHit && _character.LowHitTriggered)
        {
            OnHit();
            _divekickHit = true;
        }
        
    }

    private void OnHit() 
    {
        PlaySound(0);
        game.AddPoint(_character.PlayerId);
    }

    private void OnMiss()
    {
        PlaySound(1);
    }

    private void UpdateAnimator()
    {
        if (_punching) _animator.SetTrigger("Punching");
        _animator.SetBool("Divekicking", _divekicking);
//        _animator.SetBool("Groundkicking", _groundkicking);
    }
	void PlaySound(int clip)
	{
		audio.clip =PunchingSounds[clip];
		audio.Play ();
	}

    // AnimationEvent
    public void StopPunching()
    {
        _isPunching = false;
        _character.BlockInput(false);
        StopCoroutine("StopPunchingCoroutine");
    }

    IEnumerator StopPunchingCoroutine()
    {
        yield return new WaitForSeconds(InputBlockFallback);
        StopPunching();
    }

    public bool IsDivekicking()
    {
        return _divekicking;
    }

    public bool PressedPunch()
    {
        return _punch;
    }
}
 
