using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;
using System;

public class StateAction : ActionBase
{
    int _animationHash;
    private bool _isEnter;
    Animator _animator;
    Action? _onEnterAction;

    public StateAction(string stateName, Action? onEnterAction = null)
    {
        _animationHash = Animator.StringToHash(stateName);
        _onEnterAction = onEnterAction;
    }

    protected override void OnInit()
    {
        _animator = Owner.GetComponentInChildren<Animator>();
    }

    protected override TaskStatus OnUpdate()
    {
        if (_isEnter == false)
        {
            _isEnter = true;
            _animator.Play(_animationHash, 0, 0);

            _onEnterAction?.Invoke();

            return TaskStatus.Continue;
        }


        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            _isEnter = false;

            return TaskStatus.Success;
        }

        return TaskStatus.Continue;
    }    
}