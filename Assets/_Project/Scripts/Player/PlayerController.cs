using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float horizontalSpeed = 2.5f;
    [SerializeField] private float speedIncrease = 3f;   
    [SerializeField] private float nextThreshold = 50f;  
    private float currentSpeed;
    private float distance;
    

    private void Start()
    {
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        // accumula distanza
        distance += currentSpeed * Time.deltaTime;

        // controlla ogni 100 metri
        if (distance >= nextThreshold)
        {
            currentSpeed = Mathf.Min(currentSpeed + speedIncrease, maxSpeed);
            nextThreshold += 100f; // prepara il prossimo checkpoint
        }

        // movimento forward con la velocità corrente
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // movimento laterale
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(Vector3.left * horizontalSpeed);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(Vector3.right * horizontalSpeed);
        }
    }
}
