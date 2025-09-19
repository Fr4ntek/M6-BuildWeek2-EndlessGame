using UnityEngine;

public class TopScoresTestHarness : MonoBehaviour
{
    [Header("UI (opzionale)")]
    public TopScoresUI topScoresUI;

    [Header("Range punteggi random")]
    public int minScore = 50;
    public int maxScore = 3000;

    void Start()
    {
        if (topScoresUI) topScoresUI.Refresh();
        Debug.Log("[TopScoresTest] Tasti: A=add random, 1..5=add fissi, R=reset, P=print");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) AddRandom();
        if (Input.GetKeyDown(KeyCode.Alpha1)) AddFixed(100);
        if (Input.GetKeyDown(KeyCode.Alpha2)) AddFixed(500);
        if (Input.GetKeyDown(KeyCode.Alpha3)) AddFixed(1200);
        if (Input.GetKeyDown(KeyCode.Alpha4)) AddFixed(2000);
        if (Input.GetKeyDown(KeyCode.Alpha5)) AddFixed(2800);
        if (Input.GetKeyDown(KeyCode.R)) ResetAll();
        if (Input.GetKeyDown(KeyCode.P)) PrintToConsole();
    }

    void OnGUI()
    {
        const int w = 160, h = 32, pad = 10;
        int x = 10, y = 10;

        if (GUI.Button(new Rect(x, y + 30, w, h), "Add Random")) { AddRandom(); y += h + pad; }
        if (GUI.Button(new Rect(x, y + 30 + 30, w, h), "Add 100")) { AddFixed(100); y += h + pad; }
        if (GUI.Button(new Rect(x, y + 30 + 30 + 30, w, h), "Add 500")) { AddFixed(500); y += h + pad; }
        if (GUI.Button(new Rect(x, y + 30 + 30 + 30 + 30, w, h), "Add 1200")) { AddFixed(1200); y += h + pad; }
        if (GUI.Button(new Rect(x, y + 30 + 30 + 30 + 30 + 30, w, h), "Add 2000")) { AddFixed(2000); y += h + pad; }
        if (GUI.Button(new Rect(x, y + 30 + 30 + 30 + 30 + 30 + 30, w, h), "Add 2800")) { AddFixed(2800); y += h + pad; }
        if (GUI.Button(new Rect(x, y + 30 + 30 + 30 + 30 + 30 + 30 + 30, w, h), "Reset")) { ResetAll(); y += h + pad; }
        if (GUI.Button(new Rect(x, y + 30 + 30 + 30 + 30 + 30 + 30 + 30 + 30, w, h), "Print")) { PrintToConsole(); }
    }

    void AddRandom()
    {
        int s = Random.Range(minScore, maxScore + 1);
        TopScoresStorage.AddScore(s);
        if (topScoresUI) topScoresUI.Refresh();
        Debug.Log($"[TopScoresTest] Added random score: {s}");
    }

    void AddFixed(int s)
    {
        TopScoresStorage.AddScore(s);
        if (topScoresUI) topScoresUI.Refresh();
        Debug.Log($"[TopScoresTest] Added fixed score: {s}");
    }

    void ResetAll()
    {
        TopScoresStorage.ResetAll();
        if (topScoresUI) topScoresUI.Refresh();
        Debug.Log("[TopScoresTest] Reset all.");
    }

    void PrintToConsole()
    {
        var data = TopScoresStorage.Load();
        string msg = "[TopScoresTest] Top scores: ";
        for (int i = 0; i < data.top.Count; i++)
            msg += (i == 0 ? "" : ", ") + data.top[i].score;
        Debug.Log(msg);
    }
}
