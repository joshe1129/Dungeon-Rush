using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Sistema bancario que gestiona oro del jugador.
/// </summary>
public class Bank : MonoBehaviour, IBank
{
    [SerializeField] private TMP_Text displayBalance;
    [SerializeField] private int startingBalance = 150;

    private int currentBalance;
    public int CurrentBalance => currentBalance;

    public event Action OnBankrupt;

    private void Awake()
    {
        currentBalance = startingBalance;
        UpdateBalance();
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        UpdateBalance();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        UpdateBalance();

        if (currentBalance < 0)
        {
            OnBankrupt?.Invoke();
        }
    }

    private void UpdateBalance()
    {
        if (displayBalance != null)
            displayBalance.text = currentBalance.ToString();
    }
}
