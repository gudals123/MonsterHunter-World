using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorParamiter
{
    public static readonly int IsMoveing = Animator.StringToHash("IsMoveing");
    public static readonly int IsWalk = Animator.StringToHash("IsWalk");
    public static readonly int IsRun = Animator.StringToHash("IsRun");
    public static readonly int IsDodge = Animator.StringToHash("IsDodge");
    public static readonly int IsGetHit = Animator.StringToHash("IsGetHit");
    public static readonly int IsDead = Animator.StringToHash("IsDead");
    public static readonly int IsGrouned = Animator.StringToHash("IsGrouned");
    public static readonly int IsFall = Animator.StringToHash("IsFall");
    public static readonly int IsArmed = Animator.StringToHash("IsArmed");
    public static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
}
