using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class UIManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject SettingsM;
    [SerializeField] public AudioSource soundPlayer;

    public void Start()
    {
       PauseMenu.SetActive(false);
       SettingsM.SetActive(false);
    }
    private void OnEnable()
    {

       // playerHealth.OnPlayerDeath += EnableGameOverMenu;
    }

    private void OnDisable()
    {

      // playerHealth.OnPlayerDeath -= EnableGameOverMenu;
    }



    public void MainMenu(string sceneName)
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(sceneName);

        //Play music for the Main Menu
        SoundEffectManager.Play("MainTheme");
        Time.timeScale = 1f;

    }

    public void DeathMenu(string sceneName)
    {
        
        SceneManager.LoadScene(sceneName);

        //Play music for the game over menu
        SoundEffectManager.Play("GameOver");
        Time.timeScale = 1f;



    }
    public void Page1(string sceneName)
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;



    }
    public void Page2(string sceneName)
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;



    }
    public void Page3(string sceneName)
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;



    }
    public void Page4(string sceneName)
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(sceneName);

        //Play lvl 1 background music
        SoundEffectManager.Play("lvlMusic");
        Time.timeScale = 1f;



    }
    public void RestartLevel()
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;


    }

    public void PlayGame(string sceneName)
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(sceneName);

        //Play music for the comics
        SoundEffectManager.Play("ComicSong");
        Time.timeScale = 1f;


    }

   


    public void Quit()
    {
        SoundEffectManager.Play("Button");
        Application.Quit();

    }


}

