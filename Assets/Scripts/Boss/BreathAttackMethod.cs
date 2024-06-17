using System.Collections.Generic;
using UnityEngine;

public class BreathAttackMethod : BossAttackMethod
{
    override protected void Awake()
    {
        attackDamage = 5;
        weightValue = 2;

        isBossAttacking = false;
        isBossValidAttack = true;

        startDuration = 0f;
        endDuration = 2.9f;
        endAttack = 0f;

        base.Awake();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, 1f);
        }
    }

}
