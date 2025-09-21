using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Rendering;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SaveData SaveData { get; private set; }

    // Event fired when the coin count changes. Subscribers get the new coin total.
    public System.Action<int> OnCoinsChanged;

    private string saveFilePath;

    private void Awake()
    {
        SaveData = new SaveData();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndRun()
    {
       
        AddCoinsFromRun(CoinManager.Instance._coinCount);

        
        int runDistance = DistanceCounter.instance.GetDistance();
        SaveRunDistance(runDistance);

       
        CoinManager.Instance._coinCount = 0;
        DistanceCounter.instance.ResetDistance();

        SaveDataToFile();
    }

    
    public void AddCoinsFromRun(int coins)
    {
        SaveData.totalCoins += coins;
        OnCoinsChanged?.Invoke(SaveData.totalCoins);
    }

    // Try to spend coins. Returns true if successful and raises the OnCoinsChanged event.
    public bool TrySpend(int amount)
    {
        if (amount <= 0) return false;
        if (SaveData.totalCoins < amount) return false;

        SaveData.totalCoins -= amount;
        OnCoinsChanged?.Invoke(SaveData.totalCoins);
        return true;
    }

    // Add coins (generic method) and notify listeners
    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        SaveData.totalCoins += amount;
        OnCoinsChanged?.Invoke(SaveData.totalCoins);

    }

    
    public void SaveRunDistance(int distance)
    {
        SaveData.leaderboardDistances.Add(distance);
        SaveData.leaderboardDistances.Sort((a, b) => b.CompareTo(a));

        if (SaveData.leaderboardDistances.Count > 10) 
            SaveData.leaderboardDistances.RemoveAt(SaveData.leaderboardDistances.Count - 1);
    }

    
    public void SaveDataToFile()
    {
        Debug.LogWarningFormat("Saving data to {0}", saveFilePath);
        string json = JsonUtility.ToJson(SaveData, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData = JsonUtility.FromJson<SaveData>(json);
            // notify listeners about loaded coin total
            OnCoinsChanged?.Invoke(SaveData.totalCoins);
        }
        else
        {
            SaveData = new SaveData();
        }
    }
}

