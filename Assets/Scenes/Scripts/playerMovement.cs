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

    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public float attackDamage = 1f;
    public LayerMask attacklayer;
    public GameObject hitEffect;
    bool attacking = false;
    bool readytoAttack = true;
    int attackCount;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Foootsteps")]
    private bool playingFootsteps = false;
    private float footstepSpeed = 0.5f;

    [Header("Refernces")]
    public Climbing climbingScript;
    public Camera cam;
    PlayerInput PlayerInput;
    PauseMenu PauseMenu;
    PlayerInput.OnFootActions input;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 _moveDirection;
    private Vector3 nMoveDirection;
    Vector3 moveDirection2;
    Vector3 moveDirection;

    Rigidbody rb;

    public InputActionReference move;
    bool isWalking = false;
    private Animator animator;

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
        animator = GetComponent<Animator>();

        PlayerInput = new PlayerInput();
        input = PlayerInput.OnFoot;
        //AssignInputs();

        readyToJump = true;

        startYScale = transform.localScale.y;
    }



    // Update is called once per frame
    void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
        nMoveDirection = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.y);

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

        if (PauseMenu.isPaused)
        {
            //stop animation
            StopFootsteps();

        }

        if (nMoveDirection == Vector3.zero)
        {
            //Idle

            animator.SetFloat("Move", 0);

        }
        else
        {
            //Walk

            animator.SetFloat("Move", 1);

        }

        if (input.Attack.IsPressed())
        {
            Attack();
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

            //pause jump sound effect

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

            //animator.SetBool("IsWalking", true);
            
        }


        // Mode - Sprinting
        if (Input.GetKey(sprintKey) && grounded && verticalInput > 0)
        {
            State = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;

            //play sound
        
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
        if (climbingScript.exitingWall) return;

        Vector3 move = orientation.forward * _moveDirection.y + orientation.right * _moveDirection.x;
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        bool isMoving = move.magnitude > 0.1f && horizontalVelocity.magnitude > 0.1f;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(move) * moveSpeed * 20f, ForceMode.Force);
            if (rb.linearVelocity.y > 0f)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        if (grounded)
        {

            rb.AddForce(move.normalized * moveSpeed * 10f, ForceMode.Force);

      
            

        }

        else
        { 
        
            rb.AddForce(move.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
         

        }
            
        
        //handle gravity on slopes
        if (!wallrunning)
            rb.useGravity = !OnSlope();

        if (grounded && isMoving)
        {
            if (!playingFootsteps)
            {
                //play animation
               // animator.SetFloat("Move", 1);


                //play footsteps
                StartFootsteps();

            }    
        }
        else
        {
            if (playingFootsteps)
            {
                //stop animation
               // animator.SetFloat("Move", 0);


                //stop footsteps
                StopFootsteps();


            }

        }
        // ------------------------------------------
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

    private void Attack()
    {
        //attack function
        if (!readytoAttack || attacking) return;

        readytoAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        SoundEffectManager.Play("AttackSwing");
    }

    void ResetAttack()
    {
        readytoAttack = true;
        attacking = false;
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attacklayer))
        {
            HitTarget(hit.point); 
        
        }
    
    
    }

    void HitTarget(Vector3 pos)
    {
        SoundEffectManager.Play("AttackHit");

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);

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

    void StartFootsteps()
    {
        Debug.Log("StartFootsteps()");
        playingFootsteps = true;

        // Adjust footstep rate depending on movement state
        switch (State)
        {
            case MovementState.sprinting:
                footstepSpeed = 0.3f; // faster
                break;
            case MovementState.walking:
                footstepSpeed = 0.4f; // normal
                break;
            case MovementState.crouching:
                footstepSpeed = 0.8f; // slower
                break;
            default:
                footstepSpeed = 0.4f;
                break;
        }

        // Restart footstep sound loop with new rate
        CancelInvoke(nameof(PlayFootstep));
        InvokeRepeating(nameof(PlayFootstep), 0f, footstepSpeed);

    }


    void StopFootsteps()
    {
        Debug.Log("StopFootsteps()");
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootstep));

    }

    void PlayFootstep()
    {
        // Add small pitch variation for realism
        float randomPitch = Random.Range(0.9f, 1.1f);

        // Temporarily adjust pitch on SoundEffectManager's AudioSource
        var audioManagerObj = GameObject.FindObjectOfType<SoundEffectManager>();
        if (audioManagerObj != null)
        {
            AudioSource source = audioManagerObj.GetComponent<AudioSource>();
            if (source != null)
            {
                float originalPitch = source.pitch;
                source.pitch = randomPitch;
                SoundEffectManager.Play("Footstep");
                source.pitch = originalPitch;
            }
            else
            {
                SoundEffectManager.Play("Footstep");
            }
        }
        else
        {
            SoundEffectManager.Play("Footstep");
        }

    }
    public void Quit()
    {
        Application.Quit();

    }

}
