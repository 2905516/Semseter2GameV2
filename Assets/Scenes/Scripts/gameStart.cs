using UnityEngine;

public class gameStart : MonoBehaviour
{
    [SerializeField] public AudioSource soundEffectManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundEffectManager.Play("MainTheme");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
