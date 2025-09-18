using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossType
{
    public BoosLogicAttack boss;
    public float timeOnSpot = 10;
}

public class BossManager : MonoBehaviour
{
    [SerializeField] private float timeForshow = 2;

    [SerializeField] private List<BossType> bossList;

    private void Start()
    {
        foreach (BossType bo in bossList) if(bo.boss.gameObject.activeInHierarchy) bo.boss.BossAnimation.GoToDisableAnimation();
        StartCoroutine(ShowAndHideRoutine());
    }

    private IEnumerator ShowAndHideRoutine()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            int randomBoss = UnityEngine.Random.Range(0, bossList.Count);
            Debug.Log("Boss type " + randomBoss);
            yield return new WaitForSeconds(timeForshow);
            bossList[randomBoss].boss.gameObject.SetActive(true);
            //boss[0].BossAnimation.GoToEnableAnimation(boss[0].Player);

            yield return new WaitForSeconds(bossList[randomBoss].timeOnSpot);
            bossList[randomBoss].boss.BossAnimation.GoToDisableAnimation();
        }
    }
}
