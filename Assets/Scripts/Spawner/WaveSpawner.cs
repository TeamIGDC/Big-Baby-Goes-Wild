using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Game.Entity;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Difficulty
    {
        public string difficultyName;
        public List<GameObject> enemies; // List of enemy prefabs for this difficulty
    }

    public List<Difficulty> difficulties; // List of difficulties
    public Transform[] spawnPoints; // Array of spawn points
    public ParticleSystem spawnEffect; // Particle effect to play before spawning an enemy
    public float timeBetweenWaves = 5f; // Time between waves
    public int maxEnemiesPerWave = 10; // Maximum number of enemies to spawn in each wave
    public float spawnIntervalMin = 1f; // Minimum interval between spawns
    public float spawnIntervalMax = 3f; // Maximum interval between spawns

    private int currentWaveIndex = 0;
    private int enemiesSpawned = 0;
    private bool waveInProgress = false;

    [SerializeField] private UIWaveIndicator _waveIndicatorUI;

    void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced.");
        }

        if (difficulties.Count == 0)
        {
            Debug.LogError("No difficulties configured.");
        }
    }

    void Update()
    {
        if (!waveInProgress && currentWaveIndex < difficulties.Count)
        {
            _waveIndicatorUI.TriggerWaveChange(currentWaveIndex);
            StartCoroutine(StartWave(difficulties[currentWaveIndex]));
        }
    }

    IEnumerator StartWave(Difficulty difficulty)
    {
        waveInProgress = true;
        enemiesSpawned = 0;

        while (enemiesSpawned < maxEnemiesPerWave)
        {
            float spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(spawnInterval);

            SpawnEnemy(difficulty.enemies[Random.Range(0, difficulty.enemies.Count)]);
            enemiesSpawned++;
        }

        yield return new WaitForSeconds(timeBetweenWaves);

        waveInProgress = false;
        currentWaveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        StartCoroutine(SpawnEnemyWithEffect(enemy, spawnPoint));
    }

    IEnumerator SpawnEnemyWithEffect(GameObject enemy, Transform spawnPoint)
    {
        ParticleSystem effect = Instantiate(spawnEffect, spawnPoint.position, Quaternion.identity);
        effect.Play();
        yield return new WaitForSeconds(effect.main.duration);
        Destroy(effect.gameObject);

        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
