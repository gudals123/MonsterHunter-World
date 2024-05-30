using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityHFSM;

public class PlayerHFSM : MonoBehaviour
{
    private StateMachine hfsm;
    public bool isArm;
    public bool isHit;
    public bool isMove;
    public bool isRun;
    public bool isGrounded;
    public bool isAvoidance;
    public float isCharging;


    private void Start()
    {
        //전체 FSM
        hfsm = new StateMachine();

        //
        StateMachine unarmed = new StateMachine();
        StateMachine arm = new StateMachine();
        StateMachine hitLayer = new StateMachine();

        //비무장 FSM 내부 FSM
        StateMachine nomalMoveLayer = new StateMachine();

        //무장 FSM 내부 FSM
        StateMachine armMoveLayer = new StateMachine();
        StateMachine battleLayer = new StateMachine();



        //전체 FSM
        hfsm.AddState("Unarmed", unarmed);
        unarmed.AddState("NomalMoveLayer", nomalMoveLayer);
        nomalMoveLayer.AddState("NomalMove", onLogic: state => PrintState("NomalMove"));
        nomalMoveLayer.AddState("Run", onLogic: state => PrintState("Run"));
        nomalMoveLayer.AddState("NomalAvoidance", onLogic: state => PrintState("NomalAvoidance"));
        nomalMoveLayer.AddState("NomalFall", onLogic: state => PrintState("NomalFall"));
        unarmed.AddState("NomalIdle", onLogic: stat => PrintState("NomalIdle"));

        hfsm.AddState("Arm", arm);
        arm.AddState("ArmMoveLayer", armMoveLayer);
        armMoveLayer.AddState("ArmMove", onLogic: state => PrintState("ArmMove"));
        armMoveLayer.AddState("ArmAvoidance", onLogic: state => PrintState("ArmAvoidance"));
        armMoveLayer.AddState("ArmFall", onLogic: state => PrintState("ArmFall"));
        arm.AddState("BattleLayer", battleLayer);
        battleLayer.AddState("Charging", onLogic: state => { PrintState("Charging"); isCharging += Time.deltaTime; });
        battleLayer.AddState("Attack1", onLogic: state => PrintState("Attack1"));
        battleLayer.AddState("Attack2", onLogic: state => PrintState("Attack2"));
        battleLayer.AddState("Attack3", onLogic: state => PrintState("Attack3"));
        battleLayer.AddState("Attack4", onLogic: state => PrintState("Attack4"));
        arm.AddState("ArmIdle", onLogic: state => PrintState("ArmIdle"));

        hfsm.AddState("HitLayer", hitLayer);
        hitLayer.AddState("Hit", onLogic: state => PrintState("Hit"));
        hitLayer.AddState("Dead", onLogic: state => PrintState("Dead"));

        //상태 전이
        //손
        hfsm.AddTwoWayTransition("Arm", "Unarmed", transition => isArm);
        hfsm.AddTwoWayTransition("HitLayer", "Arm", transition => isHit && isArm);
        hfsm.AddTwoWayTransition("HitLayer", "Unarmed", transition => isHit && !isArm);

        unarmed.AddTwoWayTransition("NomalMoveLayer", "NomalIdle", transition => isMove);

        nomalMoveLayer.AddTwoWayTransition("Run", "NomalMove", transition => isRun && isMove);
        nomalMoveLayer.AddTwoWayTransition("NomalAvoidance", "NomalMove", transition => isAvoidance && isGrounded);
        nomalMoveLayer.AddTransition("NomalFall", "NomalMove", transition => isGrounded);
        nomalMoveLayer.AddTransition("Run", "NomalMove", transition => isGrounded);
        nomalMoveLayer.AddTransition("NomalAvoidance", "NomalMove", transition => isGrounded);
        nomalMoveLayer.AddTransition("Run", "NomalAvoidance", transition => isGrounded);






        //육
        #region Layer
        arm.AddTwoWayTransition("ArmMoveLayer", "ArmIdle", transition => isMove && isArm);
        arm.AddTwoWayTransition("BattleLayer", "ArmIdle", transition => isHit && isArm);
        #endregion

        #region Move
        armMoveLayer.AddTransition("ArmMove", "ArmAvoidance", transition => isArm && isGrounded && isAvoidance);
        armMoveLayer.AddTransition("ArmAvoidance", "ArmMove", transition => isArm && isGrounded && !isAvoidance);
        armMoveLayer.AddTransition("ArmMove", "ArmFall", transition => isArm && isGrounded);
        armMoveLayer.AddTransition("ArmAvoidance", "ArmFall", transition => isArm && isGrounded);
        #endregion

        #region Battle
        battleLayer.AddTransition("Charging", "Attack1", transition => isArm && isGrounded && Input.GetMouseButtonUp(0) && isCharging <= 1f);
        battleLayer.AddTransition("Charging", "Attack2", transition => isArm && isGrounded && Input.GetMouseButtonUp(0) && isCharging > 2f);
        battleLayer.AddTransition("Attack1", "Charging", transition => isArm && isGrounded && Input.GetMouseButtonDown(0));
        battleLayer.AddTransition("Attack1", "Attack3", transition => isArm && isGrounded && Input.GetMouseButtonDown(1));
        battleLayer.AddTransition("Attack2", "Charging", transition => isArm && isGrounded && Input.GetMouseButtonDown(0));
        battleLayer.AddTransition("Attack2", "Attack3", transition => isArm && isGrounded && Input.GetMouseButtonDown(1));
        battleLayer.AddTransition("Attack3", "Charging", transition => isArm && isGrounded && Input.GetMouseButtonDown(0));
        battleLayer.AddTransition("Attack3", "Attack4", transition => isArm && isGrounded && Input.GetMouseButtonDown(1));
        battleLayer.AddTransition("Attack4", "Charging", transition => isArm && isGrounded && Input.GetMouseButtonDown(0));
        battleLayer.AddTransition("Attack4", "Attack3", transition => isArm && isGrounded && Input.GetMouseButtonDown(1));
        #endregion

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
