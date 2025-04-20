using UnityEngine;

/// <summary>
/// Enemy representa una unidad que puede afectar el balance del jugador,
/// ya sea otorgando una recompensa de oro o robando parte del balance.
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("Gold Configuration")]
    [SerializeField] private int goldReward = 25;  // Oro otorgado al ser derrotado.
    [SerializeField] private int goldPenalty = 25; // Oro robado si el enemigo tiene éxito.

    private Bank bank; // Referencia al sistema de banco.

    private void Awake()
    {
        // Buscar Bank de manera más segura solo si no está ya asignado.
        if (bank == null)
        {
            bank = FindFirstObjectByType<Bank>();
        }
    }

    /// <summary>
    /// Otorga oro al jugador (por ejemplo, cuando el enemigo es derrotado).
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
    /// Quita oro al jugador (por ejemplo, cuando el enemigo logra atacar).
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
    /// Permite asignar manualmente el Bank desde otro script si se desea.
    /// </summary>
    /// <param name="bankReference">Referencia al Bank.</param>
    public void SetBank(Bank bankReference)
    {
        this.bank = bankReference;
    }
}
