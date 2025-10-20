using UnityEngine;

/// <summary>
/// Put this on traps/pits/monster-kill colliders. They only call playerHealth.TakeDamage() with a high damage
/// to cause instant death. They DO NOT modify checkpoints/respawn points.
/// </summary>
public class TrapDamage : MonoBehaviour
{
    [Tooltip("Amount of damage to apply when player enters this trigger. Use a very large value for instant kill.")]
    public float damage = 10000f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var ph = other.GetComponent<playerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning($"TrapDamage: Player object missing playerHealth component on {other.name}");
        }
    }
}