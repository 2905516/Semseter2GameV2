using System.Collections;
using 
    UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallrunSpeed;
    public float climbSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Key binds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Refernces")]
    public Climbing climbingScript;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 _moveDirection;
    Vector3 moveDirection2;
    Vector3 moveDirection;

    Rigidbody rb;

    public InputActionReference move;

    public MovementState State;
    public enum MovementState
    {
        walking,
        crouching,
        sprinting,
        wallrunning,
        sliding,
        climbing,
        air
    }

    public bool sliding;
    public bool crouching;
    public bool wallrunning;
    public bool climbing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();

        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        MyInput();
        speedControl();
        StateHandler();
        //handle drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump 
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //start crouching
        if (Input.GetKeyDown(crouchKey) && grounded)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // push down to prevent clipping through the ground

        }

        //stop crouching
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        }
    }

    private void StateHandler()
    {
        //Mode - Climbing
        if (climbing)
        {

            State = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;


        }
        //Mode - WallRunning
        else if (wallrunning)
        {
            State = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;

        }

        //Mode - Sliding
        if (sliding)
        {
            State = MovementState.sliding;

            if (OnSlope() && rb.linearVelocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;

            }
            else
            {
                desiredMoveSpeed = sprintSpeed;

            }

        }

        //Mode - Crounching
        else if (Input.GetKey(crouchKey) && grounded)
        {
            State = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }


        // Mode - Sprinting
        if (Input.GetKey(sprintKey) && grounded && verticalInput > 0)
        {
            State = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        // Mode - Walking
        else if (grounded)
        {
            State = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        //Mode - Air
        else
        {
            State = MovementState.air;

        }

        //check if desiredMoveSpeed has changed drastically
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 8f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;

        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        //smoothly lerp movement speed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {

        if (climbingScript.exitingWall)
        {
            return;

        }
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection2 = orientation.forward * _moveDirection.y + orientation.right * _moveDirection.x;


        // check if player is on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            if (rb.linearVelocity.y > 0f)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force); // prevent sliding up slopes
            }
        }

        if (grounded)
        {
            // move the player on ground
            //Version 1
            //rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            //Version 2
            // Vector2 direction = moveAction.ReadValue<Vector2>();
            //transform.position += new Vector3(_moveDirection.x, 0, _moveDirection.y) * moveSpeed * Time.deltaTime;

            //Version 3
            rb.AddForce(moveDirection2.normalized * moveSpeed * 10f, ForceMode.Force);

        }
        else if (!grounded)
        {
            // move the player in air
            rb.AddForce(moveDirection2.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //turn off gravity while standing on slope
        if (!wallrunning) rb.useGravity = !OnSlope();



    }

    private void speedControl()
    {
        // limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed; // limit speed on slope
            }
            return; // no need to limit speed on slope
        }

        //limit velocity if player is on ground or in air
        else
        {
            // limit velocity if needed
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }


        }


    }

    private void Jump()
    {
        exitingSlope = true;
        //reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        // reset jump
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;



    }
    public void Quit()
    {
        Application.Quit();

    }

}
