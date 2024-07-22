using UnityEngine;

public class BananaSpawner : MonoBehaviour
{
    [SerializeField] GameObject banana;
    [SerializeField] GameObject[] spawnPoints;

    [SerializeField] LightingManager lightningManager;

    bool isSpawned = false;

    void Update()
    {
        if(lightningManager.timeOfDay >= 7f && lightningManager.timeOfDay <= 13f && !isSpawned)
        {
            Instantiate(banana, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
            isSpawned = true;
        }

        if(lightningManager.timeOfDay >= 14f)
        {
            isSpawned = false;
        }
    }
}
