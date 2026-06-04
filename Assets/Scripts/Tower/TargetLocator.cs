using UnityEngine;

/// <summary>
/// Handles tower targeting and attack logic.
/// Finds the closest enemy within range and aims/attacks toward it.
/// </summary>
public class TargetLocator : MonoBehaviour
{
    /// <summary>
    /// The shooter transform that will be rotated to aim at targets.
    /// </summary>
    [SerializeField] private Transform shooter;

    /// <summary>
    /// Particle system that fires projectiles when attacking.
    /// </summary>
    [SerializeField] private ParticleSystem projectileFx;

    /// <summary>
    /// The maximum distance from which the tower can detect and attack enemies.
    /// </summary>
    [SerializeField] private float towerRange = 7f;

    /// <summary>
    /// Animator component for playing attack animations.
    /// </summary>
    [SerializeField] private Animator animator;

    /// <summary>
    /// The current target enemy's transform.
    /// </summary>
    private Transform target;

    /// <summary>
    /// Initializes the first available enemy as the initial target.
    /// </summary>
    private void Start()
    {
        Enemy firstEnemy = FindAnyObjectByType<Enemy>();
        if (firstEnemy != null)
        {
            target = firstEnemy.transform;
        }
    }

    /// <summary>
    /// Updates targeting and attack logic every frame.
    /// </summary>
    private void Update()
    {
        FindClosestTarget();
        AimToTarget();
    }

    /// <summary>
    /// Searches all active enemies and updates the target to the closest one.
    /// Performance Note: Searches all enemies every frame - consider caching for optimization.
    /// </summary>
    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (targetDistance < closestDistance)
            {
                closestTarget = enemy.transform;
                closestDistance = targetDistance;
            }
        }
        target = closestTarget;
    }

    /// <summary>
    /// Determines if the current target is in range and performs attack logic.
    /// </summary>
    private void AimToTarget()
    {
        if (target == null) 
            return;
            
        float targetDistance = Vector3.Distance(transform.position, target.position);
        if (targetDistance < towerRange)
        {
            Attack(true);
            LookAtTarget();
        }
        else
        {
            Attack(false);
        }
    }

    /// <summary>
    /// Rotates the weapon to face the target position (Y axis only).
    /// </summary>
    private void LookAtTarget()
    {
        Vector3 direction = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            shooter.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }

    /// <summary>
    /// Controls attack state by enabling/disabling animation and particle effects.
    /// </summary>
    /// <param name="isActive">True to activate attack, false to deactivate.</param>
    private void Attack(bool isActive)
    {
        if (animator != null) 
        { 
            animator.SetBool("isAttacking", isActive); 
        }
        var emissionModule = projectileFx.emission;
        emissionModule.enabled = isActive;
    }
}
