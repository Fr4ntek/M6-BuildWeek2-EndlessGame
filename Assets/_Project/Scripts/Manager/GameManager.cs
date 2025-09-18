using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SaveData saveData = new SaveData();

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
        }
        else
        {
            saveData = new SaveData();
        }
    }
}

