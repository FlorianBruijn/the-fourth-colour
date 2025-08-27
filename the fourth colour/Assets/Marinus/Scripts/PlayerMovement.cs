using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Assignables
    public Transform playerCam;
    public Transform orientation;
    #endregion

    #region Components
    private Rigidbody rb;
    #endregion

    #region Look / Rotation
    private float xRotation;
    [SerializeField] private float sensitivity = 50f;
    [SerializeField] private float sensMultiplier = 1f;
    #endregion

    #region Movement
    [SerializeField] private float moveSpeed = 4500f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float counterMovement = 0.175f;
    [SerializeField] private float maxSlopeAngle = 35f;
    private const float threshold = 0.01f;

    public bool grounded;
    public LayerMask whatIsGround;
    #endregion

    #region Crouch / Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    [SerializeField] private float slideForce = 400f;
    [SerializeField] private float slideCounterMovement = 0.2f;
    #endregion

    #region Jumping
    private bool readyToJump = true;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float jumpForce = 550f;
    #endregion

    #region Input
    private float x, y;
    private bool jumping, crouching;
    #endregion

    #region Misc
    private Vector3 normalVector = Vector3.up;
    private bool cancellingGrounded;
    private float desiredX;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleInput();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Input Handling
    private void HandleInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);

        if (Input.GetKeyDown(KeyCode.LeftControl)) StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl)) StopCrouch();
    }
    #endregion

    #region Movement
    private void Move()
    {
        // Extra gravity
        rb.AddForce(Vector3.down * Time.fixedDeltaTime * 10f);

        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        CounterMovement(x, y, mag);

        if (readyToJump && jumping) Jump();

        float currentMaxSpeed = maxSpeed;

        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.fixedDeltaTime * 3000f);
            return;
        }

        // Prevent exceeding max speed
        if (x > 0 && xMag > currentMaxSpeed) x = 0;
        if (x < 0 && xMag < -currentMaxSpeed) x = 0;
        if (y > 0 && yMag > currentMaxSpeed) y = 0;
        if (y < 0 && yMag < -currentMaxSpeed) y = 0;

        float multiplier = grounded ? 1f : 0.5f;
        float multiplierV = grounded ? (crouching ? 0f : 1f) : 0.5f;

        rb.AddForce(orientation.forward * y * moveSpeed * Time.fixedDeltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.right * x * moveSpeed * Time.fixedDeltaTime * multiplier);
    }
    #endregion

    #region Jump
    private void Jump()
    {
        if (!grounded || !readyToJump) return;

        readyToJump = false;

        rb.AddForce(Vector2.up * jumpForce * 1.5f);
        rb.AddForce(normalVector * jumpForce * 0.5f);

        Vector3 vel = rb.linearVelocity;
        if (rb.linearVelocity.y < 0.5f)
            rb.linearVelocity = new Vector3(vel.x, 0, vel.z);
        else if (rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector3(vel.x, vel.y / 2, vel.z);

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump() => readyToJump = true;
    #endregion

    #region Crouch / Slide
    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position -= new Vector3(0, 0.5f, 0);

        if (rb.linearVelocity.magnitude > 0.5f && grounded)
            rb.AddForce(orientation.forward * slideForce);
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position += new Vector3(0, 0.5f, 0);
    }
    #endregion

    #region Look
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        desiredX += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.localRotation = Quaternion.Euler(0, desiredX, 0);
    }
    #endregion

    #region Counter Movement
    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.fixedDeltaTime * -rb.linearVelocity.normalized * slideCounterMovement);
            return;
        }

        if ((Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f) ||
            (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.right * Time.fixedDeltaTime * -mag.x * counterMovement);
        }

        if ((Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f) ||
            (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.forward * Time.fixedDeltaTime * -mag.y * counterMovement);
        }

        if (new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude > maxSpeed)
        {
            float yVel = rb.linearVelocity.y;
            Vector3 n = rb.linearVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(n.x, yVel, n.z);
        }
    }
    #endregion

    #region Utilities
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.linearVelocity.x, rb.linearVelocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.linearVelocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) => Vector3.Angle(Vector3.up, v) < maxSlopeAngle;
    #endregion

    #region Ground Check
    private void OnCollisionStay(Collision other)
    {
        int layer = other.gameObject.layer;
        if ((whatIsGround & (1 << layer)) == 0) return;

        foreach (ContactPoint contact in other.contacts)
        {
            if (IsFloor(contact.normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = contact.normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * 3f);
        }
    }

    private void StopGrounded() => grounded = false;
    #endregion
}
