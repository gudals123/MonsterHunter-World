using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : StateMachineBehaviour
{
    private Rigidbody _rigidbody;

    [Header("Power")]
    private float dodgePower = 8.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        _rigidbody = animator.GetComponentInParent<Rigidbody>();

        Vector3 rollDirection = new Vector3(animator.transform.localRotation.x, 0, animator.transform.localRotation.z);

        Roll(animator, rollDirection);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public void Roll(Animator animator, Vector3 lastMoveDirection)
    {
        _rigidbody.velocity = lastMoveDirection + animator.transform.forward * dodgePower;
        //_rigidbody.rotation = Quaternion.Euler(0, 0, 0);    
    }

}

