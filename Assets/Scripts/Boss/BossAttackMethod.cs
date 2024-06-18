using System.Collections;
using UnityEngine;

public class BossAttackMethod : Weapon
{
    protected int weightValue;
    protected bool isBossAttacking;
    protected bool isBossValidAttack;
    protected float startDuration;
    protected float endDuration;
    protected float endAttack;

    protected virtual void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Attack을 한 번 실행하는 동안 공격이 중첩되지 않도록 
    /// isBossAttacking을 잠시 켰다 꺼는 코루틴 CoBossAttack을 실행합니다.
    /// </summary>
    public void NowBossAttacking()
    {
        if (isBossValidAttack)
        {
            StartCoroutine(CoBossAttack());
        }
    }

    protected IEnumerator CoBossAttack()
    {
        isBossValidAttack = false;
        yield return new WaitForSeconds(startDuration);
        isBossAttacking = true;
        yield return new WaitForSeconds(endDuration);
        isBossAttacking = false;
        yield return new WaitForSeconds(endAttack);
        isBossValidAttack = true;
    }

}
