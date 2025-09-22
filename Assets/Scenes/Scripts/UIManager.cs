using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class UIManager : MonoBehaviour
{
    public GameObject GameOverMenu;
    [SerializeField] public AudioSource soundPlayer;

    private void OnEnable()
    {

       // playerHealth.OnPlayerDeath += EnableGameOverMenu;
    }

    private void OnDisable()
    {

      // playerHealth.OnPlayerDeath -= EnableGameOverMenu;
    }

    public void EnableGameOverMenu()
    {
        Time.timeScale = 0f;
        GameOverMenu.SetActive(true);
        Time.timeScale = 1f;
    }

    public void MainMenu(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;



    }

    public void DeathMenu(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;



    }
    public void RestartLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;


    }

    public void PlayGame(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;


    }

    public void Options(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;


    }


    public void Quit()
    {
        Application.Quit();

    }


}

