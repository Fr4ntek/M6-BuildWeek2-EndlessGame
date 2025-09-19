using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SaveData saveData = new SaveData();

    // Event fired when the coin count changes. Subscribers get the new coin total.
    public System.Action<int> OnCoinsChanged;

    private string saveFilePath;

    private void Awake()
    {
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
        saveData.totalCoins += coins;
        OnCoinsChanged?.Invoke(saveData.totalCoins);
    }

    // Try to spend coins. Returns true if successful and raises the OnCoinsChanged event.
    public bool TrySpend(int amount)
    {
        if (amount <= 0) return false;
        if (saveData.totalCoins < amount) return false;

        saveData.totalCoins -= amount;
        OnCoinsChanged?.Invoke(saveData.totalCoins);
        return true;
    }

    // Add coins (generic method) and notify listeners
    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        saveData.totalCoins += amount;
        OnCoinsChanged?.Invoke(saveData.totalCoins);
    }

    
    public void SaveRunDistance(int distance)
    {
        saveData.leaderboardDistances.Add(distance);
        saveData.leaderboardDistances.Sort((a, b) => b.CompareTo(a));

        if (saveData.leaderboardDistances.Count > 10) 
            saveData.leaderboardDistances.RemoveAt(saveData.leaderboardDistances.Count - 1);
    }

    
    public void SaveDataToFile()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
            // notify listeners about loaded coin total
            OnCoinsChanged?.Invoke(saveData.totalCoins);
        }
        else
        {
            saveData = new SaveData();
        }
    }
}

