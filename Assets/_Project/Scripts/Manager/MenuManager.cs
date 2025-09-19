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
        // All'inizio mostra il menu principale
        ShowMainMenu();
    }

    // Produce il suono al click del bottone
    public void PlayButtonSound()
    {
        maxaudio.PlayOneShot(_buttonSound);
    }

    // Mostra il canvas del menu principale
    public void ShowMainMenu()
    {
        mainMenu.enabled = true;
        leaderBoard.enabled = false;
        shopMenu.enabled = false;
    }

    // Mostra il canvas dello shop
    public void ShowShop()
    {
        mainMenu.enabled = false;
        leaderBoard.enabled = false;
        shopMenu.enabled = true;
        //UpdateShopUI();
    }

    // Mostra il canvas della classifica
    public void ShowScore()
    {
        mainMenu.enabled = false;
        leaderBoard.enabled = true;
        shopMenu.enabled = false;
        //UpdateScoreUI();
    }

    // Aggiorna il testo delle monete nello shop
    // non più usato, ora lo fa lo script Wallet, ShopPanel e ShopSlotUI
    public void UpdateShopUI()
    {
        coinText.text = GameManager.instance.saveData.totalCoins.ToString();
    }

    // Aggiorna il testo della classifica
    public void UpdateScoreUI()
    {
        scoreText.text = "🏆 Leaderboard 🏆\n";
        int rank = 1;
        foreach (int score in GameManager.instance.saveData.leaderboardDistances)
        {
            scoreText.text += rank + ". " + score + " m\n";
            rank++;
        }

        if (GameManager.instance.saveData.leaderboardDistances.Count == 0)
        {
            scoreText.text += "Nessun punteggio ancora registrato!";
        }
    }

    // Carica la scena del gioco (indice 1)
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    // Chiude l'applicazione
    public void QuitGame()
    {
        Application.Quit();
    }

}


