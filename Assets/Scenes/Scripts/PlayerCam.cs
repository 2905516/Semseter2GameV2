using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX ; // Sensitivity for the X-axis rotation
    public float sensY; // Sensitivity for the Y-axis rotation

    public Transform orientation; // Reference to the orientation transform 

    float xRotation; // Current rotation around the X-axis
    float yRotation; // Current rotation around the Y-axis

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false;                                      
    }

    // Update is called once per frame
    private void Update()
    {
        //get mouse input
   
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the X rotation to prevent flipping

        // Apply the rotations to the camera and orientation
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f); // Apply X and Y rotation to the camera
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f); // Apply only Y rotation to the orientation
    }

 
}
