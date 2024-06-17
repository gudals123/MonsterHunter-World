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
    public bool isBossRecognized;
    public Transform targetTr;

    public bool startNormalAttaking;
    public bool startBreathAttaking;

    public float perceptionTime = 0;
    private bool isBusy;
    private State currentState;

    public int WeaponDamage { get; private set; }

    private void Awake()
    {
        anjanathBT.anjanathState = State.Idle;
        targetTr = playerTr;
        anjanathBT = GetComponent<AnjanathBT>();
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        grade = 1;
        maxHp = 300;
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
        DamageStack++;

        if (currentHp <= 0)
        {
            anjanathBT.anjanathState = State.Dead;
        }

        else if(DamageStack == 6)
        {
            DamageStack = 0;
        }

        else if(DamageStack == 5)
        {
            animator.Play("Sturn");
            afterState(2f);
            DamageStack = 0;
        }

        else if(DamageStack == 3)
        {
            animator.Play("Hit");
            afterState(0.26f);
        }
    }

    public IEnumerator afterState(float delayTime)
    {
        if (isBusy)
        {
            yield return new WaitForSeconds(delayTime);
            anjanathBT.anjanathState = currentState;
            isBusy = false;
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
            if(perceptionTime >= 7f)
            {
                perceptionTime = 0;
                targetTr = playerTr;
            }
        }
    }

    public void IsPlayerInRange()
    {
        distancePtoB = Vector3.Distance(playerTr.position, transform.position);

        // Raycast에 닿으면 공격
        Debug.DrawRay(attackTr.position, transform.forward * 5f, Color.yellow);
        RaycastHit hit;
        //Debug.Log(Physics.Raycast(attackTr.position, transform.forward, out hit, 5f));
        if (isBossRecognized && Physics.Raycast(attackTr.position, transform.forward, out hit, 5f))
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

        if (other.CompareTag("Weapon"))
        {
            isBusy = true;
            targetTr = other.transform;
            currentState = anjanathBT.anjanathState;
            anjanathBT.anjanathState = State.GetHit;
            WeaponDamage = other.gameObject.GetComponent<Weapon>().attackDamage;
            isBossRecognized = true;
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
                    isBossRecognized = true;
                    targetTr = playerTr;
                }
            }
        }
    }

}
