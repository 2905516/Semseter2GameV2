using UnityEngine;
using UnityEngine.InputSystem;




public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;

    private float verticalRotation = 0f;
    private float savedVerticalRotation = 0f;
    private Quaternion savedBodyRotation;

    private CharacterController controller;
    private PauseMenu pauseMenu; // Cached reference to avoid static access

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        pauseMenu = FindObjectOfType<PauseMenu>();

        // Lock cursor on start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Use cached PauseMenu reference safely
        if (pauseMenu != null && pauseMenu.isPaused)
            return;

        HandleMovement();
        HandleLook();

        // Smooth FOV shift (e.g., sprint or normal)
        if (Camera.main != null)
        {
            float targetFOV = (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed) ? 70f : 60f;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime * 5f);
        }
    }

    private void HandleMovement()
    {
        if (controller == null) return;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= moveSpeed;

        if (controller.isGrounded)
        {
            velocity.y = -1f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move((move + velocity) * Time.deltaTime);
    }

    private void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //  Block look input while paused
        if (pauseMenu != null && pauseMenu.isPaused)
        {
            lookInput = Vector2.zero;
            return;
        }

        lookInput = context.ReadValue<Vector2>();
    }

    public void SetSensitivity(float newValue)
    {
        lookSensitivity = newValue;
    }

    // Save and restore rotation state (for pause freeze)
    public void SaveRotationState()
    {
        savedVerticalRotation = verticalRotation;
        savedBodyRotation = transform.rotation;
    }

    public void RestoreRotationState()
    {
        verticalRotation = savedVerticalRotation;
        transform.rotation = savedBodyRotation;

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }




}