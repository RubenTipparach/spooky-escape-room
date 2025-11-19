using UnityEngine;

/// <summary>
/// Manages split-screen camera setup for 2 players
/// Handles camera positioning and viewport configuration
/// </summary>
public class SplitScreenCameraManager : MonoBehaviour
{
    [SerializeField] private Camera player1Camera;
    [SerializeField] private Camera player2Camera;

    [SerializeField] private Transform player1Target;
    [SerializeField] private Transform player2Target;

    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 5, -8);
    [SerializeField] private float cameraFollowSpeed = 5f;
    [SerializeField] private bool lookAtTarget = true;

    private void Start()
    {
        if (player1Camera == null || player2Camera == null)
        {
            Debug.LogError("SplitScreenCameraManager: Cameras not assigned!");
            return;
        }

    }

    private void LateUpdate()
    {
        if (player1Target != null && player1Camera != null)
        {
            UpdateCameraPosition(player1Camera, player1Target);
        }

        if (player2Target != null && player2Camera != null)
        {
            UpdateCameraPosition(player2Camera, player2Target);
        }
    }

    private void UpdateCameraPosition(Camera camera, Transform target)
    {
        Vector3 targetPosition = target.position + cameraOffset;
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);

        if (lookAtTarget)
        {
            camera.transform.LookAt(target.position + Vector3.up * 1.5f);
        }
    }

    public void SetPlayer1Target(Transform target)
    {
        player1Target = target;
    }

    public void SetPlayer2Target(Transform target)
    {
        player2Target = target;
    }

    public void SetCameraOffset(Vector3 offset)
    {
        cameraOffset = offset;
    }

    public void SetCameraFollowSpeed(float speed)
    {
        cameraFollowSpeed = speed;
    }
}
