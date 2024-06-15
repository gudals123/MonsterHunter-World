using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anjanath : Monster
{
    public AnjanathBT anjanathBT;
    public NormalAttackMethod normalattackMethod;
    public GameObject breathAttackMethod;

    public int DamageStack;
    public Transform playerTr;
    public Transform attackTr;
    private float distancePtoB;
    private float distanceTtoB;
    public bool isBossRecognized;
    public Transform targetTr;
    public int getOtherAttackDamage;

    public bool startNormalAttaking;
    public bool startBreathAttaking;

    public float perceptionTime = 0;

    private void Awake()
    {
        anjanathBT.anjanathState = State.Idle;
        targetTr = playerTr;
        anjanathBT = GetComponent<AnjanathBT>();
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        grade = 1;
        maxHp = 2000;
        currentHp = maxHp;
        rotationSpeed = 100;
        setHit = false;
        Idle();
    }

    public void BreathAttacking()
    {
        breathAttackMethod.SetActive(true);
    }

    public void NormalAttacking()
    {
        breathAttackMethod.SetActive(false);
        normalattackMethod.NowBossAttacking();
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);

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
        TrackingPlayer(targetTr);
    }

    public void SetAttackState()
    {
        anjanathBT.anjanathState = State.Attack;
        isBossRecognized = true;
        perceptionTime = 0;

        if (SetChance(0.5f))
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

    public void IsPlayerInRange()
    {
        distancePtoB = Vector3.Distance(playerTr.position, transform.position);
        distanceTtoB = Vector3.Distance(targetTr.position, transform.position);

        // Raycast에 닿으면 공격
        Debug.DrawRay(attackTr.position, transform.forward * 5f, Color.yellow);
        RaycastHit hit;

        if (Physics.Raycast(attackTr.position, transform.forward, out hit, 5f) && isBossRecognized)
        {
            if (hit.collider.gameObject.name == targetTr.gameObject.name)
            {
                SetAttackState();
            }
        }

        else if (isBossRecognized && distancePtoB >= 11)
        {
            perceptionTime += Time.deltaTime;

            if (perceptionTime >= 20f)
            {
                isBossRecognized = false;
                perceptionTime = 0;
            }
        }

        // 인식 범위 - capsule Collider 닿으면 인식
        else if (isBossRecognized)
        {
            anjanathBT.anjanathState = State.Tracking;
        }

        else if (!isBossRecognized)
        {
            anjanathBT.anjanathState = State.Idle;
        }
    }

    private void OnDrawGizmos()
    {
        if (gameObject != null)
        {
            Gizmos.color = anjanathBT.anjanathState == State.Attack ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, 5f);
            Gizmos.color = anjanathBT.anjanathState == State.Tracking? Color.yellow : Color.blue; ;
            Gizmos.DrawWireSphere(transform.position, 12f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            isArrivalTargetPos = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isBossRecognized)
        {
            anjanathBT.anjanathState = State.Tracking;
        }

        else if (!isBossRecognized)
        {
            if (other.CompareTag("Player"))
            {
                perceptionTime += Time.deltaTime;

                if (perceptionTime >= 2f)
                {
                    perceptionTime = 0;
                    {
                        isBossRecognized = true;
                        targetTr = playerTr;
                    }
                }
            }
        }
    }

}
