using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathAttackTrigger : MonoBehaviour
{
    [SerializeField] private int _breathAttack;

    private void Start()
    {
        gameObject.SetActive(false);
        _breathAttack = 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        CombatManager.Instance.TakeDamage("Boss", _breathAttack);
    }
}
