using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public float moveSpeed = 25f; 			// Movement Speed
	public float jumpForce = 300f;			// Jump Force
	public int maxJumps = 2;
	
	public bool isFacingRight = true;
	public int groundLayerId = 8;
	public int wallLayerId = 10;

	public GameObject footInBack;
	public GameObject footInFront;
	public GameObject wallTriggerBack;
	public GameObject wallTriggerFront;

	// public for debugging
	public bool isGrounded = false;
	
	private float horizontalInput;
	private float verticalInput;

	private bool isMoving = false;
	public int jumpCount = 0;
	
	private float velX;
	private Animator animator;
	private int groundLayerMask; // ground layer
	private int wallLayerMask; // ground layer

	private LayerTrigger footInBackTrigger;
	private LayerTrigger footInFrontTrigger;
	private LayerTrigger wallInBackTrigger;
	private LayerTrigger wallInFrontTrigger;
	
	void Start () {
		animator = GetComponent("Animator") as Animator; 	// Get the "Animator" component and set it to "animator" var
		wallLayerMask = 1 << wallLayerId;
		groundLayerMask = 1 << groundLayerId;
		footInBackTrigger = footInBack.GetComponent<LayerTrigger> ();
		footInFrontTrigger = footInFront.GetComponent<LayerTrigger> ();
		wallInBackTrigger = wallTriggerBack.GetComponent<LayerTrigger> ();
		wallInFrontTrigger = wallTriggerFront.GetComponent<LayerTrigger> ();
	}
	
	void Update () {

		HandleInput(); 			// Handles Input
		HandleMovement(); 		// Handles Movement
		HandleJump();
		SetIsFacingRight(); 	// Sets "isFacingRight"
		isGrounded = isOnGround ();
	}


	
	private void HandleInput() {
		horizontalInput = Input.GetAxisRaw("Horizontal"); 		// Set "horiztonalInput" equal to the Horizontal Axis Input
		verticalInput = Input.GetAxisRaw("Vertical"); 			// Set "verticallInput" equal to the Vertical Axis Inpu
	}
	
	private void HandleMovement() {

		Vector2 velocity = rigidbody2D.velocity;

		// don't move if the player is stuck in a wall
		if ((Direction()*velocity.x < 0 && HasWallInBack()) || (Direction()*velocity.x > 0 && HasWallInFront())) {
			return;
		}

		velocity.x = horizontalInput * moveSpeed * Time.deltaTime; // Moves gameObject based on the "moveSpeed" var
		rigidbody2D.velocity = velocity;
		velX = rigidbody2D.velocity.x; 			// Store the x velocity in "vel" var
		animator.SetFloat ("movementSpeed", Mathf.Abs(velX));
	}
	
	private void HandleJump() {
		bool standing = isOnGround ();
		if(standing){
			jumpCount = 0;
		}
		if(Input.GetButtonDown("Jump")) { 										// When "Jump" button is pressed
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
	
	public bool IsSliding () {
		return HasWallInBack() || HasWallInFront();
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
