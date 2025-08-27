using UnityEngine;

public class ApexFPSMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float crouchSpeed = 3f;
    public float jumpHeight = 1.5f;
    public float gravity = -15f;

    [Header("Crouch Settings")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;
    public float crouchTransitionSpeed = 5f;

    [Header("Camera Settings")]
    public Transform playerCamera;
    public float cameraStandingHeight = 1.6f;    // Typical FPS cam height when standing
    public float cameraCrouchingHeight = 0.9f;   // Lower camera height when crouched

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100f;
    public float maxLookAngle = 90f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;
    private bool isCrouching = false;

    private float xRotation = 0f; // For vertical camera rotation

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;

        // Set camera to standing height at start
        if (playerCamera != null)
            playerCamera.localPosition = new Vector3(0, cameraStandingHeight, 0);

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MouseLook();
        GroundCheck();
        HandleMovement();
        HandleJump();
        HandleCrouch();
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically with clamping
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    void GroundCheck()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // small negative value to keep grounded
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.Normalize();

        // Sprinting
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && moveZ > 0)
        {
            currentSpeed = sprintSpeed;
        }
        else if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleCrouch()
    {
        // Hold Left Ctrl to crouch, release to stand
        if (Input.GetKey(KeyCode.LeftControl))
            isCrouching = true;
        else
            isCrouching = false;

        float targetHeight = isCrouching ? crouchingHeight : standingHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

        // Adjust center as height changes so player collider stays grounded
        float centerY = controller.height / 2f;
        controller.center = new Vector3(0, centerY, 0);

        // Smoothly lower/raise the camera position as well
        if (playerCamera != null)
        {
            float targetCamHeight = isCrouching ? cameraCrouchingHeight : cameraStandingHeight;
            Vector3 camPos = playerCamera.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, targetCamHeight, crouchTransitionSpeed * Time.deltaTime);
            playerCamera.localPosition = camPos;
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }
}
