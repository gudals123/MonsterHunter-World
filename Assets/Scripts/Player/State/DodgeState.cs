using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : StateMachineBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerController _playerController;

    [Header("Power")]
    private float dodgePower = 8.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        _rigidbody = animator.GetComponent<Rigidbody>();
        _playerController = animator.GetComponent<PlayerController>();

        Vector3 rollDirection = new Vector3(animator.transform.localRotation.x, 0, animator.transform.localRotation.z);
        Roll(animator, rollDirection);
    }

    public void Roll(Animator animator, Vector3 lastMoveDirection)
    {
        _rigidbody.velocity = lastMoveDirection + animator.transform.forward * dodgePower;

    }

}

