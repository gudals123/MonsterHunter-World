using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        _playerController.isAttackDone = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!_playerController.isCharging && !isAppliedAnimatorSpeedDone && _playerController.isAnimationPauseDone)
        {
            _animator.speed = 0.5f;
            _playerController.isChargeAttackDone = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.isAnimationPauseDone = false;
        _playerController.isChargeAttackDone = true;
        _playerController.isAttackDone = true;
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
