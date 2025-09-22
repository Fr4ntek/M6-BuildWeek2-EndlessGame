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
    [SerializeField] private AudioSource maxAudio;
    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private AudioClip _cashAudio;
    
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
        maxAudio.PlayOneShot(_buttonSound);
    }

    public void PlayCashSound()
    {
        maxAudio?.PlayOneShot(_cashAudio);
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
    }

    // Mostra il canvas della classifica
    public void ShowScore()
    {
        mainMenu.enabled = false;
        leaderBoard.enabled = true;
        shopMenu.enabled = false;
    }

    // Carica la scena del gioco (indice 1)
    public void NewGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    // Chiude l'applicazione
    public void QuitGame()
    {
        Application.Quit();
    }

}


