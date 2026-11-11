using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TUTGameTRANS : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(Transitor), 74);
    }
    void Transitor()
    {
        SceneManager.LoadScene("GAME");
    }

}
