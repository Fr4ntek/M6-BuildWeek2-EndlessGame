using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    public virtual void GoIdle() { }

    public virtual void GoAttackOne() { }
    public virtual void GoAttackTwo() { }
    public virtual void GoAttackUltimate() { }

    public virtual void GoToEnableAnimation(Transform target) { }
    public virtual void GoToDisableAnimation() { }

    public virtual void StopCoroutineForBoss() => StopAllCoroutines();
}
