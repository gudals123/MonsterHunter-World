using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : Node
{
    Action _action;
    bool _isSuccess;

    public ActionNode(Action action, bool isSuccess = true)
    {
        _action = action;
        _isSuccess = isSuccess;
    }

    public override NodeState Run()
    {
        _action();
        return _isSuccess? NodeState.Success : NodeState.Failure;
    }
}
