using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PauseInputDebugger : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Debug.Log("[PauseInputDebugger] OLD Input system saw P");

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
            Debug.Log("[PauseInputDebugger] NEW Input System saw P");
#endif
    }
}