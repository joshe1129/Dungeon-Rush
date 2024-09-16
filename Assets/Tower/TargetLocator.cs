using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform _Weapon;
    [SerializeField] ParticleSystem projectileFX;
    [SerializeField] float towerRange = 7f;
    [SerializeField] Animator animator;

    Transform _Target;

    void Start()
    {
        _Target = FindAnyObjectByType<Enemy>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimtoTarget();
    }

    private void FindClosestTarget()
    {
        Enemy[] _Enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (Enemy enemy in _Enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (targetDistance < maxDistance )
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }
        _Target = closestTarget;
    }

    private void AimtoTarget()
    {
        if ( _Target == null ) return;
        float targetDistance = Vector3.Distance(transform.position, _Target.transform.position);
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

    private void LookAtTarget()
    {
        // Calcular la dirección hacia el objetivo ignorando la diferencia en el eje Y
        Vector3 direction = new Vector3(_Target.transform.position.x - transform.position.x, 0, _Target.transform.position.z - transform.position.z);

        // Si la dirección no es cero, rotar el arma
        if (direction != Vector3.zero)
        {
            // Calcular la rotación hacia la dirección
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Aplicar la rotación al arma limitándola solo al eje Y
            _Weapon.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }

    void Attack(bool isActive)
    {
        if (animator != null) { animator.SetBool("isAttacking", isActive); }
        var emmissionModule = projectileFX.emission;
        emmissionModule.enabled = isActive;
    }
}
