using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Implements the banking system for managing the player's gold currency.
/// Handles deposits, withdrawals, and notifies listeners when the player runs out of gold (bankrupt).
/// </summary>
public class Bank : MonoBehaviour, IBank
{
    /// <summary>
    /// Text display element that shows the current balance to the player.
    /// </summary>
    [SerializeField] private TMP_Text displayBalance;

    /// <summary>
    /// The starting amount of gold the player receives at the beginning of the game.
    /// </summary>
    [SerializeField] private int startingBalance = 150;

    /// <summary>
    /// The current balance of gold the player has.
    /// </summary>
    private int currentBalance;
    public int CurrentBalance => currentBalance;

    /// <summary>
    /// Event triggered when the player's balance becomes negative.
    /// Used to signal game over or loss conditions.
    /// </summary>
    public event Action OnBankrupt;

    /// <summary>
    /// Initializes the bank with the starting balance and updates the display.
    /// Called before Start().
    /// </summary>
    private void Awake()
    {
        currentBalance = startingBalance;
        UpdateBalance();
    }

    /// <summary>
    /// Adds gold to the player's balance.
    /// Called when enemies are defeated to reward the player.
    /// </summary>
    /// <param name="amount">The amount of gold to deposit.</param>
    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        UpdateBalance();
    }

    /// <summary>
    /// Removes gold from the player's balance.
    /// Called when towers are placed or enemies reach the castle.
    /// Triggers OnBankrupt event if balance falls below zero.
    /// </summary>
    /// <param name="amount">The amount of gold to withdraw.</param>
    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        UpdateBalance();

        if (currentBalance < 0)
        {
            OnBankrupt?.Invoke();
        }
    }

    /// <summary>
    /// Updates the balance text display on screen.
    /// </summary>
    private void UpdateBalance()
    {
        if (displayBalance != null)
            displayBalance.text = currentBalance.ToString();
    }
}
