using System;
using System.Collections;
using UnityEngine;
    
public class M_PlayerController : MonoBehaviour
{
    [Header("Setting Generic")]
    [SerializeField] private float velocityRestorPosition = 5;

    [Header("Setting Velocity")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float velocitySpeedIncrease = 0.01f;

    [Header("Setting Swap")]
    [SerializeField] private float swapDistance = 4f;
    [SerializeField] private float velocitySwap = 4;

    [Header("Setting Jump")]
    [SerializeField] private float jumpHeight = 2;
    [SerializeField] private float jumpDurationNormal = 1;
    [SerializeField] private float jumpDurationLowGravity = 1;
    [SerializeField] private AnimationCurve jumpCurveNormal;
    [SerializeField] private AnimationCurve jumpCurveLowGravity;

    [Header("Setting Slide")]
    [SerializeField] private float slideHeight = 0.3f;
    [SerializeField] private float slidePositionY = 0.3f;
    [SerializeField] private float slideDuration = 0.75f;

    public event Action<float> SpeedAnimation;
    public event Action Jump;
    public event Action Land;
    public event Action Slide;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private float originalY;
    private float originHeight;
    private float originPositionY;

    private Coroutine jumpCoroutine;
    private Coroutine slideCoroutine;

    private bool isSwapping;
    private bool isJumping;
    private bool isSliding;

    private float jumpDuration;
    private bool hasLowGravity;
    private AnimationCurve jumpCurve;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        originalY = transform.position.y;
        originHeight = capsuleCollider.height;
        originPositionY = capsuleCollider.center.y;

        hasLowGravity = GameManager.instance.SaveData.perdiPeso;
        jumpDuration = hasLowGravity? jumpDurationLowGravity : jumpDurationNormal;
        jumpCurve = hasLowGravity? jumpCurveLowGravity : jumpCurveNormal;
    }

    private void Update()
    {
        InputPlayer();

        if (speed >= maxSpeed) speed = maxSpeed;
        else speed += (Time.deltaTime * velocitySpeedIncrease);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector3.forward * (speed * Time.fixedDeltaTime));

        if (speed >= maxSpeed) return;
        SpeedAnimation?.Invoke(speed/maxSpeed);
    }

    private void InputPlayer()
    {
        if (Input.GetKeyDown(KeyCode.A)) GoLeft();
        if (Input.GetKeyDown(KeyCode.D)) GoRight();

        if (Input.GetKeyDown(KeyCode.Space)) GoUp();
        if (Input.GetKeyDown(KeyCode.S)) GoDown();
    }

    private void GoLeft()
    {
        if (isSwapping) return;

        StartCoroutine(SpwappingRoutine(-swapDistance));
    }

    private void GoRight()
    {
        if (isSwapping) return;

        StartCoroutine(SpwappingRoutine(swapDistance));
    }

    private IEnumerator SpwappingRoutine(float swap)
    {
        isSwapping = true;

        float startX = rb.position.x;
        float targetX = Mathf.Clamp(rb.position.x + swap, -swapDistance, swapDistance);

        float progress = 0f;
        while (progress < 0.85)
        {
            progress += Time.deltaTime * velocitySwap;

            Vector3 pos = rb.position;
            pos.x = Mathf.Lerp(startX, targetX, progress);

            rb.position = pos;
            yield return null;
        }

        Vector3 finalPos = rb.position;
        finalPos.x = targetX;
        rb.position = finalPos;

        isSwapping = false;
    }

    private void GoUp()
    {
        if (isJumping) return;

        isSliding = true;
        isJumping = true;

        if (slideCoroutine != null) { StopCoroutine(slideCoroutine); slideCoroutine = null; }

        jumpCoroutine = StartCoroutine(JumpRoution());
    }
    private void GoDown()
    {
        if (isSliding) return;

        isSliding = true;
        isJumping = true;

        if (jumpCoroutine != null) StopCoroutine(jumpCoroutine); jumpCoroutine = null;

        slideCoroutine = StartCoroutine(SlideRoution());
    }

    private IEnumerator JumpRoution()
    {
        if (transform.position.y != originalY) yield return StartCoroutine(RestorPosRoutine());
        float startY = originalY;

        Jump?.Invoke();
        float progress = 0f;

        while (progress < jumpDuration)
        {
            progress += Time.deltaTime;
            MovePositionY(transform,progress, jumpDuration, startY, jumpHeight, jumpCurve);

            if (progress >= 0.2f) isSliding = false;
            yield return null;
        }
        Land?.Invoke();
        OnFinishAction();
    }

    private IEnumerator SlideRoution()
    {
        if (transform.position.y != originalY) yield return StartCoroutine(RestorPosRoutine());
        float startY = originalY;

        Slide?.Invoke();
        float progress = 0f;

        while (progress < slideDuration)
        {
            progress += Time.deltaTime;

            capsuleCollider.center = new Vector3(0, slidePositionY, 0);
            capsuleCollider.height = slideHeight;

            if (progress >= 0.2f) isJumping = false;
            yield return null;
        }

        OnFinishAction();
    }

    private float MovePositionY(Transform mesh,float progress, float duration, float startY, float Height, AnimationCurve animationCurve)
    {
        float time = Mathf.Clamp01(progress / duration);

        float curveValue = animationCurve.Evaluate(time);
        Vector3 pos = rb.position;
        pos.y = startY + curveValue * Height;
        rb.position = pos;

        return time;
    }

    private void OnFinishAction()
    {
        Vector3 finalPos = rb.position;
        finalPos.y = originalY;

        rb.position = finalPos;

        capsuleCollider.center = new Vector3(0,originPositionY, 0);
        capsuleCollider.height = originHeight;

        isJumping = false;
        isSliding = false;
    }

    private IEnumerator RestorPosRoutine()
    {
        float currentPosY = rb.position.y;
        capsuleCollider.center = new Vector3(0, originPositionY, 0);
        capsuleCollider.height = originHeight;

        float progress = 0f;
        while (progress < 1)
        {
            progress += Time.deltaTime * velocityRestorPosition;

            Vector3 posY = rb.position;
            posY.y = Mathf.SmoothStep(currentPosY, originalY, progress);
            rb.position = posY;

            yield return null;
        }

        Vector3 startPosY = rb.position;
        startPosY.y = originalY;

        rb.position = startPosY;
    }
}
