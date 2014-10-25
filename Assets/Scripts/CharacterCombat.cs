using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Character))]
public class CharacterCombat : MonoBehaviour
{
    private Character _character;
    private string _punchInputName;
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
	    HandleCombat();
	}
    
    private void HandleCombat()
    {
        if (!Input.GetButtonDown(_punchInputName)) return;
        /* Get Players State via Movement System */

        Debug.Log("Hit");
        if (_character.HighHitTriggered || _character.MidHitTriggered || _character.LowHitTriggered)
        {
            Debug.Log("Hit of Player ");
        }
    }

}
