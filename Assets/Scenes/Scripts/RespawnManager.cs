using UnityEngine;

/// <summary>
/// Stores the current respawn point GameObject. Only one RespawnManager should exist in the scene and be tagged "Respawn".
/// Checkpoints will update respawnPoint. Other traps should NOT change this.
/// </summary>
public class RespawnManager : MonoBehaviour
{
    [Tooltip("The active respawn point GameObject (set by checkpoints at runtime).")]
    public GameObject respawnPoint;

    /// <summary>
    /// Returns the respawn position. If none set, returns this manager's transform as a fallback.
    /// </summary>
    public Vector3 GetRespawnPosition()
    {
        if (respawnPoint != null) return respawnPoint.transform.position;
        return transform.position;
    }
}
