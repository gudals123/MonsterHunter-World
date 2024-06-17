using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : Weapon
{

    [SerializeField] private GameObject playerObj;
    private Player player;
    private CinemachineImpulseSource impulseSource;

    protected override void Awake()
    {
        base.Awake();
        player = playerObj.GetComponent<Player>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void AttackDamageSet(bool isRightAttack, float chargeTime)
    {
        if (isRightAttack)
        {
            attackDamage = 15;
        }
        else
        {
            if(chargeTime < 1)
            {
                attackDamage = 10;
            }
            else if(chargeTime < 2.5)
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
        if(other.CompareTag("Boss") || other.CompareTag("Monster"))
        {
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, 0.1f);
            StartCoroutine(player.Snag());
            impulseSource.GenerateImpulse();

        }
        //Cat.��Ÿ������(other);
    }


}
