using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    public GameObject pauseMenuUI;
    public GameObject settingsUI;
    public DeathFadeManager deathFadeManager;
    public MonoBehaviour respawnManager;

    [Header("State")]
    public bool isPaused = false;

    private bool canToggle = true;
    private float inputCooldown = 0.25f;

    void Start()
    {
        if (pauseMenuUI) pauseMenuUI.SetActive(false);
        if (settingsUI) settingsUI.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (canToggle && Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(HandlePauseToggle());
        }
    }

    private IEnumerator HandlePauseToggle()
    {
        canToggle = false;

        if (isPaused)
            ResumeGame();
        else
            PauseGame(); 

        // Wait for key release before next toggle
        yield return new WaitUntil(() => !Input.GetKey(KeyCode.P));
        yield return new WaitForSecondsRealtime(inputCooldown);

        canToggle = true;
    }

    public void PauseGame()
    {

        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SoundEffectManager.Play("OpenUI");
        if (pauseMenuUI) pauseMenuUI.SetActive(true);
        if (settingsUI) settingsUI.SetActive(false);

        Debug.Log("[PauseMenu] Game Paused");
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        

        if (pauseMenuUI) pauseMenuUI.SetActive(false);
        if (settingsUI) settingsUI.SetActive(false);

        Debug.Log("[PauseMenu] Game Resumed");
    }

    public void OnBackButton()
    {
        SoundEffectManager.Play("Button");
        ResumeGame();
    }

    public void OnSettingsButton()
    {
        SoundEffectManager.Play("OpenUI");

        if (pauseMenuUI) pauseMenuUI.SetActive(false);
        if (settingsUI) settingsUI.SetActive(true);
    }

    public void OnSettingsBackButton()
    {
        SoundEffectManager.Play("Button");
        if (settingsUI) settingsUI.SetActive(false);
        if (pauseMenuUI) pauseMenuUI.SetActive(true);
    }

    public void OnRestartButton()
    {
        SoundEffectManager.Play("Button");
        StartCoroutine(RestartSequence());
    }

    private IEnumerator RestartSequence()
    {
        Debug.Log("[PauseMenu] Restart initiated...");
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseMenuUI) pauseMenuUI.SetActive(false);
        if (settingsUI) settingsUI.SetActive(false);

        yield return new WaitForSecondsRealtime(0.05f);

        // Fade out effect
        if (deathFadeManager != null)
        {
            deathFadeManager.gameObject.SetActive(true);
            deathFadeManager.ResetFadeImmediate();

            // Use unscaled time for fade
            yield return deathFadeManager.PlayDeathFadeRoutine();
        }

        // Respawn at checkpoint if available
        if (respawnManager != null)
        {
            var method = respawnManager.GetType().GetMethod("RespawnPlayerAtCheckpoint");
            if (method != null)
            {
                method.Invoke(respawnManager, null);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                yield break;
            }
        }

        // Fallback: reload scene after fade
        yield return new WaitForSecondsRealtime(0.4f);
        Debug.Log("[PauseMenu] Reloading current scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu(string sceneName)
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(sceneName);
        SoundEffectManager.Stop();

      
        Time.timeScale = 1f;
    }

    public void OnQuitButton()
    {
        SoundEffectManager.Play("Button");
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}