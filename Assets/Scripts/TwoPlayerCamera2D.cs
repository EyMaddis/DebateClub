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

    [Header("Camera Movement Boundary")]
    [Tooltip("Left outmost position for the camera")]
    public float MinCameraX = -10f;

    [Tooltip("Right outmost position for the camera")]
    public float MaxCameraX = 10f;

    [Tooltip("Lowest position for the camera")]
    public float MinCameraY = 10f;

    [Tooltip("Highest position for the camera")]
    public float MaxCameraY = 10f;

    [Header("Automatic Zoom Controls")] 
    [Range(0.001f, 2.0f)]
    [Tooltip("Multiplied to the distance between the characters to find the zoom level (zoom = this*distance)")]
    public float ZoomMultiplier = 0.4f;
    [Range(1.0f, 10.0f)]
    [Tooltip("What is the nearest the camera can be to the characters?")]
    public float MinZoom = 1.5f;

    [Range(1.0f, 10.0f)]
    [Tooltip("What is the furthest the camera can be to the characters?")]
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
	    var pos1 = Utils.DeleteZDimension(Player2.position);
        var pos2 = Utils.DeleteZDimension(Player1.position);
	    var distance = pos2 - pos1;
	    var center = pos1 + distance*0.5f;

        // enforce borders
	    center.x = Utils.LimitValue(center.x, MinCameraX, MaxCameraX);
	    center.y = Utils.LimitValue(center.y, MinCameraY, MaxCameraY);


        // smoothly set new camera position
	    var newPosition = new Vector3(center.x, center.y, _camera.transform.position.z);
	    transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, SmoothTime);

        // set new orthographic size based on the distance between the players
        // TODO: check if players are visibile
	    var newSize = distance.magnitude*ZoomMultiplier;
        
        newSize = Utils.LimitValue(newSize, MinZoom, MaxZoom);
	    newSize = Math.Min(newSize, MaxZoom);
	    _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, newSize, ref _sizeVelocity, SmoothZoomTime);

	}

    
}
