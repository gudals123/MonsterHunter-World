using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Burst.Intrinsics;
using UnityEditor.Hardware;
using UnityEngine;
using UnityHFSM;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TestHFSM : MonoBehaviour
{
    private StateMachine hfsm;
    public bool isLayer;
    public bool isB;



    private void Start()
    {
        //전체 FSM
        hfsm = new StateMachine();
        StateMachine layer = new StateMachine();    

        hfsm.AddState("Layer", layer);
        hfsm.AddState("Idle", onLogic: state => PrintState("Idle_State"));
        hfsm.AddTwoWayTransition("Layer", "Idle", transition => isLayer);
        //hfsm.AddTransition("Layer", "Idle", transition => !isLayer);
        //hfsm.AddTransition("Idle", "Layer", transition => isLayer);
        hfsm.SetStartState("Idle");
        
        
        layer.AddState("A_State", onLogic: state => PrintState("A_State"));
        layer.AddState("B_State", onLogic: state => PrintState("B_State"));
        layer.AddTwoWayTransition("B_State", "A_State", transition => isB);
        layer.SetStartState("A_State");

        hfsm.Init();
    }
    private void Update()
    {
        hfsm.OnLogic();

    }


    void PrintState(string state)
    {
        Debug.Log(state);
    }

}