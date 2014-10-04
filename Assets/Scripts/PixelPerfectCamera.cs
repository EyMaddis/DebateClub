using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour
{
	public float pixPerUnit = 100.0f; //This can be PixelsPerUnit, or you can change it during runtime to alter the camera.
	
	void Awake ()
	{
		this.camera.orthographicSize = Screen.height * gameObject.camera.rect.height / pixPerUnit / 2.0f;//- 0.1f;
	}
}