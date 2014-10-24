using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterMovement))]
public class CharacterCombat : MonoBehaviour
{

    public GameObject HighHit;
    public GameObject MidHit;
    public GameObject LowHit;

    [Header("Debugging")]
    public bool _highHitTriggered;
    public bool _midHitTriggered;
    public bool _lowHitTriggered;

    private LayerTrigger _midTrigger;

    private CharacterMovement _movement;
    private int _playerId;

    private string _punchInputName;
   



	// Use this for initialization
	void Start ()
	{
	    _movement = GetComponentInParent<CharacterMovement>();
	    _playerId = _movement.PlayerId;
	    _midTrigger = MidHit.GetComponent<LayerTrigger>();
	    SetInputs();
	}

    private void SetInputs()
    {
        _punchInputName = "Punch";
        if (_playerId != 1)
        {
            _punchInputName += _playerId;
        }
    }
	
	// Update is called once per frame
	void Update () {
        UpdateStates();
	    HandleCombat();

	}

    private void UpdateStates()
    {       
        _highHitTriggered = HighHit.GetComponent<LayerTrigger>().isTriggered;
        _midHitTriggered = _midTrigger.isTriggered;
        _lowHitTriggered = LowHit.GetComponent<LayerTrigger>().isTriggered;
    }

    private void HandleCombat()
    {
        if (!Input.GetButtonDown(_punchInputName)) return;
        /* Get Players State via Movement System */

        if (_highHitTriggered || _midHitTriggered || _lowHitTriggered)
        {
            Debug.Log("Hit of Player " + _playerId);
        }
    }

}
