using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;

    [Header("Zombie Type (1 to 4)")]
    [Range(1, 4)]
    public int zombieType = 1;

    public Transform spawnPoint;

    public void SpawnZombie(int type)
    {
        GameObject prefabToSpawn = null;

        switch (type)
        {
            case 1:
                prefabToSpawn = prefab1;
                break;
            case 2:
                prefabToSpawn = prefab2;
                break;
            case 3:
                prefabToSpawn = prefab3;
                break;
            case 4:
                prefabToSpawn = prefab4;
                break;
            default:
                Debug.LogWarning("Zombie type must be between 1 and 4.");
                return;
        }

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Missing prefab for zombie type: " + type);
        }
    }
}
