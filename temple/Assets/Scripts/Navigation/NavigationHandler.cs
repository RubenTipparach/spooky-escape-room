using UnityEngine;
using System.Collections;

/// <summary>
/// Handles player movement and camera transitions when navigating between nodes
/// Smoothly lerps player position through intermediate nodes and slerps camera rotation
/// </summary>
public class NavigationHandler : MonoBehaviour
{
    [SerializeField] private float positionLerpSpeed = 2f; // Speed of position interpolation
    [SerializeField] private float rotationSlerpSpeed = 2f; // Speed of rotation interpolation
    [SerializeField] private float nodeArrivalDistance = 0.5f; // Distance to consider a node reached

    private bool isNavigating = false;

    public bool IsNavigating => isNavigating;

    /// <summary>
    /// Navigate a player to a destination node through optional intermediate nodes
    /// </summary>
    public void NavigateToNode(Transform playerTransform, NavigationNode destinationNode, System.Collections.Generic.List<NavigationNode> intermediateNodes = null)
    {
        if (isNavigating)
        {
            Debug.LogWarning("Navigation already in progress");
            return;
        }

        StartCoroutine(NavigationCoroutine(playerTransform, destinationNode, intermediateNodes));
    }

    private IEnumerator NavigationCoroutine(Transform playerTransform, NavigationNode destinationNode, System.Collections.Generic.List<NavigationNode> intermediateNodes)
    {
        isNavigating = true;

        // Build the path: current position -> intermediate nodes -> destination
        System.Collections.Generic.List<Vector3> path = new System.Collections.Generic.List<Vector3>
        {
            playerTransform.position
        };

        if (intermediateNodes != null && intermediateNodes.Count > 0)
        {
            foreach (var intermediateNode in intermediateNodes)
            {
                if (intermediateNode != null)
                {
                    path.Add(intermediateNode.transform.position);
                }
            }
        }

        path.Add(destinationNode.transform.position);

        // Traverse the path
        for (int i = 1; i < path.Count; i++)
        {
            yield return StartCoroutine(LerpToPosition(playerTransform, path[i - 1], path[i]));
        }

        // Once at destination, slerp the camera to the final rotation
        yield return StartCoroutine(SlerpCameraRotation(playerTransform, destinationNode.CameraLookRotation));

        isNavigating = false;
    }

    /// <summary>
    /// Lerp player from one position to another
    /// </summary>
    private IEnumerator LerpToPosition(Transform playerTransform, Vector3 startPos, Vector3 targetPos)
    {
        float distance = Vector3.Distance(startPos, targetPos);
        float elapsedTime = 0f;

        while (elapsedTime < distance / positionLerpSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / (distance / positionLerpSpeed));

            playerTransform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Ensure we reach the exact target
        playerTransform.position = targetPos;
    }

    /// <summary>
    /// Slerp camera rotation to face the specified direction (in euler angles)
    /// </summary>
    private IEnumerator SlerpCameraRotation(Transform playerTransform, Vector3 targetEulerAngles)
    {
        // Find the camera in the player's children
        Camera playerCamera = playerTransform.GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            yield break;
        }

        Quaternion startRotation = playerCamera.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
        float elapsedTime = 0f;
        float duration = 1f / rotationSlerpSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            playerCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        // Ensure we reach the exact target rotation
        playerCamera.transform.rotation = targetRotation;
    }

    public void SetPositionLerpSpeed(float speed)
    {
        positionLerpSpeed = speed;
    }

    public void SetRotationSlerpSpeed(float speed)
    {
        rotationSlerpSpeed = speed;
    }

    public void SetNodeArrivalDistance(float distance)
    {
        nodeArrivalDistance = distance;
    }
}
