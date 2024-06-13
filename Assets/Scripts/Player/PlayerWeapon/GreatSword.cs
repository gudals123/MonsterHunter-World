using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : Weapon
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void AttackDamageSet(bool isRightAttack, float ChargeTime)
    {
        if (isRightAttack)
        {
            attackDamage = 15;
        }
        else
        {
            if(ChargeTime < 1)
            {
                attackDamage = 10;
            }
            else if(ChargeTime < 2.5)
            {
                attackDamage = 20;
            }
            else
            {
                attackDamage = 40;
            }
        }
    }

    protected override void AppearHitEffect(Vector3 hitPos, float duration)
    {
        base.AppearHitEffect(hitPos, duration);
    }


    protected override IEnumerator CoHitEffect(Vector3 hitPos, float duration)
    {
        return base.CoHitEffect(hitPos, duration);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
