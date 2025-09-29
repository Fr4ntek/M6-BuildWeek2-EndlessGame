using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LifeController : MonoBehaviour
{
    public int _baseLife = 1;
    private int _currentLife;
    public bool HasInvinciblePU { get;  private set; }
    
    public static LifeController instance;

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
        _currentLife = GameManager.instance.SaveData.extraLife + _baseLife;
        HasInvinciblePU = GameManager.instance.SaveData.temporaryInvincibility;

        Debug.Log("BaseLife:" + _baseLife);
        Debug.Log("Potenziamenti attivi:");
        Debug.Log("     --Extra life: " + GameManager.instance.SaveData.extraLife);
        Debug.Log("     --PerdiPeso: " + GameManager.instance.SaveData.perdiPeso);
        Debug.Log("     --Invincibilità: "+ HasInvinciblePU);
        Debug.Log("CurrentLife: " + _currentLife);
    }

    public void LoseLife()
    {
        _currentLife--;
        if (_currentLife >= 1)
        {
            RespawnPlayer();
        }
        else
        {
            GameOverManager.instance.GameOver();
        }
    }

    public void ResetLife()
    {
        _currentLife = _baseLife;
    }
    void RespawnPlayer()
    {
       transform.position = transform.position - new Vector3(0, 0, 10);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Player" && (other.CompareTag("Enemy") || other.CompareTag("Obstacle")))
        {
            LoseLife();
        }
    }

    public void DeactivateInvincibilityPU()
    {
        HasInvinciblePU = false;
    }
}
