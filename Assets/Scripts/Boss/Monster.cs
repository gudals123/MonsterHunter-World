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
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);  // �޽��� ������ �ڽ� ������Ʈ ��� ��
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
        // GroundCheck
        Move(moveSpeed, targetPos);
    }
    
    public void TrackingPlayer(Transform targetTr)
    { 
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("NormalAttack") && stateInfo.normalizedTime < 1.0f)
        {
            return;
        }
        else
        {
            animator.Play("BattleTracking");
            Move(5, targetTr.position);
        }
    }


}
