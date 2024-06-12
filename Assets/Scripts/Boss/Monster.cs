using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Entity
{
    public int grade { get; set; }
    protected Vector3 targetPos;
    protected int rotationSpeed;
    protected bool canAttack;
    protected bool setHit;

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
        float targetX = Random.Range(transform.position.x - 100, transform.position.x + 100);
        float targetZ = Random.Range(transform.position.z - 100, transform.position.z + 100);
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
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);  // 메쉬를 제외한 자식 오브젝트 모두 끔
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    virtual public void Idle()
    {
        animator.Play("Idle");
        targetPos = SetRandomPos();
    }

    virtual public void NormalMoving(float moveSpeed)
    {
        animator.Play("NormalWalking");
        // GroundCheck 필요
        Move(moveSpeed, targetPos);
    }
    
    public void TrackingPlayer(Transform targetTr)
    { 
        // 현재 애니메이션 상태를 가져옴
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 공격 애니메이션이 끝났는지 확인
        if (stateInfo.IsName("NormalAttack") && stateInfo.normalizedTime < 1.0f)
        {
            // 공격 애니메이션이 아직 끝나지 않음
            return;
        }

        // 공격 애니메이션이 끝났거나 공격 애니메이션이 아닐 경우 트래킹 시작
        else
        {
            animator.Play("BattleTracking");
            Move(5, targetTr.position);
        }
    }


}
