using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Character))]
public class CharacterMovement : MonoBehaviour{

    //[Tooltip("Is the character looking to the left (false) or right (true)?")]
    //public bool IsFacingRight = true;

    [Header("Movement Speed")]
    public float MoveSpeed = 25f; 			// Movement Speed
    public float CrouchingSpeed = 5f; 			// Movement Speed while crouching


    [Header("Jumping")]

    [Tooltip("How often should the character be able to jump without landing")]
    public int MaxJumps = 2;
    public float JumpForce = 300f;			// Jump Force
    public float InAirSpeed = 10f; 			// Movement Speed while jumping/falling
    [Tooltip("At which impact force the character should start rolling")]
    public float RollingImpactThreshold = 6.0f;

    [Header("Wallsliding")]

    public float WallJumpForce = 3000f;			// Jump Force

    [Tooltip("How much drag should the character have while sliding down? 0 means no sliding")]
    [Range(0.0f, 100f)]
    public float WallJumpDrag = 100.0f;

    [Range(0.0f, 2f)]
    [Tooltip("How long will the character keep on sliding even without input from the player?")]
    public float WallSlideEndDelay = 0.3f;
    

  
    [Header("Input Names")]
    private string _jumpInputName;
    private string _xAxisInputName;
    private string _yAxisInputName;


    [Header("Debugging Only")]
    
    public bool _isWallSlidingByDelay = false;

    private Character _character;

    private float _horizontalInput;
    private float _verticalInput;
    private Vector2 _inputVector;
    private bool _jumpInput;

    public int _jumpCount = 0;

    private float _maxVelocity;

    private float _dragBackup;
    private bool _lastFrameSliding = false;
    private bool _lastFrameGrounded = false;

    private Vector2 _lastFrameVelocity;
    private Vector2 _velocity;
    
    private bool _activeSliding = false; // is sliding in current frame
    private bool _abortWallSlidingDelay = false;

    // triggers that are only true for one frame (for the animator)
    private bool _jumpingTrigger = false;
    private bool _wallJumpingTrigger = false;
    private bool _landingTrigger = false;
    private bool _rollingTrigger = false;

    void Start()
    {
        _character = GetComponentInParent<Character>() as Character;
        _dragBackup = rigidbody2D.drag;

        
        InitializeInputs();
        RollingImpactThreshold *= RollingImpactThreshold; // square for performance
        if (transform.localScale.x < 0) _character.Direction = -1; //TODO Bitte was????
    }

    private void InitializeInputs()
    {
        var id = _character.PlayerId;
        _jumpInputName = "Jump";
        _xAxisInputName = "Horizontal";
        _yAxisInputName = "Vertical";

        if (id == 1) return;
        _jumpInputName += id;
        _xAxisInputName += id;
        _yAxisInputName += id;
    }

    void Update()
    {
        UpdateStates();
        GetInput(); 			// Handles Input
        

        UpdateAnimator();
    }

    void FixedUpdate()
    {
        HandleWallSliding();
        HandleDirection();
        HandleMovement(); 		// Handles Movement

        HandleJump();
        ClearInput();

    }

    private void UpdateStates()
    {
        // reset triggers
        _jumpingTrigger = false;
        _wallJumpingTrigger = false;
        _landingTrigger = false;
        _rollingTrigger = false;

        _lastFrameVelocity = _velocity;
        _velocity = rigidbody2D.velocity;

        _lastFrameGrounded = _character.IsGrounded;
        _maxVelocity = MoveSpeed*Time.deltaTime;

    }

   private void GetInput()
   {
		_horizontalInput = Input.GetAxisRaw(_xAxisInputName); 		// Set "horiztonalInput" equal to the Horizontal Axis Input
		_verticalInput = Input.GetAxisRaw(_yAxisInputName); 			// Set "verticallInput" equal to the Vertical Axis Inpu
        _inputVector = new Vector2(_horizontalInput, _verticalInput).normalized;

       if(_jumpInput) return;
       _jumpInput = Input.GetButtonDown(_jumpInputName);
   }

    private void ClearInput()
    {
        _jumpInput = false;
    }

    private void HandleDirection()
    {
        if(_character.IsWallSliding) return;

        if (_character.Direction * _horizontalInput < 0)
        {
            _character.Flip();
        }
    }

