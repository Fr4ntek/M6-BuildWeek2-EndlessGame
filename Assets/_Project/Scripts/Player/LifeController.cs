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
        currentLife = baseLife;
    }

    public void LoseLife()
    {
        currentLife--;
        if (currentLife > 1)
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
       transform.position = transform.position - new Vector3(0, 0, -10);

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
