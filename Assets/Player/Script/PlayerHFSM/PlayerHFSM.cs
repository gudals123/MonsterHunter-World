using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityHFSM;

public class PlayerHFSM : MonoBehaviour
{
    private StateMachine hfsm;
    public bool isTest;
    public bool isHit;
    public bool isMove;
    public bool isRun;


    /*    private ArmLayer armLayer;
        private UnArmedLayer unArmedLayer;
        private HitLayer hitLayer;*/


    private void Start()
    {

        //ÀüÃ¼ FSM
        hfsm = new StateMachine();

        var unArmedLayer = new UnArmedLayer();


        hfsm.AddState("Idle", onLogic: state => PrintState("Idle"));
        hfsm.AddState("UnArmedLayer", unArmedLayer.unArmedLayer);

        hfsm.AddTwoWayTransition("Idle", "UnArmedLayer", transition => isTest);
        hfsm.SetStartState("Idle");

       /* hfsm = new StateMachine();
        var MoveLayer = new StateMachine();



        hfsm.AddState("IdleState", onLogic: state => PrintState("Idle"));
        hfsm.AddState("HitState", onLogic: state => PrintState("HitState"));
        hfsm.AddState("MoveLayer", MoveLayer);
        MoveLayer.AddState("MoveState", onLogic: state => PrintState("MoveState"));
        MoveLayer.AddState("RunState", onLogic: state => PrintState("RunState"));
        hfsm.SetStartState("IdleState");
        MoveLayer.SetStartState("MoveState");

        hfsm.AddTwoWayTransition("IdleState", "HitState", transition => isHit);
        hfsm.AddTwoWayTransition("IdleState", "MoveLayer", transition => isMove);
        MoveLayer.AddTwoWayTransition("RunState", "MoveState", transition => isRun);*/



        /* 
        unArmedLayer = new UnArmedLayer();

        armLayer = new ArmLayer();
        hitLayer = new HitLayer();


        hfsm.AddState("UnArmedLayer", unArmedLayer);
        hfsm.SetStartState("UnArmedLayer");
               hfsm.AddState("ArmLayer", armLayer);
              hfsm.AddState("HitLayer", hitLayer);

              

              hfsm.AddTwoWayTransition(new Transition("ArmLayer", "UnArmedLayer", transition => isArm));
              hfsm.AddTwoWayTransition(new Transition("HitLayer", "ArmLayer", transition => isHit));
              hfsm.AddTwoWayTransition(new Transition("HitLayer", "UnArmedLayer", transition => isHit));*/

        hfsm.Init();
    }

    void Update()
    {
        hfsm.OnLogic();
    }

    void PrintState(string state)
    {
        Debug.Log(state);
    }

}
