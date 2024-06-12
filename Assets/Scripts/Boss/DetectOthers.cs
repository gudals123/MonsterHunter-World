using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectOthers : MonoBehaviour
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
            anjanath.AnjanathState = State.Tracking;
            anjanath.targetObjectPos = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anjanath.AnjanathState = State.Walk;
        }
    }
}
