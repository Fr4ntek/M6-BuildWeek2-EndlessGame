using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public int baseLife = 1;
    private int currentLife;
    

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
        Debug.Log("BaseLife:" + baseLife);
        Debug.Log("Potenziamenti attivi:");
        Debug.Log("--Extra life: " + GameManager.instance.SaveData.extraLife);
        Debug.Log("--PerdiPeso:" + GameManager.instance.SaveData.perdiPeso);
        Debug.Log("--Invincibilità:"+ GameManager.instance.SaveData.temporaryInvincibility);
      
        currentLife = GameManager.instance.SaveData.extraLife + baseLife;
        Debug.Log("currentLife: " + currentLife);
    }

    public void LoseLife()
    {
        currentLife--;
        if (currentLife >= 1)
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
        currentLife = baseLife;
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
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            LoseLife();
        }
    }
}
