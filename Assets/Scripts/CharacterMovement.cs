using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    [Range(1,2)]
    public int PlayerId = 1;
    public float MoveSpeed = 25f; 			// Movement Speed
    public float InAirSpeed = 10f; 			// Movement Speed
    public float JumpForce = 300f;			// Jump Force
    public float WallJumpForce = 3000f;			// Jump Force
    public int MaxJumps = 2;

    public float WallJumpDrag = 100.0f;

    [Range(0.0f, 2f)]
    public float WallSlideEndDelay = 0.3f;

    public bool IsFacingRight = true;
    public int GroundLayerId = 8;
    public int WallLayerId = 10;

    public GameObject FootInFront;
    public GameObject FootInBack;
    public GameObject WallTriggerBack;
    public GameObject WallTriggerFront;

    //Input Names
    private string _jumpInputName;
    private string _xAxisInputName;
    private string _yAxisInputName;

    // public for debugging
    [Header("Debugging")]
    public bool _isGrounded = false;
    public bool _isWallSliding = false;
    public bool _isWallSlidingByDelay = false;

    private float _horizontalInput;
    private float _verticalInput;
    private Vector2 _inputVector;

    private bool _isMoving = false;
    private bool _hasWallJumped = false;
    public int _jumpCount = 0;

    private int _direction = 1;
    private Animator _animator;
    private int _groundLayerMask; // ground layer
    private int _wallLayerMask; // ground layer    

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
    private bool _activeSliding = false; // is sliding in current frame
    private bool _abortWallSlidingDelay = false;


    void Start()
    {
        _animator = GetComponent("Animator") as Animator; 	// Get the "Animator" component and set it to "animator" var
        _wallLayerMask = 1 << WallLayerId;
        _groundLayerMask = 1 << GroundLayerId;
        _footInBackTrigger = FootInBack.GetComponent<LayerTrigger>();
        _footInFrontTrigger = FootInFront.GetComponent<LayerTrigger>();
        _wallInBackTrigger = WallTriggerBack.GetComponent<LayerTrigger>();
        _wallInFrontTrigger = WallTriggerFront.GetComponent<LayerTrigger>();
        _dragBackup = rigidbody2D.drag;

        InitializeInputs();
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
        _backTriggered = _wallInBackTrigger.isTriggered;
        _frontTriggered = _wallInFrontTrigger.isTriggered;
        _backFootTriggered = _footInBackTrigger.isTriggered;
        _frontFootTriggered = _footInFrontTrigger.isTriggered;
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
        
        if (_direction != (int)_horizontalInput && (int) _horizontalInput != 0)
        {
            Flip();
        }
    }

    private void HandleMovement()
    {

        if (_isGrounded)
        {
            Vector2 velocity = rigidbody2D.velocity;
            velocity.x = _horizontalInput*MoveSpeed*Time.deltaTime; // Moves gameObject based on the "moveSpeed" var
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

        if (!_isWallSliding)
        {
            
            if (_jumpCount < MaxJumps)
            {
                _jumpCount++;
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0); // Set the y velocity to 0
                rigidbody2D.AddForce((_inputVector + Vector2.up).normalized * JumpForce, ForceMode2D.Impulse); 	// Add y force set by "jumpForce" * Time.deltaTime?                
            } 
           
        }
        else
        {
            if (_direction * _horizontalInput < 0 && _backTriggered)
            {
                _inputVector.x *= -1;
                _inputVector.y *= 1;
            }

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

    private void StartWallSliding()
    {
        _dragBackup = rigidbody2D.drag;
        rigidbody2D.drag = WallJumpDrag;
        _isWallSliding = true;
        _activeSliding = true;
        _lastFrameSliding = true;
        Flip();
    }

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

    private void StopWalllSliding()
    {
        _abortWallSlidingDelay = true;
        _isWallSlidingByDelay = false;
        _activeSliding = false;
        _isWallSliding = false;
        rigidbody2D.drag = _dragBackup;
    }

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

    private void UpdateAnimator()
    {
        var velX = rigidbody2D.velocity.x;
        _animator.SetFloat("movementSpeed", Mathf.Abs(velX));
        _animator.SetBool("isWallSliding", _isWallSliding);
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
