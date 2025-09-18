using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas leaderBoard;
    [SerializeField] private Canvas shopMenu;
    [SerializeField] private AudioSource maxaudio;
    [SerializeField] private AudioClip _buttonSound;
    

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
       ShowMainMenu();
    }

    public void PlayButtonSound()
    {
        maxaudio.PlayOneShot(_buttonSound);
    }
    public void ShowMainMenu()
    {
        mainMenu.enabled = true;
        leaderBoard.enabled = false;
        shopMenu.enabled = false;
    }

    public void ShowShop()
    {
        mainMenu.enabled = false;
        leaderBoard.enabled = false;
        shopMenu.enabled = true;
        UpdateShopUI();
    }

    public void ShowScore()
    {
        mainMenu.enabled = false;
        leaderBoard.enabled = true;
        shopMenu.enabled = false;
        UpdateScoreUI();
    }

    public void UpdateShopUI()
    {
        coinText.text = GameManager.instance.SaveData.totalCoins.ToString();
    }

    public void UpdateScoreUI()
    {
        scoreText.text = "🏆 Leaderboard 🏆\n";
        int rank = 1;
        foreach (int score in GameManager.instance.SaveData.leaderboardDistances)
        {
            scoreText.text += rank + ". " + score + " m\n";
            rank++;
        }

        if (GameManager.instance.SaveData.leaderboardDistances.Count == 0)
        {
            scoreText.text += "Nessun punteggio ancora registrato!";
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}


