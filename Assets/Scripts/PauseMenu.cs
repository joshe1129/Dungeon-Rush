using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages pause menu functionality and game state during pause.
/// Handles pausing/resuming gameplay, returning to main menu, and exiting the application.
/// 
/// Improvement Opportunity: This class directly manipulates Time.timeScale.
/// Consider delegating pause state management to GameManager.ChangeState(GameState.Paused)
/// for consistency with the game's centralized state management system.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Whether the game is currently paused.
    /// </summary>
    private bool isPaused = false;

    /// <summary>
    /// Pauses the game by freezing time.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    /// <summary>
    /// Resumes the game by unfreezing time.
    /// </summary>
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// Returns to the main menu scene and resumes time.
    /// Called when the "Return to Main Menu" button is clicked.
    /// </summary>
    public void ReturnMainScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Exits the application.
    /// Note: Only works in built applications, not in the Unity editor.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
