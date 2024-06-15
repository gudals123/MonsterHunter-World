using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private int comboCount;
    private Animator _animator;
    private PlayerController _playerController;
    private bool isAppliedAnimatorSpeedDone;



    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator = animator.GetComponent<Animator>();
        _playerController = _animator.GetComponentInParent<PlayerController>();
        _animator.speed = 0.5f;
        isAppliedAnimatorSpeedDone = false;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*if(Time.time - CombatManager.Instance._chargingStartTime > _maxChargingTime)
        {
            CombatManager.Instance._isCharging = false;
        }
        */
        if (!_playerController.isCharging && !isAppliedAnimatorSpeedDone)
        {
            _animator.speed = 0.5f;
            isAppliedAnimatorSpeedDone = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
