using System.Collections;
using UnityEngine;

/// <summary>
/// ObjectPool gestiona la reutilización de enemigos mediante pooling y les inyecta dependencias necesarias.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [Header("Configuración del Pool")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] [Range(0.1f, 30f)] private float spawnTimer = 1f;
    [SerializeField] [Range(0, 50)] private int poolSize = 5;

    [Header("Dependencias")]
    [SerializeField] private Bank bank;

    private GameObject[] pool;

    private void Awake()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("ObjectPool: No se asignó Enemy Prefab.");
            return;
        }

        if (bank == null)
        {
            Debug.LogError("ObjectPool: Faltan referencias a Bank o GameManager.");
            return;
        }

        PopulatePool();
    }

    /// <summary>
    /// Llena el pool instanciando enemigos desactivados.
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
    /// Activa un objeto disponible del pool.
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
    /// Inyecta referencias necesarias al enemigo.
    /// </summary>
    /// <param name="enemyObject">GameObject instanciado del enemigo</param>
    private void InjectDependencies(GameObject enemyObject)
    {
        Enemy enemy = enemyObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.SetBank(bank);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    /// <summary>
    /// Corrutina que activa enemigos en intervalos definidos.
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
