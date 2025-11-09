using UnityEngine;
using System.Collections;

public class MonsterActivation : MonoBehaviour
{
    public float countDown = 4f; // set your starting time in seconds
    private bool isCountingDown = false;

    public GameObject monsterStatue1;
    public GameObject monsterStatue2;
    public GameObject monsterStatue3;
    public GameObject monsterStatue4;
    public GameObject monsterStatue5;
    public GameObject monsterAlive1;
    public GameObject monsterAlive2;
    public GameObject monsterAlive3;
    public GameObject monsterAlive4;
    public GameObject monsterAlive5;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onActivate();
   


    }

    public void StartCountDown()
    {

        StartCoroutine(CountDownTimer());

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator CountDownTimer()
    {
        isCountingDown = true;

        while (countDown > 0f)
        {
            countDown -= Time.deltaTime; // reduce time each frame
            yield return null;           // wait for the next frame
        }

        countDown = 0f; // make sure it doesn’t go below zero
        isCountingDown = false;

        monsterStatue1.SetActive(false);
        monsterStatue2.SetActive(false);
        monsterStatue3.SetActive(false);


        monsterAlive1.SetActive(true);
        monsterAlive2.SetActive(true);
        monsterAlive3.SetActive(true);
        monsterAlive4.SetActive(true);
        monsterAlive5.SetActive(true);

        Debug.Log("Countdown complete!");
        // You can trigger something here (e.g., play sound, enable object, etc.)
    }

    public void onActivate()
    {

        monsterStatue1.SetActive(true);
        monsterStatue2.SetActive(true);
        monsterStatue3.SetActive(true);
        monsterStatue4.SetActive(false);
        monsterStatue5.SetActive(false);



        monsterAlive1.SetActive(false);
        monsterAlive2.SetActive(false);
        monsterAlive3.SetActive(false);
        monsterAlive4.SetActive(false);
        monsterAlive5.SetActive(false);

    }
}
