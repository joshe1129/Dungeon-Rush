using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    Enemy _Enemy;

    [SerializeField] int maxHealth = 3;

    [Tooltip("Add amount to maxHealth when enemy dies")]
    [SerializeField] int difficultyRamp = 1;

    int currentHealth;

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        _Enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if (currentHealth <= 0)
        {
            ProcessKillEnemy();
        }
    }

    private void ProcessHit()
    {
        currentHealth -= 1;
    }
    
    private void ProcessKillEnemy()
    {
        gameObject.SetActive(false);
        maxHealth += difficultyRamp;
        _Enemy.RewardGold();
    }    
}
