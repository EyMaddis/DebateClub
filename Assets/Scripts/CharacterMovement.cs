using System;
using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    [Range(1,2)]
    public int PlayerId = 1;

    [Tooltip("Is the character looking to the left (false) or right (true)?")]
    public bool IsFacingRight = true;

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
    

    [Header("Trigger")]
    public int GroundLayerId = 8;
    public int WallLayerId = 10;

    public GameObject FootInFront;
    public GameObject FootInBack;
    public GameObject WallTriggerBack;
    public GameObject WallTriggerFront;

    [Header("Input Names")]
    private string _jumpInputName;
    private string _xAxisInputName;
    private string _yAxisInputName;


    [Header("Debugging Only")]

    public bool _isGrounded = false;
    public bool _isWallSliding = false;
    public bool _isWallSlidingByDelay = false;
    public bool _isCrouching = false;

    private float _horizontalInput;
    private float _verticalInput;
    private Vector2 _inputVector;

    public int _jumpCount = 0;

    private int _direction = 1;
    private Animator _animator; 

    private float _maxVelocity;

    private LayerTrigger _footInBackTrigger;
    private LayerTrigger _footInFrontTrigger;
    private LayerTrigger _wallInBackTrigger;
    private LayerTrigger _wallInFrontTrigger;

    private bool _backTriggered = false;
    private bool _frontTriggered = false;
    private bool _backFootTriggered = false;
    private bool _frontFootTriggered = false;

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
        _animator = GetComponent<Animator>() as Animator; 	// Get the "Animator" component and set it to "animator" var
        
        _footInBackTrigger = FootInBack.GetComponent<LayerTrigger>();
        _footInFrontTrigger = FootInFront.GetComponent<LayerTrigger>();
        _wallInBackTrigger = WallTriggerBack.GetComponent<LayerTrigger>();
        _wallInFrontTrigger = WallTriggerFront.GetComponent<LayerTrigger>();
        _dragBackup = rigidbody2D.drag;

        InitializeInputs();
        RollingImpactThreshold *= RollingImpactThreshold; // square for performance
    }

    private void InitializeInputs()
    {
        _jumpInputName = "Jump";
        _xAxisInputName = "Horizontal";
        _yAxisInputName = "Vertical";

        if (PlayerId != 1)
        {
            _jumpInputName += PlayerId;
            _xAxisInputName += PlayerId;
            _yAxisInputName += PlayerId;
        }


    }

    void Update()
    {
        UpdateStates();
        GetInput(); 			// Handles Input
        HandleDirection();
        HandleMovement(); 		// Handles Movement

        HandleJump();
        HandleWallSliding();

        UpdateAnimator();
    }

    private void UpdateStates()
    {
        // reset triggers
        _jumpingTrigger = false;
        _wallJumpingTrigger = false;
        _landingTrigger = false;
        _rollingTrigger = false;


        _backTriggered = _wallInBackTrigger.isTriggered;
        _frontTriggered = _wallInFrontTrigger.isTriggered;
        _backFootTriggered = _footInBackTrigger.isTriggered;
        _frontFootTriggered = _footInFrontTrigger.isTriggered;

        _lastFrameVelocity = _velocity;
        _velocity = rigidbody2D.velocity;

        _lastFrameGrounded = _isGrounded;
        _isGrounded = _frontFootTriggered || _backFootTriggered;
        _maxVelocity = MoveSpeed*Time.deltaTime;

    }

   private void GetInput()
   {
		_horizontalInput = Input.GetAxisRaw(_xAxisInputName); 		// Set "horiztonalInput" equal to the Horizontal Axis Input
		_verticalInput = Input.GetAxisRaw(_yAxisInputName); 			// Set "verticallInput" equal to the Vertical Axis Inpu
        _inputVector = new Vector2(_horizontalInput, _verticalInput).normalized;
   }

    private void HandleDirection()
    {
        if(_isWallSliding) return;
        
        if (_direction * _horizontalInput < 0)
        {
            Flip();
        }
    }

    private void HandleMovement()
    {
        _isCrouching = false;
        if (_isGrounded)
        {
            

            var speed = MoveSpeed;

            // player is pressing down: crouching
            if (_verticalInput < 0)
            {
                _isCrouching = true;
                speed = CrouchingSpeed;
            }

            // did not stand before, so probably landed right now
            if (!_lastFrameGrounded) // TODO does not always work!
            {
                _landingTrigger = true;
                // landing very hard? Maybe unreliable
                var forceSquared = _lastFrameVelocity.SqrMagnitude();
                Debug.Log(_lastFrameVelocity + ", "+ forceSquared);
                if (forceSquared >= RollingImpactThreshold)
                {

                    _rollingTrigger = true; // TODO move character while rolling
                }
            }
            
            
            Vector2 velocity = rigidbody2D.velocity;
            velocity.x = _horizontalInput*speed * Time.deltaTime; // actually moves gameObject
            rigidbody2D.velocity = velocity;             
        }
        else //in Air
        {
            rigidbody2D.AddForce((_verticalInput < 0 ? _inputVector : new Vector2(_horizontalInput, 0))*InAirSpeed);

            //Stop unlimmited acceleration.
            Vector2 velocity = rigidbody2D.velocity;
            if (velocity.x > _maxVelocity || velocity.x < (_maxVelocity*-1))
            {
                rigidbody2D.velocity = new Vector2(_maxVelocity*_direction, velocity.y);
            }
                
        }
    }

    private
       void HandleJump()
    {
        if (!Input.GetButtonDown(_jumpInputName)) return;
        if (_isGrounded)
        { // reset JumpCount
            _jumpCount = 0;
        }

        if (!_isWallSliding) // regular jump
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
            if (_direction * _horizontalInput < 0 && _backTriggered)
            {
                _inputVector.x *= -1;
                _inputVector.y *= 1;
            }
            _wallJumpingTrigger = true;

            StopWalllSliding();
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0); // Set the y velocity to 0
            rigidbody2D.AddForce((_inputVector + Vector2.up + DirectionVector()).normalized * WallJumpForce, ForceMode2D.Impulse); 	// Add y force set by "jumpForce" * Time.deltaTime?
        }
        
    }

    private void HandleWallSliding()
    {
        _activeSliding = false;

        // let the character slowly slide down the wall when he jumped and pressed against the wall
        if (/*_direction*_horizontalInput > 0 &&*/ _frontTriggered && !_isGrounded && !_isWallSliding)
        {
            StartWallSliding();
        }
        
        if (_isWallSliding)
        {
            OnWallSliding();
        }
    }

    // let the character slide on a wall/object
    private void StartWallSliding()
    {
        _dragBackup = rigidbody2D.drag;
        rigidbody2D.drag = WallJumpDrag;
        _isWallSliding = true;
        _activeSliding = true;
        _lastFrameSliding = true;
        Flip();
    }

    // called every frame, if the character is sliding
    private void OnWallSliding()
    {
        if ((_isWallSliding && !_backTriggered) || _verticalInput < 0)
        {
            StopWalllSliding();
            return;
        }

        _activeSliding = false;
        if (_direction*_horizontalInput < 0 && _backTriggered)
        {
            _activeSliding = true;
        }
        
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
        _isWallSliding = false;
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
            _isWallSliding = false;
            rigidbody2D.drag = _dragBackup;
        }
    }

    // inform the animator component about the new state of the character
    private void UpdateAnimator()
    {
        var velX = rigidbody2D.velocity.x;
        _animator.SetFloat("movementSpeed", Mathf.Abs(velX));
        _animator.SetBool("isWallSliding", _isWallSliding);
        _animator.SetBool("isCrouching", _isCrouching);
        _animator.SetBool("isGrounded", _isGrounded);
        
        if (_rollingTrigger)
            _animator.SetTrigger("startRolling");
        
        if (_jumpingTrigger)
            _animator.SetTrigger("jumping");
        
        if (_wallJumpingTrigger)
            _animator.SetTrigger("wallJumping");
    }


   private void Flip()
   {
       // Flip the gameObject based on localScale
       _direction *= -1;
       Vector3 scale = gameObject.transform.localScale;
       scale.x *= -1;
       gameObject.transform.localScale = scale;
 		
       //Flip Trigger
       bool temp = _backTriggered;
       _backTriggered = _frontTriggered;
       _frontTriggered = temp;

       temp = _backFootTriggered;
       _backFootTriggered = _frontFootTriggered;
       _frontFootTriggered = temp;
   }

   private Vector2 DirectionVector()
   {
       return Vector2.right * _direction;
   }
}
