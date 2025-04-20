using System;

public interface IBank
{
    int CurrentBalance { get; }
    event Action OnBankrupt;

    void Deposit(int amount);
    void Withdraw(int amount);
}
