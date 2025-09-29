using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    public static GameOverManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (gameOverUI != null)
        {
            GameManager.instance.EndRun();
            gameOverUI.SetActive(true);
            AudioManager.instance.PlayGameOverSound();
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        GameManager.instance.EndRun();
        SceneManager.LoadScene(1);
        AudioManager.instance.PlaySound();
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
        AudioManager.instance.StopSound();
    }
}
