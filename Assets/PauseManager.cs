using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject shopmenu;
    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {

            Time.timeScale = 0f; // Pause the game (physics, animations, etc.)
            pauseMenuUI.SetActive(true); // Show pause menu if needed
            shopmenu.SetActive(false); // Show pause menu if needed
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
            shopmenu.SetActive(true);
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
    public void QUIT()
    {
        Application.Quit();
    }
    public void RESTART()
    {
        SceneManager.LoadScene("GAME");
    }
    public void TUTTransitor()
    {
        SceneManager.LoadScene("TUTORIAL");
    }
    public void MENUTRANSITION()
    {
        SceneManager.LoadScene("MENU");
    }
}