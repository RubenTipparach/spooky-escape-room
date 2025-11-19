using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a door that can be opened and closed
/// Opens for a duration with configurable rotation and animation times
/// </summary>
public class Door : InteractableObject
{
    [SerializeField] private float openDuration = 10f; // How long door stays open
    [SerializeField] private float openDegrees = 90f; // How many degrees to rotate when opening
    [SerializeField] private float openDurationSeconds = 0.5f; // Time to fully open
    [SerializeField] private float closeDurationSeconds = 0.5f; // Time to fully close
    [SerializeField] private Vector3 openAxis = Vector3.up; // Axis to rotate around

    private Quaternion initialRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private Coroutine doorCoroutine;

    private void Awake()
    {
        initialRotation = transform.rotation;
        openRotation = initialRotation * Quaternion.AngleAxis(openDegrees, openAxis);
    }

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        if (!isOpen)
        {
            OpenDoor();
        }
    }

    public override void OnSecondaryInteract(PlayerController player)
    {
        base.OnSecondaryInteract(player);

        if (isOpen)
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        // Stop existing coroutine if running
        if (doorCoroutine != null)
        {
            StopCoroutine(doorCoroutine);
        }

        doorCoroutine = StartCoroutine(DoorSequence());
    }

    public void CloseDoor()
    {
        if (!isOpen) return;

        // Stop existing coroutine if running
        if (doorCoroutine != null)
        {
            StopCoroutine(doorCoroutine);
        }

        StartCoroutine(CloseAnimation());
    }

    private IEnumerator DoorSequence()
    {
        // Animate opening
        yield return StartCoroutine(OpenAnimation());

        // Stay open for duration
        yield return new WaitForSeconds(openDuration);

        // Animate closing
        yield return StartCoroutine(CloseAnimation());
    }

    private IEnumerator OpenAnimation()
    {
        isOpen = true;
        float elapsed = 0f;

        while (elapsed < openDurationSeconds)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / openDurationSeconds);

            transform.rotation = Quaternion.Slerp(initialRotation, openRotation, progress);
            yield return null;
        }

        transform.rotation = openRotation;
        Debug.Log($"Door {gameObject.name} opened");
    }

    private IEnumerator CloseAnimation()
    {
        float elapsed = 0f;

        while (elapsed < closeDurationSeconds)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / closeDurationSeconds);

            transform.rotation = Quaternion.Slerp(openRotation, initialRotation, progress);
            yield return null;
        }

        transform.rotation = initialRotation;
        isOpen = false;
        Debug.Log($"Door {gameObject.name} closed");
    }

    public bool IsOpen => isOpen;

    public void SetOpenDuration(float duration)
    {
        openDuration = duration;
    }

    public void SetOpenDegrees(float degrees)
    {
        openDegrees = degrees;
        openRotation = initialRotation * Quaternion.AngleAxis(openDegrees, openAxis);
    }

    public void SetAnimationTimes(float openTime, float closeTime)
    {
        openDurationSeconds = openTime;
        closeDurationSeconds = closeTime;
    }

    public void SetOpenAxis(Vector3 axis)
    {
        openAxis = axis.normalized;
        openRotation = initialRotation * Quaternion.AngleAxis(openDegrees, openAxis);
    }
}
