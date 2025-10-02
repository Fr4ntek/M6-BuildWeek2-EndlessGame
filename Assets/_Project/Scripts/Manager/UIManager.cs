using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }

    [SerializeField] private TextMeshProUGUI coinOnRun;
    [SerializeField] private TextMeshProUGUI distancePlayer;
    [SerializeField] private TextMeshProUGUI lifePlayer;
    [SerializeField] private GameObject invincibilityObj;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        HasInvincibility(GameManager.instance.SaveData.temporaryInvincibility);

        UpdateLifePlayer(GameManager.instance.SaveData.extraLife + 1);
    }


    public void AddCoin(int value) => coinOnRun.text = value.ToString();

    public void UpdateDistancePlayer(int value) => distancePlayer.text = value.ToString();

    public void UpdateLifePlayer(int value) => lifePlayer.text = value.ToString();

    public void HasInvincibility(bool value) => invincibilityObj.SetActive(value);
}
