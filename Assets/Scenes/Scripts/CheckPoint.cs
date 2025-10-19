using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private RespawnSript respawn;
    private BoxCollider checkPointCollider;

    private void Awake()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnSript>();
        checkPointCollider = GetComponent<BoxCollider>();
    }

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
            respawn.respawnPoint = this.gameObject;
            checkPointCollider.enabled = false;
        }
    }
}
