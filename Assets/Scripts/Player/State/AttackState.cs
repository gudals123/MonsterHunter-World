using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private float attackStartTime;
    private float maxChargingTime = 4;
    private float chargingTime;
    private float damege = 20;
    private bool isCharging;
    private int comboCount;
    private Animator _animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator = animator.GetComponent<Animator>();
        _animator.speed = 0.5f;
        chargingTime = 0;
        isCharging = true;

        attackStartTime = Time.time;
        if (animator.GetBool(PlayerAnimatorParamiter.IsRightAttak))
        {
            if (comboCount == 0)
            {
                damege = 30;
            }
            if (comboCount == 1)
            {
                damege = 40;
            }
            BattleManager._playerAttackDamege = damege;
        }
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chargingTime += Time.deltaTime;

        if (animator.GetBool(PlayerAnimatorParamiter.IsRightAttak))
        {
            if (comboCount == 0)
            {
                damege = 30;
            }
            else if(comboCount == 1)
            {
                damege = 40;
            }
            
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
        }
        if (!isCharging || Time.time >= attackStartTime + maxChargingTime)
        {
            _animator.speed = 0.5f;

            if(chargingTime < 3)
            {
                damege = 20;
            }
            else if (chargingTime < 4)
            {
                damege = 50;
            }
            else
            {
                damege = 100;
            }
            return;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator.speed = 1f;
        chargingTime = 0;
        comboCount++;
        if (comboCount >= 2)
        {
            comboCount = 0;
        }
        animator.SetInteger(PlayerAnimatorParamiter.ComboCount, comboCount);
        animator.SetBool(PlayerAnimatorParamiter.IsAttacking, false);

        BattleManager._playerAttackDamege = damege;
    }


}
