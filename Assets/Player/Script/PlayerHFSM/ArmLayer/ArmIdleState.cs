using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class ArmIdleState : StateBase
{
    public ArmIdleState(bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
    }

    public override string GetActiveHierarchyPath()
    {
        return base.GetActiveHierarchyPath();
    }

    public override void Init()
    {
        base.Init();
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
