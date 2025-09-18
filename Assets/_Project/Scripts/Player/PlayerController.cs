using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float horizontalSpeed = 2.5f;
    [SerializeField] private float speedIncrease = 3f;   
    [SerializeField] private float nextThreshold = 50f;
    private float _minX = -2.5f;
    private float _maxX = 2.5f;
    private float currentSpeed;
    private float distance;
    private bool isRunning = false;
    private Animator _anim;
    

    

    private void Start()
    {
        currentSpeed = moveSpeed;
        _anim = GetComponent<Animator>();
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
            float posX = Mathf.Clamp(transform.position.x - horizontalSpeed, _minX, _maxX);
            transform.position =new Vector3(posX, transform.position.y, transform.position.z);
        }
            if (Input.GetKeyDown(KeyCode.D))
        {
            float posX = Mathf.Clamp(transform.position.x + horizontalSpeed, _minX, _maxX);
            transform.position =new Vector3(posX, transform.position.y, transform.position.z);
        }

        HandleAnimator();
    }

    void HandleAnimator()
    {
        //if(currentSpeed > 0.1f)
        //{
        //    isRunning = true;
        //    _anim.SetBool("isRunning", isRunning);
        //}
    }
}
