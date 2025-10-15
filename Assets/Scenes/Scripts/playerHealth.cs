using UnityEngine;
using System;
public class playerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamagaed;
    public static event Action OnPlayerDeath;


    [SerializeField] public float health, maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
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
            Time.timeScale = 1f;
            OnPlayerDeath?.Invoke();
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
