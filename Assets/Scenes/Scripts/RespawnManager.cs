using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Tooltip("The active respawn point GameObject (set by checkpoints at runtime).")]
    public GameObject respawnPoint;

    private GameObject initialSpawnPoint;

    private void Awake()
    {
        // Save the starting respawn position as the fallback
        initialSpawnPoint = new GameObject("InitialSpawnPoint");
        initialSpawnPoint.transform.position = transform.position;
        initialSpawnPoint.transform.rotation = transform.rotation;

        // If no checkpoint has updated respawnPoint yet, use initial spawn
        if (respawnPoint == null)
            respawnPoint = initialSpawnPoint;
    }

    public Vector3 GetRespawnPosition()
    {
        if (respawnPoint != null)
            return respawnPoint.transform.position;

        return initialSpawnPoint.transform.position;
    }

    public void ResetToInitialSpawn()
    {
        respawnPoint = initialSpawnPoint;
    }

    public void RespawnPlayerAtCheckpoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("RespawnManager: Player not found during respawn.");
            return;
        }

        if (respawnPoint == null)
        {
            Debug.LogWarning("RespawnManager: No respawn point set, using initial spawn.");
            respawnPoint = initialSpawnPoint;
        }

        player.transform.position = respawnPoint.transform.position;
        player.GetComponent<playerHealth>()?.FullHP();

        // Restore camera and controls
        FPController fp = player.GetComponent<FPController>();
        if (fp != null) fp.RestoreRotationState();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}