using UnityEngine;

/// <summary>
/// Manages the health and damage system for enemies.
/// Tracks health points, handles damage from tower projectiles, and triggers death/rewards.
/// Implements progressive difficulty by increasing enemy health with each kill (difficultyRamp).
/// </summary>
[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    /// <summary>
    /// Reference to the Enemy component for reward handling on death.
    /// </summary>
    private Enemy enemy;

    /// <summary>
    /// The maximum health points for this enemy type.
    /// Increases after each enemy death to progressively make the game harder.
    /// </summary>
    [SerializeField] private int maxHealth = 3;

    /// <summary>
    /// The amount to increase maxHealth for the next spawned enemy.
    /// Applied after this enemy dies to create difficulty scaling.
    /// </summary>
    [Tooltip("Amount to increase maxHealth for next enemy spawn")]
    [SerializeField] private int difficultyRamp = 1;

    /// <summary>
    /// The current health points remaining for this enemy.
    /// </summary>
    private int currentHealth;

    /// <summary>
    /// Resets health to maximum when the enemy is enabled (spawned).
    /// Called whenever the GameObject is activated.
    /// </summary>
    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Caches the Enemy component reference.
    /// Called before the first frame update.
    /// </summary>
    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    /// <summary>
    /// Handles projectile collision damage.
    /// Called by the particle system when collision occurs.
    /// Reduces health and triggers death logic if health reaches zero.
    /// </summary>
    /// <param name="other">The object that collided with the particle effect.</param>
    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if (currentHealth <= 0)
        {
            ProcessKillEnemy();
        }
    }

    /// <summary>
    /// Reduces enemy health by one hit.
    /// </summary>
    private void ProcessHit()
    {
        currentHealth -= 1;
    }

    /// <summary>
    /// Handles enemy death logic:
    /// 1. Deactivates the enemy (returns to object pool)
    /// 2. Increases difficulty for next enemy
    /// 3. Rewards player with gold
    /// </summary>
    private void ProcessKillEnemy()
    {
        gameObject.SetActive(false);
        maxHealth += difficultyRamp;  // Difficulty scaling
        enemy.RewardGold();
    }
}
