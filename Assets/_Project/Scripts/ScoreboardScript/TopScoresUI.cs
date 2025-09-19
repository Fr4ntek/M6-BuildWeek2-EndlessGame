using UnityEngine;
using TMPro;

public class TopScoresUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] rows = new TextMeshProUGUI[5];
    [SerializeField] private string rowFormat = "#{0}  {1}"; // posizione + score

    private void OnEnable() => Refresh();

    // Aggiorna la UI con i dati salvati in GameManager.instance.saveData
    public void Refresh()
    {
        if (GameManager.instance == null)
            return;

        var distances = GameManager.instance.saveData.leaderboardDistances;

        for (int i = 0; i < rows.Length; i++)
        {
            if (!rows[i]) continue;

            if (i < distances.Count)
            {
                rows[i].text = string.Format(rowFormat, i + 1, distances[i]);
            }
            else
            {
                rows[i].text = string.Format(rowFormat, i + 1, "—");
            }
        }
    }

    // Registra un nuovo punteggio usando GameManager e salva su file
    public void RegisterScore(int score)
    {
        if (GameManager.instance == null)
            return;

        GameManager.instance.SaveRunDistance(score);
        GameManager.instance.SaveDataToFile();
        Refresh();
    }
}
