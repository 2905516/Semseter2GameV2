using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckPoint : MonoBehaviour
{
    private RespawnManager respawnManager;
    private Collider checkpointCollider;

    private void Awake()
    {
        checkpointCollider = GetComponent<Collider>();
        checkpointCollider.isTrigger = true;

        var go = GameObject.FindGameObjectWithTag("Respawn");
        if (go != null)
        {
            respawnManager = go.GetComponent<RespawnManager>();
        }
        else
        {
            Debug.LogError("CheckPoint: No object in scene tagged 'Respawn'. Create a RespawnManager and tag it 'Respawn'.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (respawnManager == null) return;

        respawnManager.respawnPoint = this.gameObject;
        checkpointCollider.enabled = false; // prevent re-triggering
        Debug.Log($"Checkpoint set to: {gameObject.name}");
    }
}