using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject SettingsM;

    public bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
        SettingsM.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //pause game sound effect
        SoundEffectManager.Play("Pause");
     
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SoundEffectManager.Play("Button");

        Time.timeScale = 1f;

        SettingsM.SetActive(false);
        pauseMenu.SetActive(false);
        
        isPaused = false;
    }

    public void MainMenu(string sceneName)
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;



    }
    public void RestartLevel()
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;


    }

    public void Settings()
    {

        SoundEffectManager.Play("Button");
        Time.timeScale = 0f;
        pauseMenu.SetActive(false);
        SettingsM.SetActive(true);


    }


    public void Quit()
    {
        SoundEffectManager.Play("Button");
        Application.Quit();

    }

    public void SBack()
    {
        SoundEffectManager.Play("Button");
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        SettingsM.SetActive(false);
        


    }
    public void PBack()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SoundEffectManager.Play("Button");
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
        SettingsM.SetActive(false);



    }
}

