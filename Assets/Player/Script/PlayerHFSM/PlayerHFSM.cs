using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityHFSM;

public class PlayerHFSM : MonoBehaviour
{
    private StateMachine hfsm;
    private bool isArm;
    private bool isHit;


    private ArmLayer armLayer;
    private UnArmedLayer unArmedLayer;
    private HitLayer hitLayer;


    private void Start()
    {        
        
        //ÀüÃ¼ FSM
        hfsm = new StateMachine();
        armLayer = new ArmLayer();
        unArmedLayer = new UnArmedLayer();
        hitLayer = new HitLayer();


        hfsm.AddState("UnArmedLayer", unArmedLayer);
        hfsm.AddState("ArmLayer", armLayer);
        hfsm.AddState("HitLayer", hitLayer);

        hfsm.SetStartState("UnArmedLayer");

        hfsm.AddTwoWayTransition(new Transition("ArmLayer", "UnArmedLayer", transition => isArm));
        hfsm.AddTwoWayTransition(new Transition("HitLayer", "ArmLayer", transition => isHit));
        hfsm.AddTwoWayTransition(new Transition("HitLayer", "UnArmedLayer", transition => isHit));



    }


}
