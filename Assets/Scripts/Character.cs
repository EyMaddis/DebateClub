using System.Diagnostics;
using UnityEngine;
using System.Collections;
using Debug = UnityEngine.Debug;

public class Character : MonoBehaviour {

	public float moveSpeed = 25f; 			// Movement Speed
	public float inAirSpeed = 10f; 			// Movement Speed
	public float jumpForce = 300f;			// Jump Force
	public float wallJumpForce = 3000f;			// Jump Force
	public int maxJumps = 2;

	public float wallJumpDrag = 100.0f;

	[Range(0.0f,2f)]
	public float wallSlideEndDelay = 0.3f;

	public bool isFacingRight = true;
	public int groundLayerId = 8;
	public int wallLayerId = 10;

	public GameObject footInFront;
	public GameObject footInBack;
	public GameObject wallTriggerBack;
	public GameObject wallTriggerFront;

    //Buttons
    public string jumpInput = "Jump";

	// public for debugging
	public bool isGrounded = false;
	public bool isWallSliding = false;
	public bool isWallSlidingByDelay = false;
    public bool wallFront = false;
    public bool wallBack = false;
  

	private float horizontalInput;
	private float verticalInput;
	private Vector2 inputVector;

	private bool isMoving = false;
	private bool hasWallJumped = false;
	public int jumpCount = 0;

	private float velX;
	private Animator animator;
	private int groundLayerMask; // ground layer
	private int wallLayerMask; // ground layer

	private LayerTrigger footInBackTrigger;
	private LayerTrigger footInFrontTrigger;
	private LayerTrigger wallInBackTrigger;
	private LayerTrigger wallInFrontTrigger;

	private float dragBackup;
	private bool lastFrameSliding = false;
	private bool activeSliding = false; // is sliding in current frame
	private bool abortWallSlidingDelay = false;


	void Start () {
		animator = GetComponent("Animator") as Animator; 	// Get the "Animator" component and set it to "animator" var
		wallLayerMask = 1 << wallLayerId;
		groundLayerMask = 1 << groundLayerId;
		footInBackTrigger = footInBack.GetComponent<LayerTrigger> ();
		footInFrontTrigger = footInFront.GetComponent<LayerTrigger> ();
		wallInBackTrigger = wallTriggerBack.GetComponent<LayerTrigger> ();
		wallInFrontTrigger = wallTriggerFront.GetComponent<LayerTrigger> ();

		dragBackup = rigidbody2D.drag;
	}

	void Update () {

        isGrounded = isOnGround ();
		HandleInput (); 			// Handles Input
		HandleMovement (); 		// Handles Movement
		SetIsFacingRight (); 	// Sets "isFacingRight"
		

		HandleJump ();
		HandleWallSliding ();

		UpdateAnimator ();

	    Debugger();
	}


   
    private void Debugger()
    {
        wallBack = HasWallInBack();
        wallFront = HasWallInFront();
    }


	private void HandleInput() {
		horizontalInput = Input.GetAxisRaw("Horizontal"); 		// Set "horiztonalInput" equal to the Horizontal Axis Input
		verticalInput = Input.GetAxisRaw("Vertical"); 			// Set "verticallInput" equal to the Vertical Axis Inpu
        inputVector = new Vector2(horizontalInput, verticalInput).normalized;
	}

	private void HandleMovement()
    {
	    if (isGrounded)
	    {
	        Vector2 velocity = rigidbody2D.velocity;
	        velocity.x = horizontalInput*moveSpeed*Time.deltaTime; // Moves gameObject based on the "moveSpeed" var
	        rigidbody2D.velocity = velocity;
	        velX = rigidbody2D.velocity.x; // Store the x velocity in "vel" var

	    }
	    else //in Air
	    {   
	        rigidbody2D.AddForce((verticalInput < 0 ? inputVector : new Vector2(horizontalInput, 0))*inAirSpeed);

	        velX = rigidbody2D.velocity.x; 			// Store the x velocity in "vel" var
	    }
	}

