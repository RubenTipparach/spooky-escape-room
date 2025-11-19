using UnityEngine;

/// <summary>
/// Controls a single player's movement and interactions
/// Player 1: WASD for movement, Z/X for interactions
/// Player 2: Arrow Keys for movement, M/N for interactions
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private int playerNumber = 1; // 1 or 2
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float interactionRange = 2f;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;
    private bool isMoving = false;

    // Input mapping
    private KeyCode moveUp;
    private KeyCode moveDown;
    private KeyCode moveLeft;
    private KeyCode moveRight;
    private KeyCode interactPrimary;
    private KeyCode interactSecondary;

    public int PlayerNumber => playerNumber;
    public bool IsMoving => isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"PlayerController {playerNumber}: Rigidbody component not found!");
        }

        SetupInputMapping();
    }

    private void SetupInputMapping()
    {
        if (playerNumber == 1)
        {
            moveUp = KeyCode.W;
            moveDown = KeyCode.S;
            moveLeft = KeyCode.A;
            moveRight = KeyCode.D;
            interactPrimary = KeyCode.Z;
            interactSecondary = KeyCode.X;
            Debug.Log("Player 1 controls: WASD movement, Z/X interaction");
        }
        else if (playerNumber == 2)
        {
            moveUp = KeyCode.UpArrow;
            moveDown = KeyCode.DownArrow;
            moveLeft = KeyCode.LeftArrow;
            moveRight = KeyCode.RightArrow;
            interactPrimary = KeyCode.M;
            interactSecondary = KeyCode.N;
            Debug.Log("Player 2 controls: Arrow Keys movement, M/N interaction");
        }
        else
        {
            Debug.LogError($"Invalid player number: {playerNumber}");
        }
    }

    private void Update()
    {
        HandleMovementInput();
        HandleInteractionInput();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            ApplyMovement();
        }
    }

    private void HandleMovementInput()
    {
        float horizontal = 0;
        float vertical = 0;

        if (Input.GetKey(moveUp))
            vertical += 1;
        if (Input.GetKey(moveDown))
            vertical -= 1;
        if (Input.GetKey(moveLeft))
            horizontal -= 1;
        if (Input.GetKey(moveRight))
            horizontal += 1;

        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        isMoving = moveDirection.magnitude > 0.1f;
    }

    private void ApplyMovement()
    {
        if (isMoving)
        {
            Vector3 velocity = moveDirection * moveSpeed;
            velocity.y = rb.linearVelocity.y; // Preserve gravity
            rb.linearVelocity = velocity;

            // Rotate player to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Stop movement but preserve gravity
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactPrimary))
        {
            TryInteract(0); // Primary interaction
        }

        if (Input.GetKeyDown(interactSecondary))
        {
            TryInteract(1); // Secondary interaction
        }
    }

    private void TryInteract(int interactionType)
    {
        // Raycast to find interactable objects
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up;
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, interactionRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if (interactionType == 0)
                {
                    interactable.OnInteract(this);
                }
                else
                {
                    interactable.OnSecondaryInteract(this);
                }
                Debug.Log($"Player {playerNumber} interacted with {hit.collider.name}");
            }
        }
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetInteractionRange(float range)
    {
        interactionRange = range;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * interactionRange);
    }
#endif
}
