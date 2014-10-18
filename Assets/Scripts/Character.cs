using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	public float moveSpeed = 25f; 			// Movement Speed
	public float jumpForce = 300f;			// Jump Force
	public int maxJumps = 2;

	public float groundDistance = 0.48f;

	// public for debugging
	public bool isGrounded = false;

	
	private float horizontalInput;
	private float verticalInput;
	
	private bool isFacingRight = true;
	private bool isMoving = false;
	private int jumpCount = 0;
	
	private float velX;
	private Animator animator;
	private int groundLayerMask = 1<<8; // ground layer

	private Feet feet;
	
	void Start () {
		animator = GetComponent("Animator") as Animator; 	// Get the "Animator" component and set it to "animator" var
		feet = GetComponentsInChildren<Feet> ();
	}
	
	void Update () {
		velX = rigidbody2D.velocity.x; 			// Store the x velocity in "vel" var
		
		HandleInput(); 			// Handles Input
		HandleMovement(); 		// Handles Movement
		HandleJump();
		SetIsGrounded();		// Sets "isGrounded"
		SetIsFacingRight(); 	// Sets "isFacingRight"
		SetIsMoving(); 			// Sets "isMoving"
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
		if(Input.GetButtonDown("Jump")) { 										// When "Jump" button is pressed
			jumpCount++;														// Add 1 to jumpCount
			if(isGrounded || jumpCount < maxJumps) {

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
	
	private void SetIsMoving() {
		if(velX != 0) { 								// If velocity isn't 0, set "isMoving" to true
			isMoving = true;	
		} else { 									// If velocity is 0, set "isMoving" to false
			isMoving = false;
		}
		animator.SetBool("isMoving", isMoving); 	// Set the "isMoving" bool parameter to equal "isMoving" var	
	}
	
	private void SetIsGrounded() {
		
		RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, -Vector2.up, groundDistance, groundLayerMask);	// Create a ray from the character to check for the ground layer
		if(raycastHit.collider != null) {															// If the ray hit isn't null
			//		if(raycastHit.collider.tag == "ground") {	
			//		Debug.Log ("Ground!");											// Check if the collider is in the "ground" tag
			isGrounded = true;																	// Set "isGrounded" to true
			jumpCount = 0;																		// Reset the "jumpCount" to 0
			//		}
		} else {
			isGrounded = false;																		// Set "isGrounded" to false if not colliding with "ground" tag
		}
	}
}
