using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
public class HitLayer : StateMachine
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("HitLayer");
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
        //���� ��ȯ
    }
}
