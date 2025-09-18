using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField, Min(0)] private int coins = 0;
    public int Coins => coins;

    public event Action<int> OnCoinsChanged;

    public bool TrySpend(int amount)
    {
        if (amount <= 0) return true;
        if (coins < amount) return false;
        coins -= amount;
        OnCoinsChanged?.Invoke(coins);
        return true;
    }

    public void Add(int amount)
    {
        coins += Mathf.Max(0, amount);
        OnCoinsChanged?.Invoke(coins);
    }
}
