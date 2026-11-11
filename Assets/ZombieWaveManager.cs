using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ZombieWaveManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI waveCounterText;

    public ZombieCounter ZombieCounter;
    public ZombieSpawner spawner;
    public ElectricalBoxPuzzle electricalBoxPuzzle;

    [Header("Wave Settings")]
    public float spawnInterval = 1f;
    public float waveDelay = 20f; // Adjustable delay between waves

    [Header("Status")]
    public bool finished = false;
    public bool abslfinished = false;
    public Dictionary<int, bool> waveStatus = new Dictionary<int, bool>(); // ? New

    [System.Serializable]
    public class WaveData
    {
        public List<int> zombieTypes = new List<int>();
        public List<int> zombieCounts = new List<int>();

        public List<int> GetShuffledSpawnList()
        {
            List<int> spawnList = new List<int>();
            for (int i = 0; i < zombieTypes.Count; i++)
            {
                for (int j = 0; j < zombieCounts[i]; j++)
                {
                    spawnList.Add(zombieTypes[i]);
                }
            }

            // Shuffle
            for (int i = 0; i < spawnList.Count; i++)
            {
                int randIndex = Random.Range(i, spawnList.Count);
                (spawnList[i], spawnList[randIndex]) = (spawnList[randIndex], spawnList[i]);
            }

            return spawnList;
        }
    }

    [Header("Custom Waves (10 Total)")]
    public List<WaveData> waveList = new List<WaveData>();

    void Awake()
    {
        EnsureTenWaves(); // Fill or pad to 10

        // ? Initialize wave status for all 10 waves
        for (int i = 0; i < waveList.Count; i++)
        {
            waveStatus[i] = false;
        }
    }

  
    void EnsureTenWaves()
    {
        while (waveList.Count < 10)
        {
            WaveData wave = new WaveData();

            // Example default: 5 type 1, 3 type 2
            wave.zombieTypes.Add(1);
            wave.zombieCounts.Add(Random.Range(4, 7));

            wave.zombieTypes.Add(2);
            wave.zombieCounts.Add(Random.Range(2, 5));

            waveList.Add(wave);
        }
    }
    private void Start()
    {
        Invoke(nameof(jupstart),15);
    }
    void jupstart()
    {

        StartCoroutine(SpawnAllWaves());
    }
    public IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = 0; waveIndex < waveList.Count; waveIndex++)
        {
            List<int> currentWaveSpawnList = waveList[waveIndex].GetShuffledSpawnList();
            int halfCount = currentWaveSpawnList.Count / 2;

            // ?? Update UI wave count
            if (waveCounterText != null)
            {
                waveCounterText.text = $"{waveIndex + 1}/10";
            }

            Debug.Log($"Wave {waveIndex + 1} starting. Spawning {currentWaveSpawnList.Count} zombies.");

            for (int i = 0; i < currentWaveSpawnList.Count; i++)
            {
                spawner.SpawnZombie(currentWaveSpawnList[i]);
                yield return new WaitForSeconds(spawnInterval);

                // ?? Before wave 5: one check at half-point with 40% chance
                if (waveIndex < 4)
                {
                    if (i == halfCount)
                    {
                        float roll = Random.Range(0f, 1f);
                        if (roll <= 0.4f)
                        {
                            Debug.Log("?? Dice roll (40%) success. Randomizing electrical box.");
                            electricalBoxPuzzle.RandomizeSwitches();
                        }
                        else
                        {
                            Debug.Log("?? Dice roll (40%) failed.");
                        }
                    }
                }
                // ?? Wave 5 and later: check every 5 zombies with 50% chance
                else
                {
                    if (i % 5 == 0 && i != 0) // Every 5 zombies (not at index 0)
                    {
                        float roll = Random.Range(0f, 1f);
                        if (roll <= 0.5f)
                        {
                            Debug.Log("?? Dice roll (50%) success. Randomizing electrical box.");
                            electricalBoxPuzzle.RandomizeSwitches();
                        }
                        else
                        {
                            Debug.Log("?? Dice roll (50%) failed.");
                        }
                    }
                }
            }

            waveStatus[waveIndex] = true;
            Debug.Log($"Wave {waveIndex + 1} completed and marked as finished.");

            yield return new WaitForSeconds(waveDelay);
        }

        if (waveCounterText != null)
        {
            waveCounterText.text = "10/10"; // Fully complete
        }

        finished = true;
        Debug.Log("All waves completed!");
    }
    private void Update()
    {
        if(finished&& ZombieCounter.currentZombieCount >= 0)
        {
            waveCounterText.text = "TRANSITION TO END"; // Fully complete
        }
    }


}
