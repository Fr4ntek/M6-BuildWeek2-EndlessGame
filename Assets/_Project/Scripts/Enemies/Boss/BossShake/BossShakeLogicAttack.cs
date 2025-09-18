using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShakeLogicAttack : BoosLogicAttack
{
    [SerializeField] private float timeForEachAttack;
    public override void EnableLogicBoss() { if (player != null) StartCoroutine(AttackRoutine()); }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (Mathf.Abs(player.position.x) < 1) bossAnimation.GoAttackUltimate();
            else if (player.position.x < 1) bossAnimation.GoAttackTwo();
            else if (player.position.x > -1) bossAnimation.GoAttackOne();

            yield return new WaitForSeconds(timeForEachAttack);
        }
    }
}
