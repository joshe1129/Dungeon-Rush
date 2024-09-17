using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;

    public void PauseGame()
    {
        // Pause the game
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnPauseGame()
    {
        // Resume the game
        Time.timeScale = 1f;
        isPaused = false;

    }

    public void ReturnMainScene()
    {
        // Load the main menu scene
        Time.timeScale = 1f; // Ensure time resumes
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    { // Exit the application
        Application.Quit();
        Debug.Log("Game is exiting..."); // This only works in a built application, not in the editor
    }
}
