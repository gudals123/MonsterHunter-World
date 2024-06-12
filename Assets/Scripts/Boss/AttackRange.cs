using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private Anjanath anjanath;

    private void Awake()
    {
        anjanath = GetComponentInParent<Anjanath>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //anjanath.AnjanathState = State.Attack;
            anjanath.SetAttackState();
            int sum = anjanath.Attack();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anjanath.isPlayerInAttackRange = false;
            anjanath.AnjanathState = State.Tracking;
        }
    }

}
