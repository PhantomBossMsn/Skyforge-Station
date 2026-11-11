using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EngineHealth : MonoBehaviour
{
    public float health =500;
    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            SceneManager.LoadScene("DeathScene");

        }
    }

}
