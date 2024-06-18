using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Anjanath : Monster
{
    public NormalAttackMethod normalattackMethod;
    public GameObject breathAttackMethod;
    public Transform playerTr;
    public Transform attackTr;
    public Transform targetTr;
    public bool startNormalAttaking;
    public bool startBreathAttaking;
    public bool isSturn;
    public bool leaveHere;

    private AnjanathBT anjanathBT;
    private State currentState;
    private float distancePtoB;
    private float distancePerception;
    private float perceptionTime = 0;
    private int DamageStack;
    private int WeaponDamage;
    private bool isBossRecognized;
    private bool isBusy;

    private void Awake()
    {
        arrivalPos = Instantiate(arrivalPos).GetComponent<Transform>();
        anjanathBT = GetComponent<AnjanathBT>();
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        anjanathBT.anjanathState = State.Idle;
        isArrivalTargetPos = true;
        targetTr = playerTr;
        maxHp = 500;
        currentHp = maxHp;
        rotationSpeed = 100;
        setHit = false;
        isDead = false;
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

    public override void Sturn()
    {
        base.Sturn();
        isSturn = false;
    }

    public override void GetHit()
    {
        base.GetHit();
        getHit = false;
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
        DamageStack++;
        if(DamageStack == 8)
        {
            DamageStack = 0;
        }

        else if(DamageStack % 3 == 0)
        {
            getHit = true;
        }
    }

    public IEnumerator CoAfterState(float delayTime, bool some)
    {
        if (isBusy)
        {
            yield return new WaitForSeconds(delayTime);
            anjanathBT.anjanathState = currentState;
            isBusy = false;
        }
        some = false;
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

    public void LeaveHere()
    {
        //animator.Play("BattleTracking");
        Move(4, arrivalPos);
    }

    public void IsPlayerInRange()
    {
        distancePtoB = Vector3.Distance(targetTr.position, transform.position);
        Vector3 normalized = (targetTr.position - transform.position).normalized;
        float _isForward = Vector3.Dot(normalized, transform.forward);

        // Raycast에 닿으면 공격
        Debug.DrawRay(attackTr.position, transform.forward * 5f, Color.yellow);
        RaycastHit hit;

        if (!isBossRecognized && distancePtoB <= 18f)
        {
            perceptionTime += Time.deltaTime;

            if (perceptionTime >= 2f)
            {
                perceptionTime = 0;
                isBossRecognized = true;
                targetTr = playerTr;
            }
        }

        else if (isBossRecognized && Physics.Raycast(attackTr.position, transform.forward, out hit, 5f))
        {
            if (hit.collider.gameObject.name == targetTr.gameObject.name)
            {
                SetAttackState();
            }
        }

        else if (isBossRecognized && distancePtoB >= 18f)
        {
            perceptionTime += Time.deltaTime;

            if (perceptionTime >= 20f)
            {
                isBossRecognized = false;
                perceptionTime = 0;
            }
        }

        else if(isBossRecognized && distancePtoB <= 7)
        {
            anjanathBT.anjanathState = State.Finding;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            isArrivalTargetPos = true;
        }

        if (other.CompareTag("Weapon"))
        {
            targetTr = other.transform.root;
            currentState = anjanathBT.anjanathState;
            WeaponDamage = other.gameObject.GetComponent<Weapon>().attackDamage;
            if (weakness)
            {
                WeaponDamage *= 2;
                weakness = false;
            }
            Debug.Log(WeaponDamage);
            isBossRecognized = true;
            Hit(WeaponDamage);
            Vector3 hitPos = other.ClosestPoint(transform.position);
            UIManager.Instance.PlayerDamageText(WeaponDamage, hitPos);
        }
    }

    private void OnDrawGizmos()
    {
        if (isBossRecognized)
        {
            // 인식 범위
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 11f);

            // 공격 범위
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(attackTr.position, attackTr.position + transform.forward * 5f);

            // 추적 범위
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 7f);
        }
        else
        {
            // 기본 인식 범위
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 11f);
        }
    }
}
