using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 25;


    [SerializeField] Bank _Bank;
    // Start is called before the first frame update
    void Start()
    {
        _Bank = FindObjectOfType<Bank>();
    }
    public void RewardGold()
    {
        if (_Bank == null) { return; }
        _Bank.Deposit(goldReward);
    }
    public void StealGold()
    {
        if (_Bank == null) { return; }
        _Bank.Withdraw(goldPenalty);
    }
}
