using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSetActive : MonoBehaviour
{
    [SerializeField] private BoxCollider playerDetectBox;
    [SerializeField] private GameObject boss;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            boss.SetActive(true);
        }
    }
}
