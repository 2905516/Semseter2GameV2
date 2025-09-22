using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
       // playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
/*
        // Enable the input actions
        onFoot.Enable();
        // Subscribe to the input events
        onFoot.Jump.performed += ctx => Debug.Log("Jump performed");
        onFoot.Move.performed += ctx => Debug.Log("Move performed: " + ctx.ReadValue<Vector2>());
        onFoot.Shoot.performed += ctx => Debug.Log("Shoot performed"); */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // Enable the input actions when the script is enabled
        onFoot.Enable();
    }

    private void OnDisable()
    {
        // Enable the input actions when the script is enabled
        onFoot.Disable();
    }
}

