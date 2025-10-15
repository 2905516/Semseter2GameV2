using UnityEngine;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public GameObject HeartPrefab;
    public playerHealth playerHealth;
    List<HealthHeart> hearts = new List<HealthHeart>();


    public void OnEnable()
    {
        playerHealth.OnPlayerDamagaed += DrawHearts;
    }

    public void OnDisable()
    {
        playerHealth.OnPlayerDamagaed -= DrawHearts;
    }
    public void DrawHearts()
    {
        ClearHearts();

        //now we are determining how many hearts we need to make based of the max health

        float maxHealthRemainder = playerHealth.maxHealth % 2; //1
        int heartstomake = (int)((playerHealth.maxHealth / 2) + maxHealthRemainder);
        //make 5 hearts
        for (int i = 0; i < heartstomake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStateRemainder = (int)Mathf.Clamp(playerHealth.health - (i * 2), 0, 2); //1
            hearts[i].SetHeartState((HeartState)heartStateRemainder);


        }

    }

    public void CreateEmptyHeart()
    {
        GameObject NewHeart = Instantiate(HeartPrefab);
        NewHeart.transform.SetParent(transform);

        HealthHeart healthHeart = NewHeart.GetComponent<HealthHeart>();
        healthHeart.SetHeartState(HeartState.Empty);
        hearts.Add(healthHeart);
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DrawHearts();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
