using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : Weapon
{

    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject catObj;
    private Player player;
    private Cat cat;
    private CinemachineImpulseSource impulseSource;

    protected override void Awake()
    {
        base.Awake();
        player = playerObj.GetComponent<Player>();
        cat = catObj.GetComponent<Cat>();
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
            if (chargeTime < 1)
            {
                attackDamage = 10;
            }
            else if (chargeTime < 2.5)
            {
                attackDamage = 20;
            }
            else
            {
                attackDamage = 40;
            }
        }
    }

/*    protected override void AppearHitEffect(Vector3 hitPos, float duration)
    {
        base.AppearHitEffect(hitPos, duration);
    }


    protected override IEnumerator CoHitEffect(Vector3 hitPos, float duration)
    {
        return base.CoHitEffect(hitPos, duration);
    }
*/
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss") || other.CompareTag("BossAttack"))
        {
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos);
            StartCoroutine(player.Snag());
            UIManager.Instance.PlayerDamageText(attackDamage, hitPos);
            impulseSource.GenerateImpulse();
            cat.SetTarget(other);
        }
    }
}
