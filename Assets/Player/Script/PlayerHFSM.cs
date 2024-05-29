using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class PlayerHFSM : MonoBehaviour
{
    StateMachine hfsm;

    private void Start()
    {
        //전체 FSM
        hfsm = new StateMachine();

        //비무장/무장 FSM
        StateMachine unarmed = new StateMachine();
        StateMachine arm = new StateMachine();

        //비무장 FSM 내부 FSM
        StateMachine nomalMoveLayer = new StateMachine();
        StateMachine nomalHitLayer = new StateMachine();

        //무장 FSM 내부 FSM
        StateMachine armMoveLayer = new StateMachine();
        StateMachine armHitLayer = new StateMachine();
        StateMachine battleLayer = new StateMachine();

        //상태 설정
        hfsm.AddState("Unarmed", unarmed);
        unarmed.AddState("NomalMoveLayer", nomalMoveLayer);
        nomalMoveLayer.AddState("NomalMove", onLogic: state => { });
        nomalMoveLayer.AddState("Run", onLogic: state => { });
        nomalMoveLayer.AddState("NomalAvoidance", onLogic: state => { });
        nomalMoveLayer.AddState("NomalFall", onLogic: state => { });
        unarmed.AddState("NomalHitLayer", nomalHitLayer);
        nomalHitLayer.AddState("NomalHit", onLogic: state => { });
        nomalHitLayer.AddState("NomalDeath", onLogic: state => { });
        unarmed.AddState("NomalIdle", onLogic: stat => { });
        hfsm.AddState("Arm", arm);
        arm.AddState("ArmMoveLayer", armMoveLayer);
        armMoveLayer.AddState("ArmMove", onLogic: state => { });
        armMoveLayer.AddState("ArmAvoidance", onLogic: state => { });
        armMoveLayer.AddState("ArmFall", onLogic: state => { });
        arm.AddState("ArmHitLayer", armHitLayer);
        armMoveLayer.AddState("ArmHit", onLogic: state => { }); 
        armMoveLayer.AddState("ArmDeath", onLogic: state => { });
        arm.AddState("BattleLayer", battleLayer);
        armMoveLayer.AddState("Charging", onLogic: state => { });
        armMoveLayer.AddState("Attack1", onLogic: state => { });
        armMoveLayer.AddState("Attack2", onLogic: state => { });
        armMoveLayer.AddState("Attack3", onLogic: state => { });
        armMoveLayer.AddState("Attack4", onLogic: state => { });
        arm.AddState("ArmIdle", onLogic: stat => { });




    }


}
