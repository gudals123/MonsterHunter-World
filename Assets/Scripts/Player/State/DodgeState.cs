using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(PlayerAnimatorParamiter.IsSwitchDone, false);
        //���׹̳� �ý���

        // ������ ���

    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(PlayerAnimatorParamiter.IsSwitchDone, true);


    }



}

