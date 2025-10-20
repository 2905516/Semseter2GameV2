using System.Collections;
using UnityEngine;
using System;

public class playerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamagaed;
    public static event Action OnPlayerDeath;

    [Header("UI & Managers")]
    [Tooltip("GameOver UI root to enable when death sequence finishes.")]
    public GameObject GameOver;

    [Tooltip("Reference to the DeathFadeManager (on Canvas).")]
    public DeathFadeManager deathFadeManager;

    [Tooltip("Optional: one RespawnManager in scene (tagged 'Respawn'). If null, will search by tag.")]
    public RespawnManager respawnManager;

    [Header("Health")]
    public float maxHealth = 100f;
    [SerializeField] public float health;

    [Header("Death / Control")]
    [Tooltip("List of components (movement, camera, input handlers) to disable during death/fade. Drag the scripts (MonoBehaviour) here.")]
    public MonoBehaviour[] componentsToDisableOnDeath;

    [Tooltip("Should the player's position be reset immediately after fade completes (true) or before fade (false). We reset AFTER fade for visual effect.")]
    public bool teleportAfterFade = true;

    private bool isDead = false;

    private void Start()
    {
        health = maxHealth;
        if (GameOver != null) GameOver.SetActive(false);

        if (respawnManager == null)
        {
            var go = GameObject.FindGameObjectWithTag("Respawn");
            if (go != null) respawnManager = go.GetComponent<RespawnManager>();
            if (respawnManager == null) Debug.LogError("playerHealth: RespawnManager not found. Create an object tagged 'Respawn'.");
        }

        if (deathFadeManager == null) Debug.LogError("playerHealth: DeathFadeManager reference missing. Assign death fade UI manager.");
    }

    /// <summary>
    /// Call this to damage the player. If health falls to 0, triggers death sequence.
    /// </summary>
    public void TakeDamage(float Amount)
    {
        if (isDead) return;

        health -= Amount;
        health = Mathf.Clamp(health, 0f, maxHealth);
        OnPlayerDamagaed?.Invoke();

        if (health <= 0f)
        {
            isDead = true;
            Debug.Log("Player is dead");
            StartCoroutine(HandleDeathSequence());
        }
    }

    private IEnumerator HandleDeathSequence()
    {
        // disable player control components if any
        foreach (var mb in componentsToDisableOnDeath)
        {
            if (mb != null) mb.enabled = false;
        }

        // ensure timeScale is normal so animations play; we'll freeze after fade + GameOver
        Time.timeScale = 1f;

        // Unlock cursor so UI can be used
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Play the layered fade (uses unscaled time)
        if (deathFadeManager != null)
        {
            bool done = false;
            deathFadeManager.PlayDeathFade(() => done = true);
            // Wait until delegate sets done
            while (!done) yield return null;
        }

        // After fade completes: teleport to respawn point
        if (respawnManager != null)
        {
            Vector3 pos = respawnManager.GetRespawnPosition();
            transform.position = pos;
        }

        // Show game over UI and freeze
        if (GameOver != null) GameOver.SetActive(true);

        // Pause the game (freeze)
        Time.timeScale = 0f;

        OnPlayerDeath?.Invoke();
    }

    /// <summary>
    /// Called by UI "Try Again" button to resume play: hide GameOver, reset HP, re-enable components, and unfreeze time.
    /// Attach this method to your button's OnClick.
    /// </summary>
    public void OnTryAgain()
    {
        // Hide game over
        if (GameOver != null) GameOver.SetActive(false);

        // Reset fade images
        if (deathFadeManager != null) deathFadeManager.ResetFadeImmediate();

        // Reset health
        health = maxHealth;
        isDead = false;

        // Re-enable components
        foreach (var mb in componentsToDisableOnDeath)
        {
            if (mb != null) mb.enabled = true;
        }

        // Re-lock cursor (optional — depends on your input system)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Unpause
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Utility: Fill HP to full (can be called externally)
    /// </summary>
    public void FullHP()
    {
        health = maxHealth;
        isDead = false;
    }

    // Optional debug: expose current health
    public float GetHealth() => health;
}