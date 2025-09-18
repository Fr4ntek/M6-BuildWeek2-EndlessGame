using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SaveData SaveData { get; private set; }

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
        string json = JsonUtility.ToJson(SaveData, true);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            SaveData = new SaveData();
        }
    }
}