	private void HandleWallSliding () {
		activeSliding = false;        

		// let the character slowly slide down the wall when he jumped and pressed against the wall
		if((isFacingRight && horizontalInput > 0) || (!isFacingRight && horizontalInput < 0)){
			if ((HasWallInFront () && !isGrounded))	{
				rigidbody2D.drag = wallJumpDrag;
				isWallSliding = true; 
				activeSliding = true;
			}
		}

		if ((isWallSliding && !HasWallInBack () && !HasWallInFront ()) || verticalInput < 0) {
			AbortWallSliding();
		}
		else if (lastFrameSliding && !activeSliding) { // character stopped sliding
			StartCoroutine("EndSlidingWithDelay");
		}

		lastFrameSliding = activeSliding;
	}

	IEnumerator EndSlidingWithDelay(){
		isWallSlidingByDelay = true;
		yield return new WaitForSeconds (wallSlideEndDelay);

		if (abortWallSlidingDelay) {
			abortWallSlidingDelay = false;
		} else {
			isWallSlidingByDelay = false;
			isWallSliding = false;
			rigidbody2D.drag = dragBackup;
		}
	}

	private void AbortWallSliding(){
		abortWallSlidingDelay = true;
		isWallSlidingByDelay = false;
		activeSliding = false;
		isWallSliding = false;
		rigidbody2D.drag = dragBackup;
	}



	private void HandleJump() {
		if(!Input.GetButtonDown(jumpInput)) return;
        if (isOnGround())
        {
            jumpCount = 0;
        }	

		if(!isWallSliding) { 						// When "Jump" button is pressed         
            Debug.Log("Ground Jump");
           											
			if(jumpCount < maxJumps) {
                jumpCount++;
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0); // Set the y velocity to 0
                rigidbody2D.AddForce((inputVector + Vector2.up).normalized * jumpForce, ForceMode2D.Impulse); 	// Add y force set by "jumpForce" * Time.deltaTime?                
			}
		}
		else
		{
		    if (Direction()*horizontalInput > 0 && wallInFrontTrigger.isTriggered)
		    {
		        inputVector.x *= -1;
		        inputVector.y *= 1;
		    }

            Debug.Log("WallJump");
            AbortWallSliding();
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0); // Set the y velocity to 0
            rigidbody2D.AddForce((inputVector + Vector2.up + DirectionVector()*-1).normalized * wallJumpForce, ForceMode2D.Impulse); 	// Add y force set by "jumpForce" * Time.deltaTime?
		}
	}

	private void UpdateAnimator() {
		animator.SetFloat("movementSpeed", Mathf.Abs(velX));
		animator.SetBool("isWallSliding", isWallSliding);
	}

	// flip the character and it's gameObject
	private void Flip() {
        if(isWallSliding) return;
		isFacingRight = !isFacingRight; 				// Toggles between "true" and "false"
		Vector3 scale = gameObject.transform.localScale;
		scale.x *= -1;
		gameObject.transform.localScale = scale; 		// Flip the gameObject based on localScale
	}

	// returns -1 for left, 1 for right if facingRight=true
	private int Direction() {
		if (isFacingRight) {
			return 1;
		} else {
			return -1;
		}
	}

    private Vector2 DirectionVector()
    {
        return Vector2.right*Direction();
    }


	private void SetIsFacingRight() {
		if(horizontalInput > 0 && !isFacingRight) { 			// If velocity is positive and gameObject isn't facing right
			Flip();
		} else if(horizontalInput < 0 && isFacingRight) { 		// If velocity is negative and gameObject is facing right
			Flip();
		}
	}

	// Trigger interaction
	public bool HasWallInBack () {
		return wallInBackTrigger.isTriggered; 
	}

	public bool HasWallInFront () {
		return wallInFrontTrigger.isTriggered;
	}


	public bool BackFootOnGround () {
		return footInBackTrigger.isTriggered;
	}

	public bool FrontFootOnGround () {
		return footInFrontTrigger.isTriggered;
	}

	public bool isOnGround () {
		return FrontFootOnGround() || BackFootOnGround();
	}

}
