using UnityEngine;
using UnityEngine.UI;

public class ZombieCounter : MonoBehaviour
{
    [Header("Tag Settings")]
    public string zombieTag = "Zombie"; // <- Make sure your zombie prefabs have this tag

    [Header("UI (Optional)")]
    public Text zombieCountText; // Optional: assign a UI Text to show the count

    public int currentZombieCount;

    void Update()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag(zombieTag);
        currentZombieCount = zombies.Length;

        if (zombieCountText != null)
        {
            zombieCountText.text = "Zombies Alive: " + currentZombieCount;
        }
    }
}
