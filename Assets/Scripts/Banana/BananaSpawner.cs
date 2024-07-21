using UnityEngine;

public class BananaSpawner : MonoBehaviour
{
    [SerializeField] GameObject banana;
    [SerializeField] GameObject spawnPoint;

    [SerializeField] LightingManager lightningManager;

    bool isSpawned = false;

    void Update()
    {
        if(lightningManager.timeOfDay >= 7f && lightningManager.timeOfDay <= 13f && !isSpawned)
        {
            Instantiate(banana, spawnPoint.transform.position, Quaternion.identity);
            isSpawned = true;
        }

        if(lightningManager.timeOfDay >= 14f)
        {
            isSpawned = false;
        }
    }
}
