using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles main menu UI interactions.
/// Provides entry point to start the game and exit the application.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Loads the gameplay scene (scene index 1) asynchronously.
    /// Called when the "Play" button is clicked.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    /// <summary>
    /// Exits the application.
    /// Called when the "Exit" button is clicked.
    /// Note: Only works in built applications, not in the Unity editor.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
