using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public float moveSpeed = 25f; 			// Movement Speed
	public float jumpForce = 300f;			// Jump Force
	public int maxJumps = 2;
	
	public bool isFacingRight = true;
	public int groundLayerId = 8;
	public int wallLayerId = 10;

	public GameObject footLeft;
	public GameObject footRight;
	public GameObject wallTriggerLeft;
	public GameObject wallTriggerRight;

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

	private LayerTrigger footLeftLayerTrigger;
	private LayerTrigger footRightLayerTrigger;
	private LayerTrigger wallLeftLayerTrigger;
	private LayerTrigger wallRightLayerTrigger;
	
	void Start () {
		animator = GetComponent("Animator") as Animator; 	// Get the "Animator" component and set it to "animator" var
		wallLayerMask = 1 << wallLayerId;
		groundLayerMask = 1 << groundLayerId;
		footLeftLayerTrigger = footLeft.GetComponent<LayerTrigger> ();
		footRightLayerTrigger = footRight.GetComponent<LayerTrigger> ();
		wallLeftLayerTrigger = wallTriggerLeft.GetComponent<LayerTrigger> ();
		wallRightLayerTrigger = wallTriggerRight.GetComponent<LayerTrigger> ();
	}
	
	void Update () {
		velX = rigidbody2D.velocity.x; 			// Store the x velocity in "vel" var
		animator.SetFloat ("movementSpeed", Mathf.Abs(velX));

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
		velocity.x = horizontalInput * moveSpeed * Time.deltaTime; // Moves gameObject based on the "moveSpeed" var
		rigidbody2D.velocity = velocity;
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
	
	private void Flip() {
		isFacingRight = !isFacingRight; 				// Toggles between "true" and "false"
		Vector3 scale = gameObject.transform.localScale;
		scale.x *= -1;
		gameObject.transform.localScale = scale; 		// Flip the gameObject based on localScale
	}
	
	private void SetIsFacingRight() {	
		if(velX > 0 && !isFacingRight) { 			// If velocity is positive and gameObject isn't facing right
			Flip();
		} else if(velX < 0 && isFacingRight) { 		// If velocity is negative and gameObject is facing right
			Flip();
		}
	}
	
//	private void SetIsMoving() {
//		if(velX != 0) { 								// If velocity isn't 0, set "isMoving" to true
//			isMoving = true;	
//		} else { 									// If velocity is 0, set "isMoving" to false
//			isMoving = false;
//		}
//		animator.SetBool("isMoving", isMoving); 	// Set the "isMoving" bool parameter to equal "isMoving" var	
//	}


	// Trigger interaction
	public bool hasWallLeft () {
		return wallLeftLayerTrigger.isTriggered;
	}
	
	public bool hasWallRight () {
		return wallRightLayerTrigger.isTriggered;
	}
	
	public bool isSliding () {
		return hasWallLeft() || hasWallRight();
	}
	
	public bool leftFootOnGround () {
		return footLeftLayerTrigger.isTriggered;
	}
	
	public bool rightFootOnGround () {
		return footRightLayerTrigger.isTriggered;
	}
	
	public bool isOnGround () {
		return rightFootOnGround() || leftFootOnGround();
	}

}
