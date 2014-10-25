using UnityEngine;

// based on: http://answers.unity3d.com/questions/674225/2d-camera-to-follow-two-players.html
public class TwoPlayerCamera2D : MonoBehaviour
{

    public Transform Target1;
    public Transform Target2;


    [Header("Smoothing")] 
    public float SmoothTime = 0.2f;
    public float SmoothZoomTime = 0.2f;

    [Header("Camera Movement Boundary")] 
    public Vector2 LowerBoundary = new Vector2(-10f, -10f);
    public Vector2 UpperBoundary = new Vector2(10f, 10f);
    
    [Tooltip("Offset of the players to the screen boundaries")]
    public float Offset = 5f;


    [Header("Automatic Zoom Controls")]
    [Range(0.1f, 10.0f)]
    public float minSizeY = 1f;

    [Range(0.001f, 1.0f)] 
    public float ZoomMultiplierX = 0.3f;
    
    [Range(0.001f, 1.0f)]
    public float ZoomMultiplierY = 0.5f;


    private Camera _camera;
    private Vector3 _velocity = Vector3.zero;
    private float _sizeVelocity = 0;

    // Use this for initialization
    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {

        SetCameraPos();
        SetCameraSize();
    }

    void SetCameraPos()
    {
        var center = (Target1.position + Target2.position) * 0.5f;

        // enforce borders
        center.x = Utils.LimitValue(center.x, LowerBoundary.x, UpperBoundary.x);
        center.y = Utils.LimitValue(center.y, LowerBoundary.y, UpperBoundary.y);

        var position = new Vector3(
            center.x,
            center.y,
            camera.transform.position.z
        );
        transform.position = Vector3.SmoothDamp(transform.position, position, ref _velocity, SmoothTime);
    }

    void SetCameraSize()
    {
        //horizontal size is based on actual screen ratio
        var aspectRatio = Screen.width/Screen.height;
        var minSizeX = minSizeY * aspectRatio;

        //multiplying by 0.5, because the ortographicSize is actually half the height
        var width = Mathf.Abs(Target1.position.x - Target2.position.x - Offset) * ZoomMultiplierX;
        var height = Mathf.Abs(Target1.position.y - Target2.position.y - Offset) * ZoomMultiplierY;

        //computing the size
        var camSizeX = Mathf.Max(width, minSizeX);
        var size = Mathf.Max(height, camSizeX, minSizeY);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, size, ref _sizeVelocity, SmoothZoomTime);
    }
}
