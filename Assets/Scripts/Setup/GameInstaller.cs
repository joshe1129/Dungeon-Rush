using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Por si estamos ya en escena inicial
        StartCoroutine(DelayedInject());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedInject());
    }

    private IEnumerator DelayedInject()
    {
        // Esperamos un frame para asegurar que todos los MonoBehaviours estén inicializados
        yield return null;

        Bank bank = FindAnyObjectByType<Bank>();
        if (bank == null)
        {
            Debug.LogError("GameInstaller: No se encontró Bank en la escena.");
            yield break;
        }

        gameManager.Init(bank);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
