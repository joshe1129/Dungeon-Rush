using System.Collections;
using UnityEngine;

/// <summary>
/// Implements object pooling for enemy instances to improve performance.
/// Reuses enemy objects instead of destroying and creating them, and injects required dependencies.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// The enemy prefab to instantiate into the pool.
    /// </summary>
    [Header("Pool Configuration")]
    [SerializeField] private GameObject enemyPrefab;

    /// <summary>
    /// Time interval (in seconds) between enemy spawns.
    /// </summary>
    [SerializeField] [Range(0.1f, 30f)] private float spawnTimer = 1f;

    /// <summary>
    /// Number of enemy instances to pre-allocate in the pool.
    /// </summary>
    [SerializeField] [Range(0, 50)] private int poolSize = 5;

    /// <summary>
    /// Reference to the Bank system for dependency injection into enemies.
    /// </summary>
    [Header("Dependencies")]
    [SerializeField] private Bank bank;

    /// <summary>
    /// Array of pooled enemy game objects.
    /// </summary>
    private GameObject[] pool;

    /// <summary>
    /// Validates configuration and pre-populates the object pool.
    /// Called before Start().
    /// </summary>
    private void Awake()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("ObjectPool: Enemy Prefab not assigned.");
            return;
        }

        if (bank == null)
        {
            Debug.LogError("ObjectPool: Bank reference missing.");
            return;
        }

        PopulatePool();
    }

    /// <summary>
    /// Creates all pool instances and injects dependencies.
    /// All enemies are initially inactive and ready to be spawned.
    /// </summary>
    private void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.SetActive(false);

            InjectDependencies(enemy);

            pool[i] = enemy;
        }
    }

    /// <summary>
    /// Activates the first inactive enemy in the pool.
    /// If all enemies are active, this method does nothing (no infinite spawning).
    /// </summary>
    private void EnableObjectInPool()
    {
        foreach (var enemy in pool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                return;
            }
        }
    }

    /// <summary>
    /// Injects the Bank dependency into an enemy instance.
    /// Called during pool population to set up enemy-bank communication.
    /// </summary>
    /// <param name="enemyObject">The enemy GameObject to inject into.</param>
    private void InjectDependencies(GameObject enemyObject)
    {
        Enemy enemy = enemyObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.SetBank(bank);
        }
    }

    /// <summary>
    /// Starts the enemy spawning coroutine.
    /// Begins activating pooled enemies at regular intervals.
    /// </summary>
    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    /// <summary>
    /// Continuously spawns enemies from the pool at spawnTimer intervals.
    /// </summary>
    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
