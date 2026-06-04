using System;

/// <summary>
/// Defines the contract for a banking system that manages the player's currency (gold).
/// Implementations should handle deposits, withdrawals, and bankruptcy notifications.
/// </summary>
public interface IBank
{
    /// <summary>
    /// Gets the current balance of gold the player has.
    /// </summary>
    int CurrentBalance { get; }

    /// <summary>
    /// Event triggered when the bank's balance falls below zero (bankruptcy condition).
    /// Listeners can use this to trigger game over states or end game scenarios.
    /// </summary>
    event Action OnBankrupt;

    /// <summary>
    /// Deposits the specified amount of gold into the bank.
    /// </summary>
    /// <param name="amount">The amount of gold to deposit. Should be non-negative.</param>
    void Deposit(int amount);

    /// <summary>
    /// Withdraws the specified amount of gold from the bank.
    /// If the withdrawal causes the balance to go below zero, the OnBankrupt event is triggered.
    /// </summary>
    /// <param name="amount">The amount of gold to withdraw. Should be non-negative.</param>
    void Withdraw(int amount);
}
