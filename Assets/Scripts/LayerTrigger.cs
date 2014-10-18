using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class LayerTrigger : MonoBehaviour {

	public bool isTriggered;
	[Range(0, 31)]
	public int layer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.layer == layer) {
			// hit 
			isTriggered = true;
			
		}
	}

	void OnTriggerExit2D (Collider2D other){
		if (other.gameObject.layer == layer) {
			// hit 
			isTriggered = false;
		}
	}
}
