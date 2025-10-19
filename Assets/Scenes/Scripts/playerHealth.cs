using UnityEngine;
using System;
public class playerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamagaed;
    public static event Action OnPlayerDeath;
    public GameObject GameOver;


    [SerializeField] public float health, maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        health = maxHealth;
        GameOver.SetActive(false);
    }

    public void TakeDamage(float Amount)
    {
        health -= Amount;
        OnPlayerDamagaed?.Invoke();

        //checking if plauer is dead
        if (health <= 0)
        {
            health = 0;
            Debug.Log("Player is dead");
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameOver.SetActive(true);


            OnPlayerDeath?.Invoke();
        }

    }

    public void FullHP()
    {
        health = maxHealth;
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}
