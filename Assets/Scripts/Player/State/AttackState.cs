using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private int comboCount;
    private Animator _animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator = animator.GetComponent<Animator>();
        _animator.speed = 0.5f;
        Debug.Log("Enter");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Update");
        if (CombatManager.Instance._isCharging == false)
        {
            _animator.speed = 0.5f;
            Debug.Log("Update_");
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exit");
        _animator.speed = 1f;
        comboCount++;
        if (comboCount >= 2)
        {
            comboCount = 0;
        }
        animator.SetInteger(PlayerAnimatorParamiter.ComboCount, comboCount);
        animator.SetBool(PlayerAnimatorParamiter.IsAttacking, false);

    }


}
