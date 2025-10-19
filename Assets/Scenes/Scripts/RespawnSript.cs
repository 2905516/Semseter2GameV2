using Unity.VisualScripting;
using UnityEngine;

public class RespawnSript : MonoBehaviour
{
    public GameObject respawnPoint;
    public GameObject player;
    public float fallDamage = 10000f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth playerHealth = player.GetComponent<playerHealth>();
            playerHealth.TakeDamage(fallDamage);
            player.transform.position = respawnPoint.transform.position;
        }
    }
}
