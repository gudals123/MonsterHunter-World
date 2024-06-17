using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Entity
{
    public int grade { get; set; }
    public Vector3 targetPos;
    protected int rotationSpeed;
    protected bool canAttack;
    protected bool setHit;
    public bool isArrivalTargetPos;
    public bool isSetTargetPos;
    public Transform arrivalPos;
    protected AnimatorStateInfo stateInfo;

    public override int Attack()
    {
        setHit = true;
        setHit = false;
        return 1;
    }

    public override void Move(float moveSpeed, Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        rigidbody.MoveRotation(rotation);
        rigidbody.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public Vector3 SetRandomPos()
    {
        float targetX = Random.Range(-60, 60);
        float targetZ = Random.Range(-50, 50);
        return new Vector3(targetX, transform.position.y, targetZ);
    }

    public override void Hit(int damage)
    {
        currentHp -= damage;
    }

    virtual public void DetectOpponent(Monster Opponent)
    {
        if(Opponent.grade == grade)
        {
            Debug.Log("Same Grade!!!!");
        }
    }

    public void Dead()
    {
        animator.Play("Die");
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);  // �޽��� ������ �ڽ� ������Ʈ ��� ��
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
        Move(moveSpeed, arrivalPos.position);

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
        Move(4, targetTr.position);
    }

    public bool SetChance(float setValue)
    {
        return Random.Range(0f, 1.0f) <= setValue ? true : false;
    }

}
