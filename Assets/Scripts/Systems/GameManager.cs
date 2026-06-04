using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Enumeration of all possible game states.
/// </summary>
public enum GameState
{
    /// <summary>Main menu or startup state.</summary>
    Menu,
    /// <summary>Game is actively running.</summary>
    Playing,
    /// <summary>Game is paused, time is frozen.</summary>
    Paused,
    /// <summary>Game over, player has lost.</summary>
    GameOver
}

/// <summary>
/// Manages global game state and orchestrates high-level game logic.
/// Implements the Singleton pattern to ensure only one instance exists.
/// Handles game state transitions, pause/resume, and bankruptcy (loss) conditions.
/// Persists across scene loads using DontDestroyOnLoad.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The singleton instance of GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Reference to the banking system for listening to bankruptcy events.
    /// </summary>
    private IBank bank;

    /// <summary>
    /// The current game state (Menu, Playing, Paused, GameOver).
    /// </summary>
    public GameState CurrentState { get; private set; } = GameState.Menu;

    /// <summary>
    /// Initializes the singleton and persists the instance across scene loads.
    /// Called when the script is first loaded.
    /// </summary>
    private void Awake()
    {
        // Implement singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Initializes the GameManager with a reference to the banking system.
    /// Called by GameInstaller during setup.
    /// </summary>
    /// <param name="injectedBank">The Bank implementation to use for state management.</param>
    public void Init(IBank injectedBank)
    {
        // Remove old listener if one exists
        if (bank != null)
            bank.OnBankrupt -= HandleBankrupt;

        bank = injectedBank;

        // Re-attach listener safely
        bank.OnBankrupt -= HandleBankrupt;  // Remove duplicate just in case
        bank.OnBankrupt += HandleBankrupt;
    }

    /// <summary>
    /// Starts the game in Playing state when the scene loads.
    /// </summary>
    private void Start()
    {
        ChangeState(GameState.Playing);
    }

    /// <summary>
    /// Handles the bankruptcy event when player runs out of gold.
    /// Triggers game over state and reloads the scene.
    /// </summary>
    private void HandleBankrupt()
    {
        Debug.Log("Game Over: Bankrupt!");
        ChangeState(GameState.GameOver);
        ReloadScene();
    }

    /// <summary>
    /// Changes the current game state and adjusts time scale accordingly.
    /// Paused and GameOver states freeze time (timeScale = 0).
    /// Playing state resumes time (timeScale = 1).
    /// </summary>
    /// <param name="newState">The new game state to transition to.</param>
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        // Freeze time for paused and game over states
        Time.timeScale = (newState == GameState.Playing) ? 1f : 0f;
        Debug.Log($"Game State changed to: {newState}");
    }

    /// <summary>
    /// Pauses the game by changing state to Paused.
    /// </summary>
    public void PauseGame() => ChangeState(GameState.Paused);

    /// <summary>
    /// Resumes the game by changing state to Playing.
    /// </summary>
    public void ResumeGame() => ChangeState(GameState.Playing);

    /// <summary>
    /// Reloads the current scene and resets time scale.
    /// </summary>
    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Unsubscribes from bank events when GameManager is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (bank != null)
            bank.OnBankrupt -= HandleBankrupt;
    }
}
