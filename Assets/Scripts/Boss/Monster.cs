using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Entity
{
    public GameObject arrivalPosPrefab;
    public Transform arrivalPos;
    public Transform bossRecognizeTr;
    public bool getHit;
    public bool isSetTargetPos;
    public bool weakness;
    public bool isDead;
    protected AnimatorStateInfo stateInfo;
    protected int rotationSpeed;
    protected bool canAttack;
    protected bool setHit;
    protected bool isArrivalTargetPos;


    public override void Move(float moveSpeed, Transform targetPos)
    {
        Vector3 direction = (targetPos.position - transform.position).normalized;
        Vector3 bossDirection = new Vector3(direction.x, 0, direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(bossDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        rigidbody.MoveRotation(rotation);
        rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
    }

    public Vector3 SetRandomPos()
    {
        float targetX = Random.Range(-64, 64);
        float targetZ = Random.Range(97, 227);
        return new Vector3(targetX, transform.position.y, targetZ);
    }

    public override void Hit(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) isDead = true;
    }

    public void Dead()
    {
        animator.Play("Die");
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    virtual public void Sturn()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    virtual public void GetHit()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    virtual public void Idle()
    {
        animator.Play("Idle");

        if (isArrivalTargetPos)
        {
            arrivalPos.position = SetRandomPos();
            isSetTargetPos = true;
            isArrivalTargetPos = false;
        }
    }

    virtual public void NormalMoving(float moveSpeed)
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Idle") && stateInfo.normalizedTime < 1.0f)
        {
            return;
        }

        animator.Play("NormalWalking");
        // GroundCheck
        Move(moveSpeed, arrivalPos);

        if (isArrivalTargetPos)
        {
            isSetTargetPos = false;
        }
    }
    
    public void TrackingPlayer(Transform targetTr)
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("NormalAttack") || stateInfo.IsName("BreathAttack"))
        {
            if (stateInfo.normalizedTime < 1.0f)
                return;
        }

        // 애니메이션이 끝났다면 추적 상태로 전환
        if (!stateInfo.IsName("BattleTracking"))
        {
            animator.Play("BattleTracking");
        }
        Move(4, targetTr);
    }

    public bool SetChance(float setValue)
    {
        return Random.Range(0f, 1.0f) <= setValue ? true : false;
    }

}
