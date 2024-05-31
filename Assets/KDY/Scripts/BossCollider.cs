using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BossCollider : MonoBehaviour
{
    [SerializeField] GameObject nomalAttackCollider;
    [SerializeField] int nomalAttack;

    private void Start()
    {
        nomalAttack = 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CombatManager.TakeDamage("Boss", nomalAttack);
            Debug.Log($"NomalAttack to Player!!!!! Player HP : {CombatManager._currentPlayerHP}");
        }
        
    }

}
