using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;
    [SerializeField] float buildTimer = .3f;

    private void Start()
    {
        StartCoroutine(Build());
    }

    IEnumerator Build()
    {
        foreach (Transform _Child in transform)
        {
            _Child.gameObject.SetActive(false);
            foreach (Transform grandChild in _Child)
            {
                grandChild.gameObject.SetActive(false);
            }
        }

        foreach (Transform _Child in transform)
        {
            _Child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildTimer);
            foreach (Transform grandChild in _Child)
            {
                grandChild.gameObject.SetActive(true);
            }
        }
    }

    public bool CreateTower(Tower towerWizard, Vector3 position)
    {
        Bank _Bank = FindObjectOfType<Bank>();
        if (_Bank == null) { return false; }
        if(_Bank.CurrentBalance >= cost)
        {
            Instantiate(towerWizard.gameObject, position, Quaternion.identity);
            _Bank.Withdraw(cost);
            return true;
        }
        return false;
    }



}
