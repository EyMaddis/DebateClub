using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class CharacterCombat : MonoBehaviour
{
    private Character _character;
    private string _punchInputName;
    
    //Actions
    private bool _divekicking = false;
    private bool _punching = false;
    private bool _groundkicking = false;



    // Use this for initialization
	void Start ()
	{
	    _character = GetComponentInParent<Character>() as Character;
	    SetInputs();
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

    private void OnHit()
    {
        //TODO onHit
    }

    private void UpdateAnimator()
    {
        //TODO Animatior for combat
    }

}
