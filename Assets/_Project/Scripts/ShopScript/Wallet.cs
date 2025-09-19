using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField, Min(0)] private int coins = 0;
    public int Coins => coins;

    // Evento chiamato quando cambia il numero di monete (passa il nuovo valore)
    public event Action<int> OnCoinsChanged;

    // Prova a spendere una certa quantità di monete
    public bool TrySpend(int amount)
    {
        if (amount <= 0) return true;
        if (coins < amount) return false;
        coins -= amount;
        OnCoinsChanged?.Invoke(coins);
        return true;
    }

    // Aggiunge una certa quantità di monete (non negativa)
    public void Add(int amount)
    {
        coins += Mathf.Max(0, amount);
        OnCoinsChanged?.Invoke(coins);
    }
}
