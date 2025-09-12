using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosLogicAttack : MonoBehaviour
{
    [SerializeField] protected Transform player;

    protected BossAnimation bossAnimation;

    public virtual void Start() => bossAnimation.GetComponent<BossAnimation>();

    public virtual void Attack() { }

}
