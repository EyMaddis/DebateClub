using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public float moveSpeed = 25f; 			// Movement Speed
	public float jumpForce = 300f;			// Jump Force
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

	// public for debugging
	public bool isGrounded = false;
	public bool isWallSliding = false;
	public bool isWallSlidingByDelay = false;

	private float horizontalInput;
	private float verticalInput;

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
	public bool abortWallSlidingDelay = false;

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

		HandleInput (); 			// Handles Input
		HandleMovement (); 		// Handles Movement
		SetIsFacingRight (); 	// Sets "isFacingRight"
		isGrounded = isOnGround ();

		HandleJump ();
		HandleWallJump ();
	}



	private void HandleInput() {
		horizontalInput = Input.GetAxisRaw("Horizontal"); 		// Set "horiztonalInput" equal to the Horizontal Axis Input
		verticalInput = Input.GetAxisRaw("Vertical"); 			// Set "verticallInput" equal to the Vertical Axis Inpu
	}

	private void HandleMovement() {

		Vector2 velocity = rigidbody2D.velocity;

		velocity.x = horizontalInput * moveSpeed * Time.deltaTime; // Moves gameObject based on the "moveSpeed" var
		rigidbody2D.velocity = velocity;
		velX = rigidbody2D.velocity.x; 			// Store the x velocity in "vel" var
		animator.SetFloat ("movementSpeed", Mathf.Abs(velX));
	}

	private void HandleWallJump () {
		activeSliding = false;

		// let the character slowly slide down the wall when he jumped and pressed against the wall
		if((isFacingRight && horizontalInput > 0) || (!isFacingRight && horizontalInput < 0)){
			if ((HasWallInFront () && !isGrounded))	{
				rigidbody2D.drag = wallJumpDrag;
				isWallSliding = true; 
				activeSliding = true;
			}
		}

		if (isWallSliding && !HasWallInBack () && !HasWallInFront ()) {
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
		bool standing = isOnGround ();
		if(standing){
			jumpCount = 0;
		}
		if(!isWallSliding && Input.GetButtonDown("Jump")) { 										// When "Jump" button is pressed
			jumpCount++;														// Add 1 to jumpCount
			if(jumpCount < maxJumps) {
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0); // Set the y velocity to 0
				rigidbody2D.AddForce(Vector2.up * jumpForce); 	// Add y force set by "jumpForce" * Time.deltaTime?
			}
		}
	}

	// flip the character and it's gameObject
	private void Flip() {
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


	private void SetIsFacingRight() {
		if(velX > 0 && !isFacingRight) { 			// If velocity is positive and gameObject isn't facing right
			Flip();
		} else if(velX < 0 && isFacingRight) { 		// If velocity is negative and gameObject is facing right
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
