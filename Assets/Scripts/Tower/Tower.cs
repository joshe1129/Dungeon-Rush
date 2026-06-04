using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a tower structure that can be placed on the battlefield.
/// Handles tower instantiation, cost management, and animated construction.
/// </summary>
public class Tower : MonoBehaviour
{
    /// <summary>
    /// The cost in gold to place this tower on the battlefield.
    /// </summary>
    [SerializeField] private int cost = 75;

    /// <summary>
    /// The delay (in seconds) between revealing each child component during construction animation.
    /// </summary>
    [SerializeField] private float buildTimer = 0.3f;

    /// <summary>
    /// Initializes the tower construction animation when the tower is instantiated.
    /// Called once before the first frame update.
    /// </summary>
    private void Start()
    {
        StartCoroutine(Build());
    }

    /// <summary>
    /// Coroutine that animates the tower construction by progressively revealing child objects.
    /// First reveals all direct children, then reveals their children with delays.
    /// </summary>
    private IEnumerator<WaitForSeconds> Build()
    {
        // Deactivate all children and grandchildren initially
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(false);
            }
        }

        // Activate children one at a time with animation
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildTimer);
            
            // Activate all grandchildren (weapon components, particles, etc.)
            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Creates a new tower instance at the specified position if the player has enough gold.
    /// Deducts the tower cost from the player's balance upon successful placement.
    /// </summary>
    /// <param name="towerPrefab">The tower prefab to instantiate.</param>
    /// <param name="position">The world position where the tower will be placed.</param>
    /// <returns>True if the tower was successfully created; false if insufficient funds or Bank not found.</returns>
    public bool CreateTower(Tower towerPrefab, Vector3 position)
    {
        Bank bank = FindFirstObjectByType<Bank>();
        if (bank == null)
            return false;

        // Check if player has enough gold
        if (bank.CurrentBalance >= cost)
        {
            // Instantiate the tower and deduct cost
            Instantiate(towerPrefab.gameObject, position, Quaternion.identity);
            bank.Withdraw(cost);
            return true;
        }

        return false;
    }
}
