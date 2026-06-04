using UnityEngine;

/// <summary>
/// Represents an enemy unit in the game that affects the player's gold balance.
/// Enemies can reward gold when defeated or penalize the player when they reach the castle.
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Amount of gold awarded to the player when this enemy is defeated.
    /// </summary>
    [Header("Gold Configuration")]
    [SerializeField] private int goldReward = 25;

    /// <summary>
    /// Amount of gold deducted from the player when this enemy reaches the castle.
    /// </summary>
    [SerializeField] private int goldPenalty = 25;

    /// <summary>
    /// Reference to the banking system for handling gold transactions.
    /// </summary>
    private Bank bank;

    /// <summary>
    /// Initializes the bank reference by finding the Bank component in the scene.
    /// Called before Start().
    /// </summary>
    private void Awake()
    {
        // Find and cache the bank reference only if not already assigned
        if (bank == null)
        {
            bank = FindFirstObjectByType<Bank>();
        }
    }

    /// <summary>
    /// Awards gold to the player when the enemy is defeated.
    /// Typically called when enemy health reaches zero.
    /// </summary>
    public void RewardGold()
    {
        if (bank == null)
        {
            Debug.LogWarning("Enemy: No Bank assigned. Cannot reward gold.");
            return;
        }

        bank.Deposit(goldReward);
    }

    /// <summary>
    /// Removes gold from the player when the enemy successfully reaches the castle.
    /// Typically called when enemy reaches the destination node.
    /// </summary>
    public void StealGold()
    {
        if (bank == null)
        {
            Debug.LogWarning("Enemy: No Bank assigned. Cannot steal gold.");
            return;
        }

        bank.Withdraw(goldPenalty);
    }

    /// <summary>
    /// Manually assigns a Bank reference to this enemy.
    /// Useful for dependency injection or runtime initialization.
    /// </summary>
    /// <param name="bankReference">The Bank instance to use for transactions.</param>
    public void SetBank(Bank bankReference)
    {
        this.bank = bankReference;
    }
}
