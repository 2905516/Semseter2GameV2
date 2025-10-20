using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public static class InputHelper
{
    public static bool KeyPressed(KeyCode key)
    {
        // Old input system
        if (Input.GetKeyDown(key))
            return true;

#if ENABLE_INPUT_SYSTEM
        // New Input System (if active)
        var keyboard = Keyboard.current;
        if (keyboard == null) return false;

        switch (key)
        {
            case KeyCode.P: return keyboard.pKey.wasPressedThisFrame;
            case KeyCode.U: return keyboard.uKey.wasPressedThisFrame;
            case KeyCode.Escape: return keyboard.escapeKey.wasPressedThisFrame;
            case KeyCode.Space: return keyboard.spaceKey.wasPressedThisFrame;
            default: return false;
        }
#endif
        return false;
    }
}
