using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anjanath : Monster
{
    public AnjanathBT anjanathBT;

    public int DamageStack;
    public Transform playerTr;
    private float distancePtoB;

    protected bool checkTarget;
    protected Transform targetTr;    // ����� �Ǵ� �÷��̾�
    public int getOtherAttackDamage;

    public bool startNormalAttaking;
    public bool startBreathAttaking;

    public float perceptionTime = 0;
    public bool isBossRecognized;


    private void Awake()
    {
        anjanathBT.anjanathState = State.Idle;

        anjanathBT = GetComponent<AnjanathBT>();
        grade = 1;
        maxHp = 2000;
        currentHp = maxHp;
        rigidbody = GetComponent<Rigidbody>();
        rotationSpeed = 100;
        setHit = false;
        animator = GetComponentInChildren<Animator>();
        Idle();
    }

    public void BreathAttacking()
    {
        //attackDamage = 5;
        animator.Play("BreathAttack");
    }

    public void NormalAttacking()
    {
        //attackDamage = 10;
        animator.Play("NormalAttack");
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
        // targetTr = ���� ���� ����

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

    public void StartTracking()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            TrackingPlayer(playerTr);
        }
    }


    public bool SetChance()
    {
        return Random.Range(0f, 1.0f) <= 0.5f ? true : false;
    }

    public void SetAttackState()
    {
        isBossRecognized = true;

        if (currentHp <= 600 && SetChance())
        {
            startBreathAttaking = true;
            startNormalAttaking = false;
        }
        else
        {
            startBreathAttaking = false;
            startNormalAttaking = true;
        }
    }

    public void DetectPlayer()
    {
        if (isBossRecognized)
        {
            anjanathBT.anjanathState = State.Tracking;
        }

        else if (!isBossRecognized)
        {
            perceptionTime += Time.deltaTime;
            if(perceptionTime >= 2f)
            {
                perceptionTime = 0;
                isBossRecognized = true;
                targetTr = playerTr;
            }
        }
    }

    public void IsPlayerInRange(Transform target)
    {
        distancePtoB = Vector3.Distance(playerTr.position, transform.position);

        Vector3 normalized = (playerTr.position - transform.position).normalized;
        float _isForward = Vector3.Dot(normalized, transform.forward);

        // ���� ����
        if (_isForward > 0 && distancePtoB <= 15f)
        {
            anjanathBT.anjanathState = State.Attack;
            SetAttackState();
        }

        // �þ� ����
        else if (_isForward > 0 && distancePtoB <= 40f)
        {
            DetectPlayer();
        }

        else if (isBossRecognized && distancePtoB <= 40f)
        {
            anjanathBT.anjanathState = State.Tracking;
        }

        else
        {
            if (isBossRecognized)
            {
                perceptionTime += Time.deltaTime;
                if (perceptionTime >= 2f)
                {
                    perceptionTime = 0;
                    isBossRecognized = false;
                }
            }

            else
            {
                Debug.Log("!@#$%^&*%$#@!#$%^");
                if (SetChance())
                    anjanathBT.anjanathState = State.Walk;
                else
                    anjanathBT.anjanathState = State.Idle;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (gameObject != null)
        {
            // ���� ���� (15f)
            Gizmos.color = anjanathBT.anjanathState == State.Attack ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, 15f);

            // �þ� ���� / ���� ���� (40f)
            Gizmos.color = anjanathBT.anjanathState == State.Tracking? Color.yellow : Color.blue; ;
            Gizmos.DrawWireSphere(transform.position, 40f);
        }
    }

}
