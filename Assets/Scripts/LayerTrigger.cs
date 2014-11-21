using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
public class LayerTrigger : MonoBehaviour {

	public bool isTriggered;
	[Range(0, 31)]
	public int[] Layers;

	void OnTriggerEnter2D (Collider2D other)
	{
	    if (other.isTrigger) return;
        if (Layers.Contains(other.gameObject.layer))
        {
			// hit 
			isTriggered = true;
			
		}
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger) return;
		if (Layers.Contains(other.gameObject.layer)) {
			// hit 
			isTriggered = false;
		}
	}
}
