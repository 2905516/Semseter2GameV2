using UnityEngine;

public class Sliding : MonoBehaviour
{

    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private playerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime; // Maximum time the player can slide
    public float slideForce; // Force applied during sliding
    private float slideTimer; // Timer to track sliding duration

    public float slideYScale; // Y scale of the player during sliding
    private float startYScale; // Original Y scale of the player

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<playerMovement>();

        startYScale = playerObj.localScale.y; // Store the original Y scale of the player
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if (Input.GetKeyUp(slideKey) && pm.sliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
        {
            SlidingMovement();
           
        }
    }
    private void StartSlide()
    {
        pm.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z); // Change the player's Y scale for sliding
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime; // Reset the slide timer
    }
    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // Calculate the input direction based on orientation

        //sliding normal
        if (!pm.OnSlope() || rb.linearVelocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force); // Apply force in the input direction]

            slideTimer -= Time.fixedDeltaTime; // Decrease the slide timer


        }

        //sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);


        }



        // Check if the slide timer has reached zero
        if (slideTimer <= 0)
        {
            StopSlide(); // Stop sliding when the timer runs out
        }
    }


    private void StopSlide()
    {
        pm.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z); // Change the player's Y scale back

    }

}
