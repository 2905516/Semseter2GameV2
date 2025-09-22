using UnityEngine;

public class WallRunning : MonoBehaviour
{

    [Header("Wallrunning")]
    public LayerMask whatIsWall; // Layer mask for walls
    public LayerMask whatIsGround; // Layer mask for ground
    public float wallRunForce; // Force applied during wall running
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed; // Speed of climbing during wall run
    public float maxWallRunTime; // Maximum time the player can wall run
    private float wallRunTimer; // Timer to track wall running duration

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space; //jump key
    public KeyCode upwardsRunKey = KeyCode.LeftShift; // Key to initiate wall run 
    public KeyCode downwardsRunKey = KeyCode.LeftControl; // Key to stop wall run
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance; // Distance to check for walls
    public float minJumpHeight; // Minimum height to initiate wall run
    private RaycastHit leftWallHit; // Raycast hit for the left wall
    private RaycastHit rightWallHit; // Raycast hit for the right wall
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    private Rigidbody rb;
    private playerMovement pm;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        StateMachine(); // Handle state transitions based on inputs and conditions
    }

    private void FixedUpdate()
    {
        
        if (pm.wallrunning) // If the player is wall running
        {
            WallRunningMovement(); // Apply wall running movement
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    { 
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);

    }

    private void StateMachine()
    {
        //Getting Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey); // Check if the upward wall run key is pressed
        downwardsRunning = Input.GetKey(downwardsRunKey); // Check if the downward wall run key is pressed

        //State 1 - Wall Running
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pm.wallrunning) // If not already wall running
            {
                StartWallRun(); // Start wall running
            }

            //wallrun Timer
            if (wallRunTimer > 0)
            { 
            
                wallRunTimer -= Time.deltaTime;
            
            
            
            }

            if (wallRunTimer <= 0 && pm.wallrunning)
            { 
            
            
                exitingWall = true;
                exitWallTimer = exitWallTime;
            
            
            
            }
            
            //wall jump
            if (Input.GetKeyDown(jumpKey))
            {
                WallJump();



            }

        }
        //State 3 - Exit Wall
        else if (exitingWall)
        {
            if (pm.wallrunning)
            {
            
                StopWallRun();


            }

            if (exitWallTimer > 0)
            { 
                exitWallTimer -= Time.deltaTime;
            
            
            }

            if (exitWallTimer <= 0)
            { 
            
                exitingWall = false;
            
            
            }
        
        
        }
        //State 3 - None
        else
        {
            if (pm.wallrunning) // If currently wall running
            {
                StopWallRun(); // Stop wall running
            }
        }

    }

    private void StartWallRun()
    {
        pm.wallrunning = true; // Set the player's state to wall running
       
        wallRunTimer = maxWallRunTime;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

    }

    private void WallRunningMovement()
    { 
        rb.useGravity = useGravity; // Disable gravity during wall run

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal; // Get the wall normal based on which wall is hit

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up); // Calculate the forward direction along the wall

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward; // Flip the direction if the opposite wall is closer 

        }

        // Apply force to the player in the direction of the wall
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);


        // upwards/downwards wall run
        if (upwardsRunning)
        { 
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbSpeed, rb.linearVelocity.z); // Set upward velocity

        }

        if (downwardsRunning)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallClimbSpeed, rb.linearVelocity.z); // Set upward velocity

        }

        //push player towards wall
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            // If the player is not pressing towards the wall, apply a force to push them towards it
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        //weaken gravity
        if (useGravity)
        {
            rb.AddForce  (transform.up * gravityCounterForce, ForceMode.Force);

        }
    }

    private void StopWallRun()
    {
        pm.wallrunning = false; // Set the player's state to not wall running
    }
    private void WallJump()
    {
        //enter exiting wall
        exitingWall = true;
        exitWallTimer = exitWallTime;
        
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal; // Get the wall normal based on which wall is hit

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //reset y velocity add force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
