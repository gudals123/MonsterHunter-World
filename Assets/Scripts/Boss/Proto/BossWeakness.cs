using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossWeakness : MonoBehaviour
{
    [SerializeField]
    private int _countHitByWeapon;

    private void Awake()
    {
        _countHitByWeapon = 0;
    }

    private void Update()
    {
        if (_countHitByWeapon == 5)
        {
            //CombatManager._isBossSturned = true;
            _countHitByWeapon = 0;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            _countHitByWeapon++;
        }
    }

}
