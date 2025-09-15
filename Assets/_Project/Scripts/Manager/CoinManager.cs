using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    public int _coinCount = 0;

    public static CoinManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddCoin()
    {
        _coinCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        coinsText.text = "Coins: " + _coinCount;
    }

}
