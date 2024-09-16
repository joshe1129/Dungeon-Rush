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
        //if (hitVFX != null)
        //{
        //    GameObject _VFX = Instantiate(hitVFX, transform.position, Quaternion.identity);
        //    _VFX.transform.parent = parentGameObject.transform;
        //}
        currentHealth -= 1;
    }
    private void ProcessKillEnemy()
    {
        //if (_ScoreBoard == null) return;
        //_ScoreBoard.IncreaseScore(scoreValue);
        //GetComponent<MeshRenderer>().enabled = false;
        //GetComponent<MeshCollider>().enabled = false;
        //if (enemyCrashFX != null)
        //{
        //    GameObject _FX = Instantiate(enemyCrashFX, transform.position, Quaternion.identity);
        //    _FX.transform.parent = parentGameObject.transform;
        //}
        gameObject.SetActive(false);
        maxHealth += difficultyRamp;
        _Enemy.RewardGold();
    }    
}
