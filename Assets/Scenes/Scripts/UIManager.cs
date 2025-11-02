using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class UIManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject SettingsM;
    public GameObject GameOver;
    [SerializeField] public AudioSource soundEffectManager;
    public playerHealth playerHealth;
    public GameObject player;

    public void Start()
    {
        playerHealth = player.GetComponent<playerHealth>();
        player = GameObject.FindWithTag("Player");
        PauseMenu.SetActive(false);
        SettingsM.SetActive(false);
        GameOver.SetActive(false);
    }

    public void Update()
    {

       // playerHealth playerHealth = player.GetComponent<playerHealth>();
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
        SoundEffectManager.Stop();

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

        SoundEffectManager.Stop();
        SoundEffectManager.Play("ComicTheme");

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
        Time.timeScale = 1f;




    }

    public void Page5(string sceneName)
    {
        SoundEffectManager.Play("Button");
       

        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;



    }
    public void RestartLevel()
    {
        SoundEffectManager.Play("Button");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;


    }

    public void Respawn()
    {
        playerHealth.FullHP();
        SoundEffectManager.Play("Button");
        SoundEffectManager.Stop();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameOver.SetActive(false);
        Time.timeScale = 1f;

    }
    public void PlayGame(string sceneName)
    {
        SoundEffectManager.Play("Button");
        SceneManager.LoadScene(sceneName);
        SoundEffectManager.Stop();

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

