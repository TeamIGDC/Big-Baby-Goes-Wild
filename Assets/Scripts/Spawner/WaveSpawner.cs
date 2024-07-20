using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] WaveContent[] waves;

    public int enemiesKilled;
    
    int currentWave;
    float spawnRange = 10f;

    Vector3 spawnPos;

    void Start()
    {
        SpawnWave();
    }

    void Update()
    {
        if(enemiesKilled >= waves[currentWave].GetWaveSpawnList().Length)
        {
            enemiesKilled = 0;
            currentWave++;
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        for(int i = 0; i < waves[currentWave].GetWaveSpawnList().Length; i++)
        {
            Instantiate(waves[currentWave].GetWaveSpawnList()[i], FindSpawnLoc(), Quaternion.identity);
        }
    }

    Vector3 FindSpawnLoc()
    {
        float xLoc = Random.Range(-spawnRange, spawnRange) + transform.position.x;
        float zLoc = Random.Range(-spawnRange, spawnRange) + transform.position.z;
        float yLoc = transform.position.y;

        spawnPos =  new Vector3(xLoc, yLoc, zLoc);

        if(Physics.Raycast(spawnPos, Vector3.down, 5f))
        {
            return spawnPos;
        } else {
            return FindSpawnLoc();
        }
    }
}

[System.Serializable]
public class WaveContent
{
    public GameObject[] waveSpawners;

    public GameObject[] GetWaveSpawnList()
    {
        return waveSpawners;
    }
}
