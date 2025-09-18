using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_PlayerAnimation : MonoBehaviour
{
    [SerializeField] private string paramNameSpeed = "Speed";
    [SerializeField] private string paramNameJump = "Jump";
    [SerializeField] private string paramNameLand = "Land";
    [SerializeField] private string paramNameSlide = "Slide ";

    private M_PlayerController controller;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<M_PlayerController>();

        SetUpAction();
    }

    private void SetUpAction()
    {
        if(controller == null) return;
        controller.SpeedAnimation += OnChangeSpeed;
        controller.Jump += OnJump;
        controller.Land += OnLand;
        controller.Slide += OnSlide;    
    }

    private void OnChangeSpeed(float speed) => animator.SetFloat(paramNameSpeed, speed);

    private void OnJump() => animator.SetTrigger(paramNameJump);
    private void OnLand() => animator.SetTrigger(paramNameLand);
    private void OnSlide() => animator.SetTrigger(paramNameSlide);


    private void OnDisable()
    {
        if(controller == null) return;
        controller.SpeedAnimation -= OnChangeSpeed;
        controller.Jump -= OnJump;
        controller.Land -= OnLand;
        controller.Slide -= OnSlide;
    }
}
