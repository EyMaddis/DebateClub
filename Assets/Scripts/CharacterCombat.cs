using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class CharacterCombat : MonoBehaviour
{
    private Character _character;
    private Animator _animator;
    private string _punchInputName;
    
    //Actions
    private bool _divekicking = false;
    private bool _punching = false;
    private bool _groundkicking = false;

	public string nameofthisplayer; // Johan
	private Points points;// Johan

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
	    UpdateStates();
	    HandleCombat();
        UpdateAnimator();
	}

    private void UpdateStates()
    {
         _divekicking = false;
         _punching = false;
         _groundkicking = false;
        
    }
    
    private void HandleCombat()
    {
        if (!Input.GetButtonDown(_punchInputName)) return;
        /* Get Players State via Movement System */

        if (_character.IsGrounded) //Ground Attack
        {
            if (_character.IsCrouching) //Chrouch Attack
            {
                _groundkicking = true;
                if (_character.LowHitTriggered)
                {
                    OnHit();
                }
            }
            else //Normal Punch
            {
                _punching = true;
                if (_character.MidHitTriggered)
                {
                   OnHit();
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

    private void OnHit()  // johan
    {

		if (nameofthisplayer == "player1")  
		{
			//add point for player2
			Debug.Log("player2 got hit");
			points.Player2Points += 1;
		}
		
		if (nameofthisplayer == "player2")
		{
			//add point for player1
			Debug.Log("player1 got hit");
			points.Player1Points += 1;
		}





        //TODO onHit
    }

    private void UpdateAnimator()
    {
        if (_punching) _animator.SetTrigger("Punching");
//        _animator.SetBool("Divekicking", _divekicking);
//        _animator.SetBool("Groundkicking", _groundkicking);
    }

}
