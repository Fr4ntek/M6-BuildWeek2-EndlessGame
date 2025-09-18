using UnityEngine;
using TMPro;

public class TopScoresUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] rows = new TextMeshProUGUI[5];
    [SerializeField] private string rowFormat = "#{0}  {1}"; // posizione + score

    private void OnEnable() => Refresh();

    public void Refresh()
    {
        var data = TopScoresStorage.Load();

        for (int i = 0; i < rows.Length; i++)
        {
            if (!rows[i]) continue;

            if (i < data.top.Count)
            {
                var e = data.top[i];
                rows[i].text = string.Format(rowFormat, i + 1, e.score);
            }
            else
            {
                rows[i].text = $"#{i + 1}  —";
            }
        }
    }

    public void RegisterScore(int score)
    {
        TopScoresStorage.AddScore(score);
        Refresh();
    }
}
