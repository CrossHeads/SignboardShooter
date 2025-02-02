using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DC.HealthSystem;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab; // The enemy prefab to spawn
        public int amount; // The amount of this specific enemy to spawn
        public bool isBoss; // Whether this is a boss
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public EnemySpawnInfo[] enemiesToSpawn; // List of enemies with amounts
        public float spawnDelay; // Time between spawns
        public float waveDuration; // Time limit for non-boss waves
    }

    public Wave[] waves; // Array of waves
    public Transform[] spawnPoints; // Array of spawn points
    public float timeBetweenWaves; // Delay between waves
    public GameObject fx; // FX for enemy destruction

    private int currentWaveIndex = 0;
    private bool spawningWave = false;
    private int totalEnemiesInCurrentWave = 0;
    private int defeatedEnemies = 0;
    private bool bossDefeated = false;

    public TMP_Text wavesText;

    public TMP_Text timeText;

    private float effectCooldown = 0.5f; // Time in seconds before you can spawn the next death effect
    private float lastEffectTime = 0f;

    private bool waveInProgress = false; // Flag to track if a wave is in progress

    private void Start()
    {
        StartCoroutine(StartWaveRoutine());
        wavesText.text = "WAVE: " + currentWaveIndex;
    }

    private IEnumerator StartWaveRoutine()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];

            // Set wave text and initial time
            wavesText.text = "WAVE: " + (currentWaveIndex + 1);
            timeText.text = "TIME LEFT: " + Mathf.FloorToInt(currentWave.waveDuration).ToString();

            // Mark the wave as in progress
            waveInProgress = true;

            // Reset counters and prepare for spawning
            spawningWave = true;
            defeatedEnemies = 0; // Reset defeated enemies counter
            totalEnemiesInCurrentWave = GetTotalEnemiesInWave(currentWave); // Get total enemies for current wave

            // Start spawning enemies after initializing the wave
            StartCoroutine(SpawnEnemies(currentWave));

            // If it's a boss wave, wait for the boss to be defeated
            if (IsBossWave(currentWave))
            {
                while (!bossDefeated)
                {
                    yield return null; // Wait until boss is defeated
                }
            }
            else
            {
                // For non-boss waves, use the wave duration
                float waveEndTime = Time.time + currentWave.waveDuration; // Set the wave's end time
                while (waveInProgress && Time.time < waveEndTime && defeatedEnemies < totalEnemiesInCurrentWave)
                {
                    // Update the time left in the UI
                    int timeLeft = Mathf.FloorToInt(waveEndTime - Time.time);
                    timeText.text = "TIME LEFT: " + timeLeft.ToString();

                    yield return null; // Continue while the wave is still ongoing
                }

                // After the wave time ends or all enemies are defeated, stop spawning enemies and destroy any remaining ones
                spawningWave = false;

                // If the wave time ends before all enemies are defeated, destroy remaining enemies
                if (defeatedEnemies < totalEnemiesInCurrentWave)
                {
                    DestroyRemainingEnemies();
                }

                // Mark the wave as completed
                waveInProgress = false;
            }

            // Wait before starting the next wave
            float timeUntilNextWave = timeBetweenWaves;
            while (timeUntilNextWave > 0)
            {
                timeText.text = "TIME TILL NEXT WAVE: " + Mathf.FloorToInt(timeUntilNextWave).ToString();
                timeUntilNextWave -= Time.deltaTime; // Decrease the countdown time

                yield return null; // Wait one frame
            }

            // Move to the next wave
            currentWaveIndex++;

            // Check if this was the final wave
            if (currentWaveIndex >= waves.Length)
            {
                Debug.Log("All waves completed!");
                break; // Stop spawning waves after the last one
            }
        }
    }

    private IEnumerator SpawnEnemies(Wave wave)
    {
        Debug.Log($"Starting Wave: {wave.waveName}");

        foreach (EnemySpawnInfo enemyInfo in wave.enemiesToSpawn)
        {
            for (int i = 0; i < enemyInfo.amount; i++)
            {
                // Check if the wave is still in progress before spawning an enemy
                if (!waveInProgress)
                {
                    yield break; // Stop spawning if the wave is no longer active
                }

                // Randomly select a spawn point
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                // Spawn the enemy
                GameObject enemy = Instantiate(enemyInfo.enemyPrefab, spawnPoint.position, Quaternion.identity);

                // Set up health and death handlers for the enemy
                Health enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.OnDeath += DefeatEnemy; // Add event listener for enemy defeat
                }

                // If it's a boss, set up the boss defeat handler
                if (enemyInfo.isBoss)
                {
                    enemyHealth.OnDeath += BossDefeated;
                }

                // Wait for the next spawn delay
                yield return new WaitForSeconds(wave.spawnDelay);
            }
        }

        // Mark spawning as completed for this wave
        spawningWave = false;
        Debug.Log($"Wave {wave.waveName} completed.");
    }

    private void DefeatEnemy(float damage)
    {
        defeatedEnemies++;

        // Instantiate the FX when an enemy is defeated
        GameObject fxLive = Instantiate(fx, transform.position, transform.rotation);
        Destroy(fxLive, 0.27f); // Adjust FX duration as needed
    }

    private void BossDefeated(float damage)
    {
        bossDefeated = true;

        // Destroy all remaining enemies after boss defeat
        DestroyRemainingEnemies();
    }

    private void DestroyRemainingEnemies()
    {
        GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in remainingEnemies)
        {
            Destroy(enemy);

            // Instantiate the FX for each destroyed enemy
            GameObject fxLive = Instantiate(fx, enemy.transform.position, enemy.transform.rotation);
            Destroy(fxLive, 0.27f);
        }

        Debug.Log("Wave time ended, remaining enemies destroyed.");
    }

    private int GetTotalEnemiesInWave(Wave wave)
    {
        int total = 0;
        foreach (EnemySpawnInfo enemyInfo in wave.enemiesToSpawn)
        {
            total += enemyInfo.amount;
        }
        return total;
    }

    private bool IsBossWave(Wave wave)
    {
        foreach (EnemySpawnInfo enemyInfo in wave.enemiesToSpawn)
        {
            if (enemyInfo.isBoss)
                return true;
        }
        return false;
    }
}