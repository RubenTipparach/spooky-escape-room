using UnityEngine;
using UnityEngine.InputSystem;

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

    // Input System references
    private InputAction moveUpAction;
    private InputAction moveDownAction;
    private InputAction moveLeftAction;
    private InputAction moveRightAction;
    private InputAction interactPrimaryAction;
    private InputAction interactSecondaryAction;

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

    private void OnEnable()
    {
        moveUpAction?.Enable();
        moveDownAction?.Enable();
        moveLeftAction?.Enable();
        moveRightAction?.Enable();
        interactPrimaryAction?.Enable();
        interactSecondaryAction?.Enable();
    }

    private void OnDisable()
    {
        moveUpAction?.Disable();
        moveDownAction?.Disable();
        moveLeftAction?.Disable();
        moveRightAction?.Disable();
        interactPrimaryAction?.Disable();
        interactSecondaryAction?.Disable();
    }

    private void SetupInputMapping()
    {
        if (playerNumber == 1)
        {
            moveUpAction = new InputAction(binding: "<Keyboard>/w");
            moveDownAction = new InputAction(binding: "<Keyboard>/s");
            moveLeftAction = new InputAction(binding: "<Keyboard>/a");
            moveRightAction = new InputAction(binding: "<Keyboard>/d");
            interactPrimaryAction = new InputAction(binding: "<Keyboard>/z");
            interactSecondaryAction = new InputAction(binding: "<Keyboard>/x");
            Debug.Log("Player 1 controls: WASD movement, Z/X interaction");
        }
        else if (playerNumber == 2)
        {
            moveUpAction = new InputAction(binding: "<Keyboard>/upArrow");
            moveDownAction = new InputAction(binding: "<Keyboard>/downArrow");
            moveLeftAction = new InputAction(binding: "<Keyboard>/leftArrow");
            moveRightAction = new InputAction(binding: "<Keyboard>/rightArrow");
            interactPrimaryAction = new InputAction(binding: "<Keyboard>/m");
            interactSecondaryAction = new InputAction(binding: "<Keyboard>/n");
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

        if (moveUpAction.IsPressed())
            vertical += 1;
        if (moveDownAction.IsPressed())
            vertical -= 1;
        if (moveLeftAction.IsPressed())
            horizontal -= 1;
        if (moveRightAction.IsPressed())
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
        if (interactPrimaryAction.WasPressedThisFrame())
        {
            TryInteract(0); // Primary interaction
        }

        if (interactSecondaryAction.WasPressedThisFrame())
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
