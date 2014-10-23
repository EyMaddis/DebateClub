using System;
using UnityEngine;
using System.Collections;

public class TwoPlayerCamera2D : MonoBehaviour
{
    
    public Transform Player1;
    public Transform Player2;

    [Header("Smoothing")]
    public float SmoothTime = 0.2f;
    public float SmoothZoomTime = 0.2f;

    [Header("Border")] 
    public float MinCameraX = -10f;
    public float MaxCameraX = 10f;
    public float MinCameraY = 10f;
    public float MaxCameraY = 10f;

    [Header("Automatic Zoom Controls")] 
    [Range(0.001f, 2.0f)] 
    public float SizeMultiplier = 0.5f;
    [Range(1.0f, 10.0f)]
    public float MinZoom = 1.5f;

    [Range(1.0f, 10.0f)]
    public float MaxZoom = 5.0f;

    private Camera _camera;
    private Vector3 _velocity = Vector3.zero;
    private float _sizeVelocity = 0;

	// Use this for initialization
	void Start ()
	{
	    _camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        // center camera
	    var pos1 = DeleteZDimension(Player2.position);
        var pos2 = DeleteZDimension(Player1.position);
	    var distance = pos2 - pos1;
	    var center = pos1 + distance*0.5f;

        // enforce borders
	    center.x = Math.Min(center.x, MaxCameraX);
	    center.x = Math.Max(center.x, MinCameraX);
	    center.y = Math.Min(center.y, MaxCameraY);
	    center.y = Math.Max(center.y, MinCameraX);


        // smoothly set new camera position
	    var newPosition = new Vector3(center.x, center.y, _camera.transform.position.z);
	    transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, SmoothTime);

        // set new orthographic size based on the distance between the players
        // TODO: check if players are visibile
	    var newSize = distance.magnitude*SizeMultiplier;
        
        newSize = Math.Max(newSize, MinZoom);
	    newSize = Math.Min(newSize, MaxZoom);
	    _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, newSize, ref _sizeVelocity, SmoothZoomTime);

	}

    private Vector2 DeleteZDimension(Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }
}
