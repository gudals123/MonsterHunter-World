using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(PlayerAnimatorParamiter.IsSwitchDone, false);
        //스테미너 시스템

        // 구르기 모션

    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(PlayerAnimatorParamiter.IsSwitchDone, true);


    }



}

