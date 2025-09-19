using UnityEditor;
using UnityEngine;

public static class TopScoresEditorTools
{
    [MenuItem("Tools/Top Scores/Seed Sample Data")]
    public static void Seed()
    {
        TopScoresStorage.ResetAll();
        int[] samples = { 350, 1200, 980, 2100, 1600, 500, 2400 };
        foreach (var s in samples) TopScoresStorage.AddScore(s);
        Debug.Log("[TopScoresEditor] Seeded sample scores.");
    }

    [MenuItem("Tools/Top Scores/Reset")]
    public static void Reset()
    {
        TopScoresStorage.ResetAll();
        Debug.Log("[TopScoresEditor] Reset top scores.");
    }

    [MenuItem("Tools/Top Scores/Print")]
    public static void Print()
    {
        var data = TopScoresStorage.Load();
        string msg = "[TopScoresEditor] Top scores: ";
        for (int i = 0; i < data.top.Count; i++)
            msg += (i == 0 ? "" : ", ") + data.top[i].score;
        Debug.Log(msg);
    }
}