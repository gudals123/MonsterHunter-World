using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathTrigger : MonoBehaviour
{
    [SerializeField] int breathAttack;

    private void Start()
    {
        gameObject.SetActive(false);
        breathAttack = 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        CombatManager.TakeDamage("Boss", breathAttack);
        Debug.Log($"Breath!!! Attack to Player!!!!! Player HP : {CombatManager._currentPlayerHP}");
    }
}
