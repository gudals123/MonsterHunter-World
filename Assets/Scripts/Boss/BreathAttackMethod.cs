using System.Collections.Generic;
using UnityEngine;

public class BreathAttackMethod : BossAttackMethod
{
    override protected void Awake()
    {
        bossAttackValue = 5;
        weightValue = 2;

        isBossAttacking = false;
        isBossValidAttack = true;

        startDuration = 1f;
        endDuration = 1f;
        endAttack = 0.5f;

        base.Awake();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("BreathAttackComplete");
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, 1f);
        }
    }

}
