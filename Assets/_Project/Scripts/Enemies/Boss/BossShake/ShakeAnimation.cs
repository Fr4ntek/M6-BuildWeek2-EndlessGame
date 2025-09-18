using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnimation : BossAnimation
{
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Vector3 offsetRotation;
    [SerializeField] private float speedIdleMovement = 0.5f;

    [SerializeField] private Transform head;
    [SerializeField] private Transform lastTail;

    [Header("Idle")]
    [SerializeField] private Vector3 headIdlePositionOne;
    [SerializeField] private Vector3 headIdleRotationOne;

    [SerializeField] private Vector3 headIdlePositionTwo;
    [SerializeField] private Vector3 headIdleRotationTwo;

    [Header("EnableLogicBoss Ultimate")]
    [SerializeField] private Vector3 headAttackPositionOne;
    [SerializeField] private Vector3 headAttackRotationOne;

    [SerializeField] private Vector3 headAttackPositionTwo;
    [SerializeField] private Vector3 headAttackRotationTwo;

    [SerializeField] private Vector3 headAttackPositionTree;
    [SerializeField] private Vector3 headAttackRotationTree;

    private bool isOnAnimationStar;
    private Vector3 startLocalPositionHead;
    private Quaternion startLocalRotationHead;

    private Vector3 originalForceTail;

    private bool lookPlayer;

    private void Start()
    {
        startLocalPositionHead = head.localPosition;
        startLocalRotationHead = head.localRotation;

        if (lastTail.TryGetComponent(out ConstantForce constant)) originalForceTail = constant.force;

        StartCoroutine(IdleShakeRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) GoAttackOne();
        if (Input.GetKeyDown(KeyCode.Alpha2)) GoAttackTwo();
        if (Input.GetKeyDown(KeyCode.Alpha3)) GoAttackUltimate();
    }

    private void LateUpdate()
    {
        if (!lookPlayer) return;
        head.LookAt(lookTarget);
        head.rotation *= Quaternion.Euler(offsetRotation);
    }


    [ContextMenu("GoToDisableAnimation")]
    public override void GoToDisableAnimation()
    {
        if (isOnAnimationStar) return;
        StopAllCoroutines();
        lookPlayer = false;
        isOnAnimationStar = true;
        StartCoroutine(GoToStarAnimationRoutione());
    }

    private IEnumerator IdleShakeRoutine()
    {
        lookPlayer = true;
        while (true)
        {
            Vector3 currentPosition = head.localPosition;
            Quaternion currentRotation = head.localRotation;

            yield return StartCoroutine(LerpPositionRotationRoutine(head, speedIdleMovement, currentPosition, currentRotation, headIdlePositionOne, Quaternion.Euler(headIdleRotationOne), true));

            currentPosition = head.localPosition;
            currentRotation = head.localRotation;
            yield return StartCoroutine(LerpPositionRotationRoutine(head, speedIdleMovement, currentPosition, currentRotation, headIdlePositionTwo, Quaternion.Euler(headIdleRotationTwo), true));
        }
    }

    [ContextMenu("GoIdle")]
    public override void GoIdle()
    {
        if (isOnAnimationStar) return;
        StopAllCoroutines();
        StartCoroutine(IdleShakeRoutine());
    }

    [ContextMenu("AttackOne")]
    public override void GoAttackOne()
    {
        if (isOnAnimationStar) return;
        StopAllCoroutines();
        StartCoroutine(AttackOneRoutine(180, -2f, 0, -300));
    }

    [ContextMenu("GoAttackTwo")]
    public override void GoAttackTwo()
    {
        if (isOnAnimationStar) return;
        Debug.Log("GoAttackTwo");
        StopAllCoroutines();
        StartCoroutine(AttackOneRoutine(-180, 1.5f, 0, 300));
    }

    [ContextMenu("GoAttackUltimate")]
    public override void GoAttackUltimate()
    {
        if (isOnAnimationStar) return;
        StopAllCoroutines();
        StartCoroutine(AttackShankeRoutine());
    }


    private IEnumerator AttackShankeRoutine()
    {
        yield return StartCoroutine(PreparationToIlde(1));

        Vector3 currentPosition = head.localPosition;
        Quaternion currentRotation = head.localRotation;

        Vector3 targetForceTail;
        if (lastTail.TryGetComponent(out ConstantForce constant))
        {
            targetForceTail = new Vector3(originalForceTail.x, 500, originalForceTail.z);
            constant.force = targetForceTail;
        }

        //Up
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 3, currentPosition, currentRotation, headAttackPositionOne, Quaternion.Euler(headAttackRotationOne), false));

        currentPosition = head.localPosition;
        currentRotation = head.localRotation;
        //Down
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 1.5f, currentPosition, currentRotation, headAttackPositionTwo, Quaternion.Euler(headAttackRotationTwo), false));

        currentPosition = head.localPosition;
        currentRotation = head.localRotation;
        //EnableLogicBoss
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 1, currentPosition, currentRotation, headAttackPositionTree, Quaternion.Euler(headAttackRotationTree), false));


        currentPosition = head.localPosition;
        currentRotation = head.localRotation;
        //Return Up
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 1, currentPosition, currentRotation, headAttackPositionOne, Quaternion.Euler(headAttackRotationOne), true));
        if (constant != null) constant.force = originalForceTail;

        //Start Position
        currentPosition = head.localPosition;
        currentRotation = head.localRotation;
        yield return StartCoroutine(LerpPositionRotationRoutine(head, speedIdleMovement, currentPosition, currentRotation, startLocalPositionHead, startLocalRotationHead, true));
        StartCoroutine(IdleShakeRoutine());
    }



    private IEnumerator AttackOneRoutine(float angle, float posX, float targetX, float constanceForceX)
    {
        yield return StartCoroutine(PreparationToIlde(1));

        Vector3 targetForceTail;

        Vector3 currentPosition = head.localPosition;
        Quaternion currentRotation = head.localRotation;

        Vector3 headPosAttackOne = currentPosition + new Vector3(posX, -0.5f, -2);


        if (lastTail.TryGetComponent(out ConstantForce constant))
        {
            targetForceTail = new Vector3(constanceForceX, originalForceTail.y, originalForceTail.z);
            constant.force = targetForceTail;
        }

        Debug.Log("Fase1");
        //Preparation EnableLogicBoss
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 3, currentPosition, currentRotation, headPosAttackOne, currentRotation, true));


        float flipX = posX > 0 ? -posX * posX : Mathf.Abs(posX) * Mathf.Abs(posX/1.5f);
        headPosAttackOne = currentPosition + new Vector3(flipX, -0.5f, -10);

        currentPosition = head.localPosition;
        currentRotation = head.localRotation;
        Debug.Log("Fase2");
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 3, currentPosition, currentRotation, headPosAttackOne, currentRotation, false));

        currentPosition = head.localPosition;
        currentRotation = head.localRotation;

        Vector3 headRotAttackTwo = new Vector3(0, angle, 0);

        if (constant != null)
        {
            targetForceTail = new Vector3(constant.force.x, originalForceTail.y, -2000);
            constant.force = targetForceTail;
        }

        Vector3 currentPositionTail = lastTail.localPosition;
        Quaternion currentRotationTail = lastTail.localRotation;

        Vector3 targetPositionTail = new Vector3(0, -2.5f, -15);
        //Rotation And EnableLogicBoss
        Debug.Log("Fase3");
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 1f, currentPosition, currentRotation, currentPosition, Quaternion.Euler(headRotAttackTwo), false));

        yield return new WaitForSeconds(0.5f);

        constant.force = originalForceTail;
        yield return StartCoroutine(PreparationToIlde(2));
        StartCoroutine(IdleShakeRoutine());
    }


    private IEnumerator GoToStarAnimationRoutione()
    {
        Vector3 currentPosition = head.localPosition;
        Quaternion currentRotation = head.localRotation;

        //
        Quaternion targetRotPrimary = Quaternion.Euler(0f, 0f, 0f);
        Quaternion targetRotSecondary = Quaternion.Euler(0f, 0f, 0f);

        //

        Debug.Log("GoToStarAnimation");
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 2, currentPosition, currentRotation, startLocalPositionHead, startLocalRotationHead,true));
        Vector3 targetPosition = head.localPosition + new Vector3(0, 20, 0);
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 1, startLocalPositionHead, startLocalRotationHead, targetPosition, startLocalRotationHead,false));

        //root.gameObject.SetActive(false);
        //root.localPosition = new Vector3(0, 0, 0);

        StartCoroutine(DisableRoutine());
    }

    private IEnumerator DisableRoutine()
    {
        //yield return null;
        //body.localPosition = startLocalPosition;
        //body.localRotation = startLocalRotation;

        //yield return new WaitForSeconds(1f);
        yield return null;
        isOnAnimationStar = false;
        gameObject.SetActive(false);
        //head.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(GoToEnableRoutine());
    }

    private void OnDisable()
    {
        if (lastTail.TryGetComponent(out ConstantForce constant))
        {
            constant.force = originalForceTail;
        }
    }

    private IEnumerator GoToEnableRoutine()
    {
        yield return null;
        transform.position = new Vector3(transform.position.x, transform.position.y, lookTarget.position.z + 100);
        head.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        Vector3 currentPos = head.localPosition;
        Vector3 targetPos = new Vector3(0, 0, 0);
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 2, currentPos, head.localRotation, targetPos, head.localRotation,true));

        GoIdle();
    }


    private IEnumerator PreparationToIlde(float velocityToIlde = 2)
    {
        if (lastTail.TryGetComponent(out ConstantForce constant)) constant.force = originalForceTail;

        Vector3 currentPosition = head.localPosition;
        Quaternion currentRotation = head.localRotation;
        lookPlayer = false;

        yield return StartCoroutine(LerpPositionRotationRoutine(head, velocityToIlde, currentPosition, currentRotation, new Vector3(0, -1, 0), startLocalRotationHead, true));
    }

    private IEnumerator LerpPositionRotationRoutine(Transform target, float velocity, Vector3 currentPos, Quaternion currentRot, Vector3 targetPos, Quaternion targetRot, bool isSmooth)
    {
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * velocity;
            float smooth = Mathf.SmoothStep(0, 1, progress);

            float t = isSmooth ? smooth : progress;

            target.localPosition = Vector3.Lerp(currentPos, targetPos, t);
            target.localRotation = Quaternion.Lerp(currentRot, targetRot, t);

            yield return null;
        }
        target.localPosition = targetPos;
        target.localRotation = targetRot;
    }
}
