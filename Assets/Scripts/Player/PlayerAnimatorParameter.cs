using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorParamiter
{
    public static readonly int IsMoving = Animator.StringToHash("IsMoveing");
    public static readonly int IsRun = Animator.StringToHash("IsRun");
    public static readonly int IsRoll = Animator.StringToHash("IsRoll");
    public static readonly int IsGetHit = Animator.StringToHash("IsGetHit");
    public static readonly int IsDead = Animator.StringToHash("IsDead");
    public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    public static readonly int IsArmed = Animator.StringToHash("IsArmed");
    public static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    public static readonly int IsSwitchDone = Animator.StringToHash("IsSwitchDone");
    public static readonly int IsRightAttak = Animator.StringToHash("IsRightAttak");
    public static readonly int IsFall = Animator.StringToHash("IsFall");
    public static readonly int ComboCount = Animator.StringToHash("ComboCount");
}
