using System;
using System.Collections;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamagaed;
    public static event Action OnPlayerDeath;

    [Header("UI & Managers")]
    public GameObject GameOver;
    public DeathFadeManager deathFadeManager;
    public RespawnManager respawnManager;

    [Header("Health")]
    public float maxHealth = 100f;
    [SerializeField] public float health;

    [Header("Death / Control")]
    public MonoBehaviour[] componentsToDisableOnDeath;
    public bool teleportAfterFade = true;

    private bool isDead = false;

    private void Awake()
    {
        // Ensure cursor is locked at game start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        health = maxHealth;
        if (GameOver != null) GameOver.SetActive(false);

        if (respawnManager == null)
        {
            var go = GameObject.FindGameObjectWithTag("Respawn");
            if (go != null) respawnManager = go.GetComponent<RespawnManager>();
            if (respawnManager == null)
                Debug.LogError("playerHealth: RespawnManager not found. Create an object tagged 'Respawn'.");
        }

        if (deathFadeManager == null)
            Debug.LogError("playerHealth: DeathFadeManager reference missing. Assign it in Inspector.");
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
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
        // Disable player controls
        foreach (var mb in componentsToDisableOnDeath)
            if (mb != null) mb.enabled = false;

        // Ensure time is normal for fade animations
        Time.timeScale = 1f;

        // Unlock cursor for UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Fade
        if (deathFadeManager != null)
            yield return deathFadeManager.PlayDeathFadeRoutine();

        // Teleport to respawn point
        if (respawnManager != null)
            transform.position = respawnManager.GetRespawnPosition();

        // Show Game Over UI
        if (GameOver != null) GameOver.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;

        OnPlayerDeath?.Invoke();
    }

    public void OnTryAgain()
    {
        // Hide GameOver and reset fade
        if (GameOver != null) GameOver.SetActive(false);
        if (deathFadeManager != null) deathFadeManager.ResetFadeImmediate();

        // Reset health
        health = maxHealth;
        isDead = false;

        // Re-enable player control components
        foreach (var mb in componentsToDisableOnDeath)
            if (mb != null) mb.enabled = true;

        // Unpause
        Time.timeScale = 1f;

        // Redraw health UI
        OnPlayerDamagaed?.Invoke();

        // Lock cursor again (delayed one frame to ensure UI state settles)
        StartCoroutine(ReLockCursorNextFrame());
    }
    private IEnumerator ReLockCursorNextFrame()
    {
        yield return null;

        // Find PauseMenu instance (safe even if missing)
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();

        // Only lock cursor if game is not paused
        if (pauseMenu == null || !pauseMenu.isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void FullHP()
    {
        health = maxHealth;
        isDead = false;
        OnPlayerDamagaed?.Invoke();
    }

    public float GetHealth() => health;
}