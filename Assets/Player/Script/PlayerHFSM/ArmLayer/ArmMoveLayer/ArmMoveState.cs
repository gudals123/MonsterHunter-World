using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
public class ArmMove : StateBase
{
    public ArmMove(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }

    public override string GetActiveHierarchyPath()
    {
        return base.GetActiveHierarchyPath();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnExitRequest()
    {
        base.OnExitRequest();
    }

    public override void OnLogic()
    {
        base.OnLogic();
    }
}
