using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    public float countDown = 15f; // set your starting time in seconds
    private bool isCountingDown = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CountDownTimer());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private IEnumerator CountDownTimer()
    {
        isCountingDown = true;

        while (countDown > 0f)
        {
            countDown -= Time.deltaTime; // reduce time each frame
            yield return null;           // wait for the next frame
        }

        countDown = 0f; // make sure it doesn’t go below zero
        isCountingDown = false;

        SoundEffectManager.Play("Portal");
        Debug.Log("Countdown complete!");
        // You can trigger something here (e.g., play sound, enable object, etc.)
    }
}
