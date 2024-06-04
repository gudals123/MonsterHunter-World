using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSwordState : StateMachineBehaviour
{
    private PlayerController _playerController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController = animator.GetComponentInParent<PlayerController>();

        _playerController.IsSwitchDone = false; 
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.IsSwitchDone = true;
    }
}
