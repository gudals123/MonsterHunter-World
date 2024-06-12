using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anjanath : Monster
{
    public State AnjanathState;
    public Vector3 targetObjectPos;
    public int DamageStack;

    public bool startNormalAttaking;
    public bool startBreathAttaking;

    public bool isPlayerInAttackRange;
    public bool isPlayerInVisualRange;

    public float distancePtoB { get; private set; }
    public bool _bossAttackRange { get; private set; }
    public bool _bossVisualRange { get; private set; }
    public bool _bossPerceptionRange { get; private set; }
    public float _playerAttackDamege { get; private set; }



    private void Awake()
    {
        grade = 1;
        maxHp = 2000;
        currentHp = maxHp;
        rigidbody = GetComponent<Rigidbody>();
        startPosition = rigidbody.position;
        rotationSpeed = 100;
        setHit = false;
        animator = GetComponentInChildren<Animator>();
    }

    public void SetAttackState()
    {
        if (currentHp <= 600 && SetChance())
        {
            Debug.Log("Breath@@@@@@@@@@@");
            startBreathAttaking = true;
            startNormalAttaking = false;
        }
        else
        {
            Debug.Log("Normal###########");
            startBreathAttaking = false;
            startNormalAttaking = true;
        }
    }

    public void BreathAttacking()
    {
        attackDamage = 5;
        animator.Play("BreathAttack");
    }

    public void NormalAttacking()
    {
        attackDamage = 10;
        animator.Play("NormalAttack");
    }

    public override void SetDamage(int damage)
    {
        AnjanathState = State.SetDamage;

        base.SetDamage(damage);

        if(currentHp <= 0)
        {
            Dead();
        }

        else if(DamageStack == 5)
        {
            animator.Play("Sturn");
        }

        else if(DamageStack == 2)
        {
            animator.Play("Hit");
        }
    }

    public IEnumerator AnjanathAttack(GameObject AttackObj, float duration)
    {
        canAttack = false;
        AttackObj.SetActive(true);
        yield return new WaitForSeconds(duration);
        AttackObj.SetActive(false);
        canAttack = true;
    }

    public bool SetChance()
    {
        return Random.Range(0f, 1.0f) <= 0.5f? true: false;
    }


    public void TrackingPlayer()
    {
        animator.Play("BattleTracking");
        // GroundCheck 필요함
        Move(5, targetObjectPos);
    }

    public void isPlayerInRange(Transform player, Transform boss)
    {
        distancePtoB = Vector3.Distance(player.position, boss.position);

        Vector3 normalized = (player.position - boss.position).normalized;
        float _isForward = Vector3.Dot(normalized, boss.forward);

        // 공격 범위
        if (_isForward > 0 && distancePtoB <= 7f)
        {
            Debug.Log("공격 범위");
            AnjanathState = State.Attack;
            SetAttackState();
        }

        // 시야 범위
        else if (_isForward > 0 && distancePtoB <= 20f)
        {
            Debug.Log("시야 범위");
            AnjanathState = State.Tracking;
        }

        // 인지 범위
        else if (distancePtoB <= 18f)
        {
            Debug.Log("인지 범위");
            AnjanathState = State.Walk;
        }

        else
        {
            Debug.Log("아무것도");
            AnjanathState = State.Idle;
        }

    }

}
