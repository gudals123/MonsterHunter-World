using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
public class UnArmedLayer : MonoBehaviour
{
    public StateMachine unArmedLayer;

    public bool isB = true;

    private void Start()
    {
        unArmedLayer = new StateMachine();

        unArmedLayer.AddState("A_State", onLogic: state => PrintState("A_State"));
        unArmedLayer.AddState("B_State", onLogic: state => PrintState("B_State"));

        unArmedLayer.SetStartState("A_State");

        unArmedLayer.AddTwoWayTransition("B_State", "A_State", transition => isB);

        unArmedLayer.Init();
    }

    void Update()
    {
        unArmedLayer.OnLogic();
    }

    void PrintState(string state)
    {
        Debug.Log(state);
    }


}
