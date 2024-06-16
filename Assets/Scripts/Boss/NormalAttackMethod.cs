using System.Collections;
using UnityEngine;

public class NormalAttackMethod : BossAttackMethod
{
    override protected void Awake()
    {
        attackDamage = 10;
        weightValue = 1;

        isBossAttacking = false;
        isBossValidAttack = true;

        startDuration = 0.5f;
        endDuration = 1f;
        endAttack = 1.5f;

        base.Awake();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (isBossAttacking && other.CompareTag("Player"))
        {
            Debug.Log("BossAttackComplete");
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, 1f);
        }
    }

}
