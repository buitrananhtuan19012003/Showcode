using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoin : PlayerAbstract
{
    [SerializeField] private int currentCoin = 1000;
    public int CurrentCoin { get { return currentCoin; } }

    public void SubtractCoin(int amount)
    {
        currentCoin -= amount;
    }

    public void AddCoin(int amount)
    {
        currentCoin += amount;
    }
}
