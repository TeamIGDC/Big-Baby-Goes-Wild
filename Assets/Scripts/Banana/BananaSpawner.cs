using UnityEngine;

public class BananaSpawner : MonoBehaviour
{
    [SerializeField] GameObject banana;
    [SerializeField] GameObject spawnEffect;

    [SerializeField] GameObject[] spawnPoints;

    [SerializeField] LightingManager lightningManager;

    bool isSpawned = false;

    void Update()
    {
        if(lightningManager.timeOfDay >= 7f && lightningManager.timeOfDay <= 13f && !isSpawned)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            GameObject effect = Instantiate(spawnEffect, spawnPoints[spawnIndex].transform.position, Quaternion.identity);
            Destroy(effect, 2f);
            Instantiate(banana, spawnPoints[spawnIndex].transform.position, Quaternion.identity);
            isSpawned = true;
        }

        if(lightningManager.timeOfDay >= 14f)
        {
            isSpawned = false;
        }
    }
}
