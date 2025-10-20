using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class ActiveInputDebugger : MonoBehaviour
{
    private void Start()
    {
#if ENABLE_INPUT_SYSTEM
        Debug.Log($"[InputDebugger] New Input System: ENABLED ({InputSystem.settings.updateMode})");
#else
        Debug.Log("[InputDebugger] New Input System: DISABLED");
#endif

        Debug.Log($"[InputDebugger] Old Input Manager available: {Input.anyKey}");
        if (EventSystem.current != null)
            Debug.Log($"[InputDebugger] EventSystem Module: {EventSystem.current.currentInputModule.GetType().Name}");
        else
            Debug.LogWarning("[InputDebugger] No EventSystem found in scene!");
    }
}