using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float smoothTime = 0f;
    private Vector3 camVelocity = Vector3.zero;
    private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    void Awake() { cam = GetComponent<Camera>(); }

    /* Camera follows where the player goes */
    void LateUpdate()
    {
        // LateUpdate moves camera after player moves
        if (target == null) return;

        // Moves toward final position rather than snapping to it
        Vector3 finalPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        Vector3 smoothMv = Vector3.SmoothDamp(transform.position, finalPos, ref camVelocity, smoothTime);

        transform.position = ClampToBounds(smoothMv);
    }
    
    /* Stop the camera from going out of bounds */
    private Vector3 ClampToBounds(Vector3 pos)
    {
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        float clampedX = Mathf.Clamp(pos.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(pos.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
        return new Vector3(clampedX, clampedY, transform.position.z);
    }

    /* Jumps the camera straight to the target with no smoothing */
    public void SnapToTarget()
    {
        if (target == null) return;
        
        Vector3 finalPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = ClampToBounds(finalPos);
        camVelocity = Vector3.zero;
    }

    /* Set bounds for new room */
    public void SetBounds(Vector2 newMinBounds, Vector2 newMaxBounds)
    {
        minBounds = newMinBounds;
        maxBounds = newMaxBounds;
    }
}
