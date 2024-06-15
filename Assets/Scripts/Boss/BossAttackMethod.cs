using System.Collections;
using UnityEngine;

public class BossAttackMethod : Weapon
{
    [SerializeField] protected int bossAttackValue;
    [SerializeField] protected int weightValue;

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
        Debug.Log("공격 시작");
        yield return new WaitForSeconds(startDuration);
        isBossAttacking = true;
        Debug.Log("딜 감지 시작");
        yield return new WaitForSeconds(endDuration);
        isBossAttacking = false;
        Debug.Log("딜 감지 끝");
        yield return new WaitForSeconds(endAttack);
        isBossValidAttack = true;
        Debug.Log("공격 끝");
    }

}
