using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosLogicAttack : MonoBehaviour
{
    [SerializeField] protected Transform player;
    [SerializeField] protected BossAnimation bossAnimation;

    public BossAnimation BossAnimation => bossAnimation;
    public Transform Player => player;

    public virtual void EnableLogicBoss() { }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
