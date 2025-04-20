using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Enum <c>GameState</c> define los distintos estados posibles del juego.
/// </summary>
public enum GameState
{
    Menu,
    Playing,
    Paused,
    GameOver
}

/// <summary>
/// Controla el estado del juego y maneja eventos globales.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private IBank bank;
    public GameState CurrentState { get; private set; } = GameState.Menu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Init(IBank injectedBank)
    {
        if (bank != null)
            bank.OnBankrupt -= HandleBankrupt;

        bank = injectedBank;

        // Seguridad extra
        bank.OnBankrupt -= HandleBankrupt; // por si acaso está duplicado
        bank.OnBankrupt += HandleBankrupt;
    }

    private void Start()
    {
        ChangeState(GameState.Playing);
    }

    private void HandleBankrupt()
    {
        Debug.Log("Game Over: Bankrupt!");
        ChangeState(GameState.GameOver);
        ReloadScene();
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        Time.timeScale = (newState == GameState.Playing) ? 1f : 0f;
        Debug.Log($"Game State changed to: {newState}");
    }

    public void PauseGame() => ChangeState(GameState.Paused);
    public void ResumeGame() => ChangeState(GameState.Playing);

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        if (bank != null)
            bank.OnBankrupt -= HandleBankrupt;
    }
}
