using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Initializes and injects dependencies between game systems.
/// Ensures GameManager receives a reference to the Bank system for state management.
/// Part of the dependency injection initialization chain.
/// </summary>
public class GameInstaller : MonoBehaviour
{
    /// <summary>
    /// Reference to the GameManager singleton that needs to be initialized.
    /// </summary>
    [SerializeField] private GameManager gameManager;

    /// <summary>
    /// Registers for scene load events and triggers initial dependency injection.
    /// Called when this component's script is first loaded.
    /// </summary>
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Delay injection to ensure all MonoBehaviours are initialized
        StartCoroutine(DelayedInject());
    }

    /// <summary>
    /// Handles scene load events by re-injecting dependencies.
    /// Called whenever a new scene is loaded.
    /// </summary>
    /// <param name="scene">The loaded scene.</param>
    /// <param name="mode">How the scene was loaded.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedInject());
    }

    /// <summary>
    /// Waits one frame to ensure all MonoBehaviours have initialized, then injects Bank into GameManager.
    /// </summary>
    private IEnumerator DelayedInject()
    {
        // Wait one frame for all Awake() calls to complete
        yield return null;

        Bank bank = FindAnyObjectByType<Bank>();
        if (bank == null)
        {
            Debug.LogError("GameInstaller: No Bank found in the scene.");
            yield break;
        }

        gameManager.Init(bank);
    }

    /// <summary>
    /// Unregisters scene load callback when this component is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
