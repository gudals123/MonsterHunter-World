using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
public class ArmLayer : StateMachine
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ArmLayer");
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