    private void HandleMovement()
    {
        _character.IsCrouching = false;
        if (_character.IsGrounded)
        {
            var speed = MoveSpeed;

            // player is pressing down: crouching
            if (_verticalInput < 0)
            {
                _character.IsCrouching = true;
                speed = CrouchingSpeed;
            }

            // did not stand before, so probably landed right now
            if (!_lastFrameGrounded) // TODO does not always work!
            {
                _landingTrigger = true;
                // landing very hard? Maybe unreliable
                var forceSquared = _lastFrameVelocity.SqrMagnitude();
                if (forceSquared >= RollingImpactThreshold)
                {
                    _rollingTrigger = true; // TODO move character while rolling
                }
            }
            
            
            var velocity = rigidbody2D.velocity;
            velocity.x = _horizontalInput*speed * Time.deltaTime; // actually moves gameObject
            rigidbody2D.velocity = velocity;             
        }
        else //in Air
        {
            rigidbody2D.AddForce((_verticalInput < 0 ? _inputVector : new Vector2(_horizontalInput, 0))*InAirSpeed);

            //Stop unlimmited acceleration.
            var velocity = rigidbody2D.velocity;
            if (velocity.x > _maxVelocity || velocity.x < (_maxVelocity*-1))
            {
                rigidbody2D.velocity = new Vector2(_maxVelocity*_character.Direction, velocity.y);
            }
                
        }
    }

    private
       void HandleJump()
    {
        if (!_jumpInput) return;
        if (_character.IsGrounded)
        { // reset JumpCount
            _jumpCount = 0;
        }

        if (!_character.IsWallSliding) // regular jump
        {
            if (_jumpCount < MaxJumps)
            {
                _jumpCount++;
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0); // Set the y velocity to 0
                rigidbody2D.AddForce((_inputVector + Vector2.up).normalized * JumpForce, ForceMode2D.Impulse); 	// Add y force set by "jumpForce" * Time.deltaTime?                
                _jumpingTrigger = true; // trigger
            } 
           
        }
        else // jumping from the wall
        {
            if (_character.Direction * _horizontalInput < 0 && _character.BackTriggered)
            {
                _inputVector.x *= -1;
                _inputVector.y *= 1;
            }
            _wallJumpingTrigger = true;

            StopWalllSliding();
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0); // Set the y velocity to 0
            rigidbody2D.AddForce((_inputVector + Vector2.up + _character.DirectionVector()).normalized * WallJumpForce, ForceMode2D.Impulse); 	// Add y force set by "jumpForce" * Time.deltaTime?
        }
        
    }

    private void HandleWallSliding()
    {
        _activeSliding = false;

        // let the character slowly slide down the wall when he jumped and pressed against the wall
        if (/*_direction*_horizontalInput > 0 &&*/ _character.FrontTriggered && !_character.IsGrounded && !_character.IsWallSliding)
        {
            StartWallSliding();
        }

        if (_character.IsWallSliding)
        {
            OnWallSliding();
        }
    }

    // let the character slide on a wall/object
    private void StartWallSliding()
    {
        _dragBackup = rigidbody2D.drag;
        rigidbody2D.drag = WallJumpDrag;
        _character.IsWallSliding = true;
        _activeSliding = true;
        _lastFrameSliding = true;
        _character.Flip();
    }

    // called every frame, if the character is sliding
    private void OnWallSliding()
    {
        if ((_character.IsWallSliding && !_character.BackTriggered) || _verticalInput < 0)
        {
            StopWalllSliding();
            return;
        }

        _activeSliding = false || _character.Direction * _horizontalInput < 0 && _character.BackTriggered;

        if(_lastFrameSliding && !_activeSliding)
        { // character stopped sliding
            StartCoroutine("EndSlidingWithDelay");
            return;
        }
        _lastFrameSliding = _activeSliding;
        
    }

    // immediately stop sliding
    private void StopWalllSliding()
    {
        _abortWallSlidingDelay = true;
        _isWallSlidingByDelay = false;
        _activeSliding = false;
        _character.IsWallSliding = false;
        rigidbody2D.drag = _dragBackup;
    }

    // will delay the end of the wall sliding for better user experience
    IEnumerator EndSlidingWithDelay()
    {
        _isWallSlidingByDelay = true;
        yield return new WaitForSeconds(WallSlideEndDelay);

        if (_abortWallSlidingDelay)
        {
            _abortWallSlidingDelay = false;
        }
        else
        {
            _isWallSlidingByDelay = false;
            _character.IsWallSliding = false;
            rigidbody2D.drag = _dragBackup;
        }
    }

    // inform the animator component about the new state of the character
    private void UpdateAnimator()
    {
        if (_rollingTrigger)
            _character.Animator.SetTrigger("startRolling");
        
        if (_jumpingTrigger)
            _character.Animator.SetTrigger("jumping");
        
        if (_wallJumpingTrigger)
            _character.Animator.SetTrigger("wallJumping");
    }
}
