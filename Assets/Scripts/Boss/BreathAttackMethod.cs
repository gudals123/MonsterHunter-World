using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathAttackMethod : BossAttackMethod
{
    public Transform _breathObject;

    override protected void Awake()
    {
        bossAttackValue = 5;
        weightValue = 2;

        isBossAttacking = false;
        isBossValidAttack = true;

/*        startDuration = 0.5f;
        endDuration = 1f;
        endAttack = 1.5f;*/

        base.Awake();
        _breathObject.gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("BossAttackComplete");
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, 1f);
        }
    }

}
