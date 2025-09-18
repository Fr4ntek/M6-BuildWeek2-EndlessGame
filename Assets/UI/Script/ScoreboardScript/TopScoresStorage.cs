using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class RunEntry
{
    public int score;
    public string isoTime;

    public RunEntry(int score)
    {
        this.score = score;
        isoTime = DateTime.UtcNow.ToString("o");
    }
}

[Serializable]
public class TopScoresData
{
    public List<RunEntry> top = new(); // ordinati disc per score
}

public static class TopScoresStorage
{
    private const int MaxTop = 5;
    private static string FilePath =>
        Path.Combine(Application.persistentDataPath, "top_scores.json");

    public static TopScoresData Load()
    {
        try
        {
            if (!File.Exists(FilePath)) return new TopScoresData();
            var json = File.ReadAllText(FilePath);
            return JsonUtility.FromJson<TopScoresData>(json) ?? new TopScoresData();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"TopScores Load error: {e.Message}");
            return new TopScoresData();
        }
    }

    public static void Save(TopScoresData data)
    {
        try
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(FilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"TopScores Save error: {e.Message}");
        }
    }

    /// Inserisce un nuovo score, mantiene solo i migliori 5
    public static TopScoresData AddScore(int score)
    {
        var data = Load();
        data.top.Add(new RunEntry(score));

        // ordina discendente
        data.top.Sort((a, b) => b.score.CompareTo(a.score));

        // tronca
        if (data.top.Count > MaxTop)
            data.top.RemoveRange(MaxTop, data.top.Count - MaxTop);

        Save(data);
        return data;
    }

    public static void ResetAll()
    {
        try { if (File.Exists(FilePath)) File.Delete(FilePath); }
        catch (Exception e) { Debug.LogWarning($"TopScores Reset error: {e.Message}"); }
    }
}
