using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private int comboCount;
    private float lastTimeAttacked;
    private float comboWindow = 2;
    private float attackStartTime;
    private float maxChargingTime = 3;
    private float minChargingTime = 2.1f;
    private const float chargingMaxDamege = 100;
    private float damege;
    private float currentDamege;
    private float currentPoint;
    private bool isCharging;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(Time.time);
        animator.GetComponent<Animator>().speed = 0.5f;
        isCharging = true;
        if (!animator.GetBool(PlayerAnimatorParamiter.IsRightAttak))
        {
            attackStartTime = Time.time;
            return;
        }
        if (comboCount >= 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCount = 0;
        }
        animator.SetInteger(PlayerAnimatorParamiter.ComboCount, comboCount);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentPoint = (Time.time - attackStartTime + minChargingTime) / (maxChargingTime + minChargingTime);
        currentDamege = Mathf.Lerp(20,100, currentPoint);

        if (Time.time >= attackStartTime + maxChargingTime)
        {
            isCharging = false;
            animator.GetComponent<Animator>().speed = 0.5f;
            damege = chargingMaxDamege;
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
        }
        if (!isCharging)
        {
            animator.GetComponent<Animator>().speed = 0.5f;
            damege = currentDamege;
            return;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Animator>().speed = 1f;
        if (!animator.GetBool(PlayerAnimatorParamiter.IsRightAttak))
        {
            Debug.Log($"damege : {damege}");
            Debug.Log($"currentPoint : {currentPoint}");
            //BattleManager.SetDamege(damege);
            return;
        }
        comboCount++;
        lastTimeAttacked = Time.time;
        Debug.Log(Time.time);

    }


}
