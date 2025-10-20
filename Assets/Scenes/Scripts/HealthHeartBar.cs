using UnityEngine;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public GameObject HeartPrefab;
    public playerHealth playerHealth;

    private List<HealthHeart> hearts = new List<HealthHeart>();

    private void OnEnable()
    {
        playerHealth.OnPlayerDamagaed += DrawHearts;
    }

    private void OnDisable()
    {
        playerHealth.OnPlayerDamagaed -= DrawHearts;
    }

    private void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

        // Determine number of hearts based on max health
        float maxHealthRemainder = playerHealth.maxHealth % 2;
        int heartsToMake = (int)((playerHealth.maxHealth / 2) + maxHealthRemainder);

        for (int i = 0; i < heartsToMake; i++)
            CreateEmptyHeart();

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStateValue = (int)Mathf.Clamp(playerHealth.GetHealth() - (i * 2), 0, 2);
            hearts[i].SetHeartState((HeartState)heartStateValue);
        }
    }

    private void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(HeartPrefab, transform);
        HealthHeart healthHeart = newHeart.GetComponent<HealthHeart>();
        healthHeart.SetHeartState(HeartState.Empty);
        hearts.Add(healthHeart);
    }

    private void ClearHearts()
    {
        foreach (Transform t in transform)
            Destroy(t.gameObject);

        hearts.Clear();
    }
}