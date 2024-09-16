using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] private TMP_Text displayBalance;
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    public int CurrentBalance { get => currentBalance; set => currentBalance = value; }

    private void Awake()
    {
        currentBalance = startingBalance;
        UpdateBalance();
    }

    public void Deposit(int _Amount)
    {
        currentBalance += Mathf.Abs(_Amount);
        UpdateBalance();
    }
    public void Withdraw(int _Amount)
    {
        currentBalance -= Mathf.Abs(_Amount);
        UpdateBalance();
        if (currentBalance < 0)
        {
            //lose the game
            ReloadScene();
        }
    }

    void UpdateBalance()
    {
        if (displayBalance == null) { return; }
        displayBalance.text = currentBalance.ToString();
    }

    void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
